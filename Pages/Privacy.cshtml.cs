using AngleSharp.Html.Parser;
using ECSForm.Model;
using ECSForm.Utils;
using Jint;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSForm.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PrivacyModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> OnPost(string id)
        {
            string tt;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                // tt = await System.Text.Json.JsonSerializer.DeserializeAsync<Dictionary<string,object>>(reader.BaseStream);
                tt = await reader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<IDictionary<object, object>>(
            tt, new JsonConverter[] {
                new MyConverter() }
            );

            var dynamicForm = GenericModel.ReadYamlCfg(@$"schemas/{id}.ecsform.yml");


            var context = new ValidationContext(data, serviceProvider: null, items: null);

            FormData formData = new FormData();

            GetEffectiveComponents(data, dynamicForm.Form, ref formData);


            /* foreach (var item in dynamicForm.Form as IDictionary<object, object>)
             {

                 var ttt = 5;*/
            /*if (item.Validations != null)
            {
                foreach (var validation in item.Validations.ToList())
                {
                    data.TryGetValue(item.Name, out var val).ToString();

                    if (!item.Vif && !validation.IsValid(val))
                    {
                        return NotFound();
                    }
                }
            }*/
            //  }
            return new OkResult();
        }

        public static IDictionary<object, object>? GetEffectiveComponents(IDictionary<object, object> data, object components, ref FormData formData)
        {
            var obj = components as IDictionary<object, object>;
            var obj2 = components as IList<object>;



            if (obj != null && obj.ContainsKey("sections"))
            {
                return GetEffectiveComponents(data, obj["sections"], ref formData);
            }
            else if (obj2 != null)
            {
                foreach (var item in obj2)
                {


                    var dictItem = item as IDictionary<object, object>;

                    if (dictItem != null && dictItem.TryGetValue("v-if", out var vif))
                    {
                        //Run v-if
                        //var lambdaParser = new NReco.Linq.LambdaParser();
                        //
                        //var varContext = new Dictionary<string, object>();
                        //
                        //varContext["form"] = data;
                        ////varContext["form"] = dynamicForm.Form;
                        //varContext["eq"] = new FormHelpers().equalsFormulate;
                        //varContext["hasValue"] = new FormHelpers().hasValueFormulate;
                        //var equation = vif.ToString().Replace('\'','"');
                        ////Normalize JS to C#
                        ////equation = equation.Replace("===", "==");
                        //var ttt = lambdaParser.Eval(equation, varContext);

                        var result = new Engine()
                                        .SetValue("form", data) // define a new variable
                                                                //.SetValue("comp", new Action<string, object,>(formref.GetComponent)) // define a new variable
                                        .Execute($"({vif} ? true : false)") // execute a statement
                                        .GetCompletionValue() // get the latest statement completion value
                                        .AsBoolean();
                        if (!result) { continue; }
                    }

                    if (dictItem.TryGetValue("components", out var innerComponents))
                    {
                        var returnComp = GetEffectiveComponents(data, innerComponents, ref formData);
                        if (returnComp != null)
                        {
                            return returnComp;
                        }
                    }

                    var inputV = new InputV();
                    inputV.ParseAttributes(dictItem);
                    /* if (dictItem != null && dictItem.TryGetValue("v-if", out var vIf))
                     {
                         inputV.Vif = vIf;
                     }

                     if (dictItem != null && dictItem.TryGetValue("validation", out var validations))
                     {

                     }*/
                    formData.Inputs.Add(inputV);
                }
            }

            return null;
        }
    }
}
