using Stubble.Core;
using Stubble.Core.Builders;
using Stubble.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ECSForm.Utils
{
    internal static class FormHelpers
    {
        public static bool isStubbleInitialized { get; set; }
        private static StubbleVisitorRenderer _stubble { get; set; }

        private static Func<HelperContext, dynamic?, string> jsObject = FormHelpers.JsObject;
        private static Func<HelperContext, IDictionary<object, object>?, string> generateValidations = FormHelpers.GenerateValidations;
        private static Func<HelperContext, IDictionary<object, object>?, string> i18n = FormHelpers.I18n;
        private static Func<HelperContext, IEnumerable<dynamic>, string> recursiveComponents = FormHelpers.RecursiveComponents;
        private static Func<HelperContext, string, object, string> generateClasses = FormHelpers.GenerateClasses;

        private static string GenerateClasses(HelperContext context, string type, dynamic component)
        {
            var dictComponent = component as Dictionary<object, object>;

            if (dictComponent is null) return string.Empty;

            var input = InternalGenerateClassesFromObject(dictComponent, "inputClasses", type, ":input-class", InputDefaultClasses);
            var outer = InternalGenerateClassesFromObject(dictComponent, "outerClasses", type, ":outer-class", OuterDefaultClasses);

            return $"{input} {outer}";
        }

        private static string InternalGenerateClassesFromObject(Dictionary<object, object> dictComponent, string yamlKey, string type, string attributeName, IDictionary<string, string>? defaultClasses)
        {

            if (defaultClasses is null) return string.Empty;

            object? customClasses = null;

            dictComponent?.TryGetValue(yamlKey, out customClasses);

            var listInputCustom = customClasses?.ToString()?.Split(' ') ?? new string[] { };

            if (defaultClasses.TryGetValue(type, out var classesType))
            {
                return InternalGenerateClasses(attributeName, classesType, listInputCustom);
            }
            else if (defaultClasses.TryGetValue("default", out var classesDefault))
            {
                return InternalGenerateClasses(attributeName, classesDefault, listInputCustom);
            }
            else
            {
                return InternalGenerateClasses(attributeName, "", listInputCustom);
            }
        }


        private static string InternalGenerateClasses(string attributeName, string classes, string[] listInputCustom)
        {
            var classesList = (classes ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            classesList.AddRange(listInputCustom);

            if (!classesList.Any())
            {
                return string.Empty;
            }

            return $"{attributeName}=\"['{string.Join("', '", classesList.Select(k => k.ToString().Sanitize()))}']\"";
        }

        public static IDictionary<string, string>? TemplateList { get; set; }
        public static IDictionary<string, string>? InputDefaultClasses { get; set; }
        public static IDictionary<string, string>? OuterDefaultClasses { get; set; }

        public static StubbleVisitorRenderer Stubble
        {
            get
            {
                if (!isStubbleInitialized)
                {
                    var helpers = new Helpers()
                            .Register("GenerateClasses", generateClasses)
                            .Register("RecursiveComponents", recursiveComponents)
                            .Register("i18n", i18n)
                            .Register("JsObject", jsObject)
                            .Register("GenerateValidations", generateValidations);
                    _stubble = new StubbleBuilder()
                                .Configure(conf =>
                                {
                                    //conf.AddToTemplateLoader(new DictionaryLoader(partials));
                                    conf.AddHelpers(helpers);
                                    conf.SetIgnoreCaseOnKeyLookup(true);
                                })
                                .Build();
                }
                return _stubble;
            }
        }


        public static string JsObject(HelperContext context, dynamic? dict)
        {
            if (dict is null) return string.Empty;

            Dictionary<object, object> vueDict = new Dictionary<object, object>();
            if (dict is string)
            {
                if (dict == "yesno")
                {
                    Dictionary<object, object> tempDict = new Dictionary<object, object>();
                    tempDict.Add("fr", "Oui");
                    tempDict.Add("en", "Yes");
                    vueDict.Add("true", I18n(context, tempDict));

                    tempDict.Clear();
                    tempDict.Add("fr", "Non");
                    tempDict.Add("en", "No");
                    vueDict.Add("false", I18n(context, tempDict));
                }
            }
            else if (dict is List<object>)
            {
                return $"['{string.Join("', '", (dict as List<object>).Select(k => k.ToString().Sanitize()))}']";
            }
            else
            {
                var checkVal = dict as Dictionary<object, object>;

                if (checkVal is null) return string.Empty;

                if (checkVal.ContainsKey("fr"))
                {
                    return JsObject(context, checkVal.GetLocalizedObject());
                }
                else
                {
                    if (checkVal.FirstOrDefault().Value is Dictionary<object, object>)
                    {
                        foreach (var val in checkVal)
                        {
                            vueDict.Add(val.Key, GetLocalizedObject(val.Value as Dictionary<object, object>));
                        }
                    }
                }

                if (!vueDict.Any())
                {
                    vueDict = dict;
                }
            }

            return "{" + string.Join(", ", vueDict.Select(kv => kv.Key + $": '{kv.Value?.ToString().Sanitize()}'").ToArray()) + "}";
        }

        public static string GenerateValidations(HelperContext context, IDictionary<object, object>? dict)
        {
            if (dict is null) return string.Empty;

            Dictionary<object, object>? dictValidations = null;

            if (dict.TryGetValue("validations", out var validations))
            {
                dictValidations = validations as Dictionary<object, object>;
            }

            if (dictValidations is null)
            {
                dictValidations = new Dictionary<object, object>();
            }

            if (!dictValidations.ContainsKey("optional"))
            {
                dictValidations.TryAdd("required", "trim");
            }

            var element = "[";

            foreach (var item in dictValidations)
            {
                if (item.Value is null) continue;
                var val = item.Value.ToString();
                if (string.IsNullOrEmpty(val)) continue;

                element += $"['{item.Key}', '{String.Join("','", val.Split(','))}'], ";
            }
            return $":validation=\"{element}]\"";
        }

        public static string I18n(HelperContext context, IDictionary<object, object>? dict)
        {
            return GetLocalizedObject(dict)?.ToString() ?? string.Empty;
        }

        public static string RecursiveComponents(HelperContext context, IEnumerable<dynamic> components)
        {
            string html = string.Empty;
            foreach (var component in components)
            {
                if (TemplateList.TryGetValue(component["type"], out string template))
                {
                    html += Stubble.Render(template, component);
                }
                else if (TemplateList.TryGetValue("input", out var inputTemplate))
                {
                    html += Stubble.Render(inputTemplate, component);
                }
            }

            return html;
        }

        public static string Sanitize(this string? value)
        {
            if (value is null) return string.Empty;

            return value.Replace("'", "\\'");
        }

        public static dynamic? GetLocalizedObject(this IDictionary<object, object>? dict)
        {
            if (dict is null) return null;
            dict.TryGetValue(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, out var value);
            if (value is null)
            {
                return dict.FirstOrDefault();
            }

            return value;
        }

        public static void TryAdd<TKey, TValue>(this IDictionary<TKey, TValue>? dictionary, IDictionary<TKey, TValue>? source) where TKey : notnull
        {
            if (dictionary is null) throw new Exception("Target dict need to be init.");

            if (source is null) return;

            foreach (var item in source)
            {
                dictionary?.TryAdd(item.Key, item.Value);
            }
        }
    }

}