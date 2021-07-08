using FRW.PR.Extra.Utils;
using FRW.PR.Model.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FRW.PR.Extra.Pages
{
    public class BindingIndexModel : PageModel
    {
        private readonly IVueParser _vueParser;
        private readonly ILogger<BindingIndexModel>? _logger;

        public Dictionary<string, object?> VueData { get; set; } = new Dictionary<string, object?>();

        [VueData("templates")]
        public List<Dictionary<string, string>>? Templates { get; set; }

        [VueData("bind")]
        public Dictionary<string, Dictionary<string, BindElement>>? Bind { get; set; }

        [VueData("gabaritEnCours")]
        public string? GabaritEnCours { get; set; }

        [VueData("optionsGroups")]
        public List<string>? OptionsGroups { get; set; }

        [VueData("allOptionsFields")]
        public Dictionary<string, HashSet<string>>? AllOptionsFields { get; set; }

        public BindingIndexModel(ILogger<BindingIndexModel> logger, IVueParser vueParser)
        {
            _logger = logger;
            _vueParser = vueParser;

            //var val = ReadYamlCfg(@"C:\Users\Dany\Documents\Visual Studio 2012\Projects\TestsVue\mapping\3003\ecsbind.yml");
            //SaveYamlCfg(val, @"C:\Users\Dany\Documents\Visual Studio 2012\Projects\TestsVue\mapping\3003\ecsbind.yml");
        }

        public IActionResult OnGet(string id, string? gabarit = "1")
        {
            GabaritEnCours = gabarit;
            var mappgingObj = ReadYamlCfg(@"mapping/3003/ecsbind.yml");

            Templates = mappgingObj.Templates;
            Bind = mappgingObj.Bind;

            if(Bind is null)
            {
                Bind = new Dictionary<string,Dictionary<string, BindElement>>();
            }

            var form = GenericModel.ReadYamlCfg(@"schemas/3003.ecsform.yml");

            var formData = new List<ComponentBinding>();

            GetComponents(form.Form?["sectionsGroup"], ref formData, null, null, null, null);

            OptionsGroups = new List<string>();
            AllOptionsFields = new Dictionary<string, HashSet<string>>();

            foreach (var component in formData)
            {
                var list = new HashSet<string>();
                foreach (var item in component.NameValues)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }

                if (AllOptionsFields.ContainsKey(component.SectionName))
                {
                    foreach (var item in list)
                    {
                        if (!AllOptionsFields[component.SectionName].Contains(item))
                        {
                            AllOptionsFields[component.SectionName].Add(item);
                        }
                    }
                }
                else
                {
                    AllOptionsFields.Add(component.SectionName, list);
                }


            }

            AllOptionsFields.Add("Interne", new HashSet<string>() { "<NULL>" });

            foreach (var component in AllOptionsFields)
            {
                if (!OptionsGroups.Contains(component.Key))
                {
                    OptionsGroups.Add(component.Key);
                }
            }

            VueData = _vueParser.ParseData(this);

            return Page();
        }

        public static void GetComponents(object components, ref List<ComponentBinding> formData, string? sectionName, string? prefixId, string? groupName, bool? isRepeatable)
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

                            GetComponents(sections, ref formData, sectionName, currentPrefixId?.ToString(), null, null);
                        }

                        if (dictItem.TryGetValue("components", out var innerComponents))
                        {
                            dictItem.TryGetValue("id", out var section);
                            if (section != null)
                            {
                                sectionName = section.ToString();
                            }

                            if (dictItem.TryGetValue("type", out var componentType))
                            {
                                if (componentType != null && componentType.ToString()!.EndsWith("group", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    groupName = dictItem["name"].ToString();

                                    if (componentType.ToString()!.Contains("repeatable"))
                                    {
                                        isRepeatable = true;
                                    }
                                    else
                                    {
                                        isRepeatable = false;
                                    }
                                }
                            }

                            GetComponents(innerComponents, ref formData, sectionName, prefixId, groupName, isRepeatable);
                        }
                    }

                    var inputV = new ComponentBinding();
                    inputV.ParseAttributes(dictItem);
                    inputV.GroupName = groupName;
                    inputV.SectionName = sectionName;
                    inputV.PrefixId = prefixId;
                    inputV.IsRepeatable = isRepeatable;
                    inputV.NameValues = new List<string>();
                    if (inputV.Type != TypeInput.SKIP)
                    {
                        inputV.NameValues.AddRange(GenerateElementName(inputV, null));

                        if (inputV.AcceptedValues != null)
                        {
                            foreach (var val in inputV.AcceptedValues)
                            {
                                inputV.NameValues.AddRange(GenerateElementName(inputV, $"=={val.Key}"));
                            }
                        }

                        formData.Add(inputV);
                    }

                }
            }

        }

        private static IEnumerable<string> GenerateElementName(ComponentBinding input, string? value)
        {
            if (input.GroupName != null)
            {
                if ((bool)input.IsRepeatable!)
                {
                    for (int i = 0; i < (input.MaxOccur ?? 2); i++)
                    {
                        yield return $"{input.PrefixId}{input.GroupName}.{i}.{input.PrefixId}{input.Name}{value}";
                    }
                }
                else
                {
                    yield return $"{input.PrefixId}{input.GroupName}.0.{input.PrefixId}{input.Name}{value}";
                }
            }
            else
            {
                yield return $"{input.PrefixId}{input.Name}{value}";
            }
        }

        public static IDeserializer deserializer = new DeserializerBuilder()
                     .WithNamingConvention(CamelCaseNamingConvention.Instance)
                     .IgnoreUnmatchedProperties()
                     .Build();

        public static ISerializer serializer = new SerializerBuilder()
                     .WithNamingConvention(CamelCaseNamingConvention.Instance)
                     .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                     .Build();

        public static Binder ReadYamlCfg(string filename)
        {
            dynamic cfg;
            using (var configFile = new StreamReader(filename))
            {
                cfg = deserializer.Deserialize<Binder>(configFile);
            }
            return cfg;
        }

        public static void SaveYamlCfg(Binder value, string filename)
        {
            using (var configFile = new StreamWriter(filename))
            {
                serializer.Serialize(configFile, graph: value);
            }
        }

    }


    public class Binder
    {
        public List<Dictionary<string, string>> Templates { get; set; }
        public Dictionary<string, Dictionary<string, BindElement>> Bind { get; set; }
    }

    public class BindElement
    {
        public IEnumerable<string>? Champs { get; set; }
        public string? Formule { get; set; }
    }


}
