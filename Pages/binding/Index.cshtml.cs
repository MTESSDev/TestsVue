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
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ECSForm.Pages
{
    public class BindingIndexModel : PageModel
    {
        private readonly ILogger<IndexModel>? _logger;

        public BindingIndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            //var val = ReadYamlCfg(@"C:\Users\Dany\Documents\Visual Studio 2012\Projects\TestsVue\mapping\3003\ecsbind.yml");
            //SaveYamlCfg(val, @"C:\Users\Dany\Documents\Visual Studio 2012\Projects\TestsVue\mapping\3003\ecsbind.yml");

        }

        public IActionResult OnGet()
        {
            var form = GenericModel.ReadYamlCfg(@"C:\Users\Dany\Documents\Visual Studio 2012\Projects\TestsVue\schemas\3003CC.ecsform.yml");

            var formData = new FormData();

            GetComponents(form.Form?["sectionsGroup"], ref formData, null, null);

            
            return Page();
        }

        public static void GetComponents(object components, ref FormData formData, string? prefixId, string? groupName)
        {
            var obj2 = components as IList<object>;

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
                            GetComponents(sections, ref formData, currentPrefixId?.ToString(), null);
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

                            GetComponents(innerComponents, ref formData, prefixId, groupName);
                        }
                    }

                    var inputV = new Inputs();
                    inputV.ParseAttributes(dictItem, false);
                    inputV.GroupName = groupName;
                    inputV.PrefixId = prefixId;
                    if (inputV.Type != TypeInput.SKIP)
                    {
                        formData.Inputs.Add(inputV);
                    }
                }
            }

        }

        public static IDeserializer deserializer = new DeserializerBuilder()
                     .WithNamingConvention(CamelCaseNamingConvention.Instance)
                     .IgnoreUnmatchedProperties()
                     .Build();

        public static ISerializer serializer = new SerializerBuilder()
                     .WithNamingConvention(CamelCaseNamingConvention.Instance)
                     .Build();

        public static object ReadYamlCfg(string filename)
        {
            dynamic cfg;
            using (var configFile = new StreamReader(filename))
            {
                cfg = deserializer.Deserialize<dynamic>(configFile);
            }
            return cfg;
        }

        public static void SaveYamlCfg(object value, string filename)
        {
            using (var configFile = new StreamWriter(filename))
            {
                serializer.Serialize(configFile, graph: value);
            }
        }

    }
}
