using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Globalization;
using FRW.PR.Extra.Utils;
using FRW.PR.Extra.Model;
using FRW.TR.Commun;
using FRW.TR.Commun.Utils;

namespace FRW.PR.Extra.Pages
{
    public class GenericModel : PageModel
    {
        private readonly ILogger<GenericModel> _logger;
        private readonly IVueParser _vueParser;

        public string? Layout { get; set; } = "_Layout";
        public string Language => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        public string Created { get; set; } = string.Empty;
        public string? FormRaw { get; set; }
        public Dictionary<string, object?> VueData { get; set; } = new Dictionary<string, object?>();

        [FromQuery]
        public bool ShowAll { get; set; }

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

        [VueData("title")]
        public string? Title { get; set; }

        [VueData("pagesGroup")]
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
                    //if (reader is null) return NotFound();

                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        currentLine++;

                        if (line.StartsWith("        -"))
                        {
                            sectionId++;
                        }

                        if (line.Contains("            - name:") || line.Contains("              name:"))
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

        public async Task<IActionResult> OnPostNew(string? id, string render, string breadcrumb, [FromQuery] Guid? uniqueID)
        {
            /* Layout = null;
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
                     //if (reader is null) return NotFound();

                     string? line;
                     while ((line = reader.ReadLine()) != null)
                     {
                         currentLine++;

                         if (line.StartsWith("        -"))
                         {
                             sectionId++;
                         }

                         if (line.Contains("            - name:") || line.Contains("              name:"))
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
             }*/

            return NotFound();
        }

        public async Task<IActionResult> OnGet(string? id)
        {
            return await RenderPage(id);
        }

        private async Task<IActionResult> RenderPage(string? id, string? render = null)
        {
            var configName = id?.Replace('@', '/') ?? "default";
            Config.Add("keepData", false);
            Config.Add("configName", configName);

            DynamicForm? defaultCfg = null;
            DynamicForm? dynamicForm;

            if (System.IO.File.Exists(@$"schemas/default.ecsform.yml"))
            {
                defaultCfg = OutilsYaml.LireFicher<DynamicForm>(@$"schemas/default.ecsform.yml");
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

                dynamicForm = OutilsYaml.LireFicher<DynamicForm>(@$"schemas/{configName}.ecsform.yml");
            }

            if (dynamicForm is null || dynamicForm.Form is null || dynamicForm?.Form?["sectionsGroup"] is null) { return NotFound(); }

            ///* Section TEST pour le v-if "SERVER-SIDE" */
            //var lambdaParser = new NReco.Linq.LambdaParser();
            //
            //var varContext = new Dictionary<string, object>();
            //varContext["pi"] = 3.14;
            ////varContext["form"] = dynamicForm.Form;
            //varContext["eq"] = new FormHelpers().equalsFormulate;
            //var equation = "eq(\"TypeDemande\", false)";
            ////Normalize JS to C#
            ////equation = equation.Replace("===", "==");
            //var ttt = lambdaParser.Eval(equation, varContext);

            if (defaultCfg != null && defaultCfg.Form != null)
            {
                var dynamicFormDict = dynamicForm.Form as IDictionary<object, object>;

                foreach (KeyValuePair<object, object> item in defaultCfg?.Form ?? throw new Exception("No form element in YAML default."))
                {
                    if (dynamicFormDict != null && !dynamicFormDict.TryAdd(item.Key, item.Value))
                    {
                        var defaultVal = item.Value as IDictionary<object, object>;
                        var newDict = new Dictionary<object, object>();
                        newDict.TryAdd(defaultVal);
                        (dynamicFormDict[item.Key] as IDictionary<object, object>).TryAdd(newDict);
                    }
                }
            }

            Title = (dynamicForm.Form["title"] as Dictionary<object, object>)?.GetLocalizedObject();

            if (!ShowAll)
            {
                dynamicForm.Form["enableVif"] = true;
            }

            using (StreamReader streamReader = new StreamReader(@"schemas/formTemplate.vue", Encoding.UTF8))
            {
                var content = await streamReader.ReadToEndAsync();
                FormRaw = await FormHelpers.Stubble.RenderAsync(content, dynamicForm);
            }

            //if (dynamicForm?.Form?["sections"] is null) { return NotFound(); }

            var groupNo = 0;
            var sectionNo = 0;
            foreach (var sectionGroup in dynamicForm.Form["sectionsGroup"])
            {
                var pageGroupDict = (sectionGroup as Dictionary<object, object>);
                List<Section> pages = new List<Section>();

                if (pageGroupDict is null) { continue; }

                if (pageGroupDict.TryGetValue("sections", out var sections))
                {
                    var sectionsAsList = (sections as List<object>);

                    if (sectionsAsList is null) { continue; }

                    foreach (Dictionary<object, object>? section in sectionsAsList)
                    {
                        if (section is null) { continue; }

                        pages.Add(new Section()
                        {
                            No = sectionNo++,
                            Id = (pageGroupDict.TryGetValue("prefixId", out var prefixId) ? prefixId?.ToString() ?? string.Empty : string.Empty) + (section.TryGetValue("id", out var pageId) ? pageId?.ToString() ?? string.Empty : string.Empty),
                            Titre = section.TryGetValue("section", out var pageName) ? (pageName as Dictionary<object, object>).GetLocalizedObject() ?? "Title not found" : "Title not found",
                            VIf = (section.TryGetValue("v-if", out object? pageVif) ? pageVif?.ToString() ?? string.Empty : string.Empty),
                            Classes = (section.TryGetValue("classes", out object? pageClasses) ? pageClasses?.ToString() : null)
                        });
                    }

                }

                Sections?.Add(new Section()
                {
                    No = groupNo++,
                    Id = pageGroupDict.TryGetValue("prefixId", out var sectionId) ? sectionId?.ToString() ?? string.Empty : string.Empty,
                    Titre = pageGroupDict.TryGetValue("sectionGroup", out var sectionName) ? (sectionName as Dictionary<object, object>).GetLocalizedObject() : null,
                    VIf = (pageGroupDict.TryGetValue("v-if", out object? vif) ? vif?.ToString() ?? string.Empty : string.Empty),
                    Classes = (pageGroupDict.TryGetValue("classes", out object? classes) ? classes?.ToString() : null),
                    Pages = pages
                });
            }

            // Restore last state
            HttpContext.Request.Cookies.TryGetValue($"FRW{configName}", out var form);

            if (string.IsNullOrEmpty(form))
                Form = new { validAll = false };
            else
                Form = JsonConvert.DeserializeObject<dynamic>(form);

            // Parse Vue data
            VueData = _vueParser.ParseData(this);

            return Page();
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

            validationError = default!;
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
