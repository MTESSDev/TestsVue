using AngleSharp;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stubble.Core.Builders;
using Stubble.Core.Loaders;
using Stubble.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using ECSForm.Utils;
using ECSForm.Model;

namespace ECSForm.Pages
{
    public class GenericModel : PageModel
    {
        private readonly ILogger<GenericModel> _logger;
        private readonly IVueParser _vueParser;

        //public DynamicForm? Form { get; set; }
        public string? Layout { get; set; } = "_Layout";
        public string Created { get; set; } = string.Empty;
        public string? FormRaw { get; set; }
        public Dictionary<string, object?> VueData { get; set; } = new Dictionary<string, object?>();

        [VueData("formErrors")]
        public object[] FormErrors { get; set; }

        [VueData("inputErrors")]
        public Dictionary<string, string> InputErrors { get; set; }

        [VueData("config")]
        public Dictionary<string, object?> Config { get; set; }

        [VueData("form")]
        public dynamic Form { get; set; }

        [VueData("noPageCourante")]
        public int NoPageCourante { get; set; } = 0;

        [VueData("pages")]
        public List<Section>? Sections { get; set; } = new List<Section>();

        public static IDeserializer deserializer = new DeserializerBuilder()
                             .WithNamingConvention(CamelCaseNamingConvention.Instance)
                             .IgnoreUnmatchedProperties()
                             .Build();

        public GenericModel(ILogger<GenericModel> logger, IVueParser vueParser)
        {
            _logger = logger;
            _vueParser = vueParser;

            InputErrors = new Dictionary<string, string>();
            FormErrors = new object[0];
            Config = new Dictionary<string, object?>();
            Form = new { };
        }

        public async Task<IActionResult> OnPost(string? id, string render, int currentCursorLine, [FromQuery] Guid? uniqueID)
        {
            Layout = null;
            string? readFile = null;
            var cfg = Base64Decode(render);

            if (uniqueID.HasValue)
            {
                if (uniqueID.Value != Guid.Empty)
                {

                    readFile = "snapshots/" + uniqueID.Value.ToString();
                    System.IO.File.WriteAllText(@$"schemas/{readFile}.ecsform.yml", cfg);
                }
            }

            if (currentCursorLine != -1)
            {
                var currentLine = -1;
                var nameBlock = "";
                var sectionId = -1;

                using (StringReader reader = new StringReader(cfg))
                {
                    if (reader is null) return NotFound();

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        currentLine++;

                        if (line.StartsWith("    -"))
                        {
                            sectionId++;
                        }

                        if (line.Contains("        - name:") || line.Contains("        name:"))
                        {
                            nameBlock = line.Trim(' ', '-');
                        }

                        if (currentLine < currentCursorLine - 1)
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                nameBlock = nameBlock.Replace("name", "");
                nameBlock = nameBlock.TrimStart(' ', ':');

                Created = @$"this.effectuerNavigation({sectionId}, '{nameBlock}', true); ";
            }

            if (readFile is null)
            {
                return await RenderPage(id, cfg);
            }
            else
            {
                return await RenderPage(readFile);
            }
        }

        public async Task<IActionResult> OnGet(string? id)
        {
            return await RenderPage(id);
        }

        private async Task<IActionResult> RenderPage(string? id, string? render = null)
        {
            var configName = id.Replace('@', '/') ?? "default";
            Config.Add("keepData", false);
            Config.Add("configName", configName);

            /* Section TEST pour le v-if "SERVER-SIDE" */
            /*var lambdaParser = new NReco.Linq.LambdaParser();
            var varContext = new Dictionary<string, object>();
            varContext["pi"] = 3.14;
            var equation = "pi===3.14";
            //Normalize JS to C#
            equation = equation.Replace("===", "==");
            var ttt = lambdaParser.Eval(equation, varContext);*/

            DynamicForm? dynamicForm = null;
            DynamicForm? defaultCfg = null;

            if (System.IO.File.Exists(@$"schemas/default.ecsform.yml"))
            {
                defaultCfg = ReadYamlCfg(@$"schemas/default.ecsform.yml");
            }

            if (id == "render" && render != null)
            {
                dynamicForm = deserializer.Deserialize<DynamicForm>(render);
            }
            else
            {
                if (!System.IO.File.Exists(@$"schemas/{configName}.ecsform.yml"))
                {
                    return NotFound();
                }

                dynamicForm = ReadYamlCfg(@$"schemas/{configName}.ecsform.yml");
            }

            if (dynamicForm is null || dynamicForm.Form is null || dynamicForm?.Form?.Sections is null) { return NotFound(); }

            if (defaultCfg != null)
            {
                dynamicForm.Form.InputDefaultClasses.TryAdd(defaultCfg?.Form?.InputDefaultClasses);
                dynamicForm.Form.Templates.TryAdd(defaultCfg?.Form?.Templates);
            }

            FormHelpers.TemplateList = dynamicForm.Form?.Templates;
            FormHelpers.InputDefaultClasses = dynamicForm.Form?.InputDefaultClasses;

            using (StreamReader streamReader = new StreamReader(@"schemas/formTemplate.vue", Encoding.UTF8))
            {
                var content = await streamReader.ReadToEndAsync();
                FormRaw = await FormHelpers.Stubble.RenderAsync(content, dynamicForm);
            }

            if (dynamicForm?.Form?.Sections is null) { return NotFound(); }

            var sectionId = 0;
            foreach (var section in dynamicForm.Form.Sections)
            {
                Sections?.Add(new Section()
                {
                    No = sectionId++,
                    Id = section.Id,
                    Titre = section.Section.GetLocalizedObject() ?? "Title not found",
                    VIf = section.VIf
                });
            }

            // Restore last state
            HttpContext.Request.Cookies.TryGetValue($"ECSForm{configName}", out var form);

            if (string.IsNullOrEmpty(form))
                Form = new { validAll = false };
            else
                Form = JsonConvert.DeserializeObject<dynamic>(form);

            // Parse Vue data
            VueData = _vueParser.ParseData(this);

            return Page();
        }

        private DynamicForm ReadYamlCfg(string filename)
        {
            DynamicForm cfg;
            using (var configFile = new StreamReader(filename))
            {
                cfg = deserializer.Deserialize<DynamicForm>(configFile);
            }
            return cfg;
        }

        private static bool TryValidate(object value, ValidationContext validationContext, ValidationAttribute attribute, out ValidationError validationError)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException("validationContext");
            }

            ValidationResult validationResult = attribute.GetValidationResult(value, validationContext);
            if (validationResult != ValidationResult.Success)
            {
                validationError = new ValidationError(attribute, value, validationResult);
                return false;
            }

            validationError = null;
            return true;
        }
        /// <summary>
        /// Private helper class to encapsulate a ValidationAttribute with the failed value and the user-visible
        /// target name against which it was validated.
        /// </summary>
        private class ValidationError
        {
            internal ValidationError(ValidationAttribute validationAttribute, object value, ValidationResult validationResult)
            {
                this.ValidationAttribute = validationAttribute;
                this.ValidationResult = validationResult;
                this.Value = value;
            }

            internal object Value { get; set; }

            internal ValidationAttribute ValidationAttribute { get; set; }

            internal ValidationResult ValidationResult { get; set; }

            internal void ThrowValidationException()
            {
                throw new ValidationException(this.ValidationResult, this.ValidationAttribute, this.Value);
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        /*  private void ParseValidation(ref FormData validations, string name, string value, Dictionary<string, string> msg)
          {
              if (validations.Elements == null) { validations.Elements = new Dictionary<string, Validation>(); }



              validations.Elements.Add(name, new Validation());
          }*/
    }
}
