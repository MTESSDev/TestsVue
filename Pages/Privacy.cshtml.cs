using AngleSharp.Html.Parser;
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

namespace WebApplication2.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> OnPost()
        {
            string tt;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                // tt = await System.Text.Json.JsonSerializer.DeserializeAsync<Dictionary<string,object>>(reader.BaseStream);
                tt = await reader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<JObject>(tt);

            var parser = new HtmlParser(new HtmlParserOptions
            {
                IsEmbedded = true,

            });

            var formData = new FormData();

            using (var r = new StreamReader(@"form.vue"))
            {
                var vue = await r.ReadToEndAsync();
                //Just get the DOM representation
                //var document = await context.OpenAsync(async req => req.Content(await r.ReadToEndAsync()));
                var document = await parser.ParseDocumentAsync(vue);

                foreach (var item in document.Body.GetElementsByTagName("*"))
                {
                    if (item.TagName.StartsWith("formulate", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (item.TagName.EndsWith("FORM"))
                        {
                            foreach (var attr in item.Attributes)
                            {
                                formData.Form.Attributes.Add(new Attribute() { Name = attr.LocalName, Value = attr.Value });
                            }
                        }
                        else
                        {
                            var input = new Input();

                            foreach (var attr in item.Attributes)
                            {
                                input.ParseAttribute(new Attribute() { Name = attr.LocalName, Value = attr.Value });
                            }
                            formData.Inputs.Add(input);
                        }

                        /*var val = item.Attributes.Where(e => e.LocalName == "validation").FirstOrDefault();
                        var valmsg = item.Attributes.Where(e => e.LocalName == ":validation-messages").FirstOrDefault();
                        var name = item.Attributes.Where(e => e.LocalName == "name").FirstOrDefault();

                        if (val != null)
                        {
                            Dictionary<string, string> msg = null;
                            if (valmsg != null)
                            {
                                msg = JsonConvert.DeserializeObject<Dictionary<string, string>>(valmsg.Value);
                            }
                            ParseValidation(ref validations, name.Value, val.Value, msg);
                        }*/
                        //validations.Elements.Add(item.Attributes["name"], new Validation() {  TypeValidation = new TypeValidation()  })
                    }
                }
            }


            var context = new ValidationContext(data, serviceProvider: null, items: null);


            foreach (var item in formData.Inputs)
            {
                if (item.Validations != null)
                {
                    foreach (var validation in item.Validations.ToList())
                    {
                        data.TryGetValue(item.Name, out var val).ToString();

                        if (!item.Vif && !validation.IsValid(val))
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return new OkResult();
        }
    }
}
