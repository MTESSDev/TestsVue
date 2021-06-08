﻿using AngleSharp.Html.Parser;
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
            string jsonDataFromUser;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonDataFromUser = await reader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<IDictionary<object, object>>(
                                    jsonDataFromUser,
                                        new JsonConverter[] {
                                                new MyConverter() }
                                    );

            if (data is null) { throw new Exception("No data received."); }

            var dynamicForm = GenericModel.ReadYamlCfg(@$"schemas/{id}.ecsform.yml");

            var context = new ValidationContext(data, serviceProvider: null, items: null);

            FormData formData = new FormData();

            GetEffectiveComponents(data, dynamicForm.Form?["sectionsGroup"], ref formData, null, null);

            foreach (var item in formData.Inputs)
            {
                if (item is { } && item.Validations != null)
                {
                    foreach (var validation in item.Validations.ToList())
                    {
                        if (!string.IsNullOrEmpty(item.Name))
                        {
                            data.TryGetValue(item.PrefixId + item.Name, out var val);

                            if (!validation.IsValid(val))
                            {
                                return BadRequest($"{validation.FormatErrorMessage(item.Name)}");
                            }
                        }
                    }
                }
            }
            return new OkResult();
        }

        public enum ComponentType
        {
            None = 0,
            group = 1
        }

        public static IDictionary<object, object>? GetEffectiveComponents(IDictionary<object, object> data, object components, ref FormData formData, string? prefixId, string? groupName)
        {
            //var obj = components as IDictionary<object, object>;
            var obj2 = components as IList<object>;

            /*if (obj != null && obj.ContainsKey("sections"))
             {
                 return GetEffectiveComponents(data, obj["sections"], ref formData, null);
             }
             else */
            if (obj2 != null)
            {
                foreach (var item in obj2)
                {
                    var dictItem = item as IDictionary<object, object>;

                    if (dictItem != null)
                    {
                        if (dictItem.TryGetValue("sections", out var sections))
                        {
                            dictItem.TryGetValue("prefixId", out var currentPrefixId);
                            return GetEffectiveComponents(data, sections, ref formData, currentPrefixId?.ToString(), null);
                        }

                        if (dictItem.TryGetValue("v-if", out var vif))
                        {
                            //Run v-if
                            try
                            {
                                var result = new Engine()
                                    .SetValue("index", 0)
                                    .SetValue("name", groupName)
                                    .SetValue("form", data)
                                    //.Execute($"form.EvenementsDerniereAnnee")
                                    .Execute($"({vif} ? true : false)")
                                    .GetCompletionValue()
                                    .AsBoolean();
                                if (!result)
                                {
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }

                        }

                        if (dictItem.TryGetValue("components", out var innerComponents))
                        {
                            if (dictItem.TryGetValue("type", out var componentType))
                            {
                                if (componentType != null && componentType.ToString()!.EndsWith("group", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    groupName = dictItem["name"].ToString();
                                }
                            }

                            var returnComp = GetEffectiveComponents(data, innerComponents, ref formData, prefixId, groupName);
                            if (returnComp != null)
                            {
                                return returnComp;
                            }
                        }
                    }

                    var inputV = new Inputs();
                    inputV.ParseAttributes(dictItem);
                    inputV.GroupName = groupName;
                    inputV.PrefixId = prefixId;
                    if (inputV.Type != TypeInput.SKIP)
                    {
                        formData.Inputs.Add(inputV);
                    }
                }
            }

            return null;
        }
    }
}
