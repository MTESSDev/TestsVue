using Stubble.Core;
using Stubble.Core.Builders;
using Stubble.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WebApplication2.Pages
{
    internal static class FormHelpers
    {
        public static bool isStubbleInitialized { get; set; }
        private static StubbleVisitorRenderer _stubble { get; set; }

        private static Func<HelperContext, dynamic?, string> jsObject = FormHelpers.JsObject;
        private static Func<HelperContext, IDictionary<object, object>?, string> jsArray = FormHelpers.JsArray;
        private static Func<HelperContext, IDictionary<object, object>?, string> i18n = FormHelpers.I18n;
        private static Func<HelperContext, IEnumerable<dynamic>, string> recursiveComponents = FormHelpers.RecursiveComponents;
        private static Func<HelperContext, string, object, string> generateInputClasses = FormHelpers.GenerateInputClasses;

        private static string GenerateInputClasses(HelperContext context, string type, dynamic component)
        {
            if (InputDefaultClasses is null) return string.Empty;

            var dictComponent = component as Dictionary<object, object>;
            object? customClasses = null;
            dictComponent?.TryGetValue("classes", out customClasses);
            var listInputCustom = customClasses?.ToString()?.Split(' ') ?? new string[] { };

            if (InputDefaultClasses.TryGetValue(type, out var classesType))
            {
                return InternalGenerateInputClasses(classesType, listInputCustom);
            }
            else if (InputDefaultClasses.TryGetValue("default", out var classesDefault))
            {
                return InternalGenerateInputClasses(classesDefault, listInputCustom);
            }

            return string.Empty;
        }


        private static string InternalGenerateInputClasses(string classes, string[] listInputCustom)
        {
            if (classes is null) return string.Empty;
            var classesList = classes.Split(' ').ToList();
            classesList.AddRange(listInputCustom);
            return $":input-class=\"['{string.Join("', '", classesList.Select(k => k.ToString().Sanitize()))}']\"";
        }

        public static IDictionary<string, string>? TemplateList { get; set; }
        public static IDictionary<string, string>? InputDefaultClasses { get; set; }

        public static StubbleVisitorRenderer Stubble
        {
            get
            {
                if (!isStubbleInitialized)
                {
                    var helpers = new Helpers()
                            .Register("GenerateInputClasses", generateInputClasses)
                            .Register("RecursiveComponents", recursiveComponents)
                            .Register("i18n", i18n)
                            .Register("JsObject", jsObject)
                            .Register("JsArray", jsArray);
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
                    return JsObject(context, GetLocalizedObject(checkVal));
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

        public static string JsArray(HelperContext context, IDictionary<object, object>? dict)
        {
            if (dict is null) return string.Empty;

            var element = "[";

            foreach (var item in dict)
            {
                if (item.Value is null) continue;
                var val = item.Value.ToString();
                if (string.IsNullOrEmpty(val)) continue;

                element += $"['{item.Key}', '{String.Join("','", val.Split(','))}'], ";
            }
            return element + "]";
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

        private static dynamic? GetLocalizedObject(IDictionary<object, object>? dict)
        {
            if (dict is null) return null;
            dict.TryGetValue(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, out var value);
            if (value is null)
            {
                return dict.FirstOrDefault();
            }

            return value;
        }
    }

}