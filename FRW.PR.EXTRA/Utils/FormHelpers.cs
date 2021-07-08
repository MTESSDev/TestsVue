using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Stubble.Core;
using Stubble.Core.Builders;
using Stubble.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ECSForm.Utils
{
    public class FormHelpers
    {

        public static bool IsStubbleInitialized { get; set; }
        private static StubbleVisitorRenderer? _stubble;

        private static readonly Func<HelperContext, dynamic?, string> jsObject = FormHelpers.JsObject;
        private static readonly Func<HelperContext, IDictionary<object, object>?, string> generateValidations = FormHelpers.GenerateValidations;
        private static readonly Func<HelperContext, IDictionary<object, object>?, string> i18n = FormHelpers.I18n;
        private static readonly Func<HelperContext, IEnumerable<dynamic>, string> recursiveComponents = FormHelpers.RecursiveComponents;
        private static readonly Func<HelperContext, string, object, string> generateClasses = FormHelpers.GenerateClasses;
        private static readonly Func<HelperContext, string, object, string> generateDefaults = FormHelpers.GenerateDefaults;
        private static readonly Func<HelperContext, object, string> generateVif = FormHelpers.GenerateVif;
        public readonly Func<string, string, bool> equalsFormulate = FormHelpers.EqualsFormulate;
        //public readonly Func<string, bool> hasValueFormulate = FormHelpers.HasValueFormulate;

        public static bool EqualsFormulate(string componentName, string value)
        {
            /* WIP  if (form.GetComponent(componentName)["type"] == "")
             {
                 return "1";
             }*/
            return true;
        }
        public static bool HasValueFormulate(object form, IDictionary<object, object> data, string componentName)
        {
            // if (form.GetComponent(componentName)["value"] == "")
            //  {
            //      return "1";
            //  }
            return true;
        }

        private static string GenerateClasses(HelperContext context, string type, dynamic component)
        {
            var dictComponent = component as IDictionary<object, object>;

            if (dictComponent is null) return string.Empty;

            var input = InternalGenerateClassesFromObject(dictComponent, "inputClasses", type, ":input-class", component["Form"], "inputDefaultClasses");
            var outer = InternalGenerateClassesFromObject(dictComponent, "outerClasses", type, ":outer-class", component["Form"], "outerDefaultClasses");

            return $"{input} {outer}";
        }

        private static string GenerateDefaults(HelperContext context, string type, dynamic component)
        {
            var dictComponent = component as IDictionary<object, object>;

            if (dictComponent is null) return string.Empty;

            var defaultDict = context.Lookup<Dictionary<object, object>>($"Form.defaults.{type}");

            var html = string.Empty;

            if (defaultDict != null)
            {
                foreach (var defaultVal in defaultDict)
                {
                    dictComponent.TryGetValue(defaultVal.Key, out var overrideValue);

                    if (overrideValue is null)
                    {
                        var element = (defaultVal.Value as Dictionary<object, object>);
                        if (element is null)
                        {
                            html += $" {defaultVal.Key}=\"{defaultVal.Value}\" ";
                        }
                        else
                        {
                            var localPlaceholder = (defaultVal.Value as Dictionary<object, object>).GetLocalizedObject();
                            html += $" {defaultVal.Key}=\"{localPlaceholder}\" ";
                        }
                    }
                }
            }

            return html;
        }

        private static string GenerateVif(HelperContext context, object vif)
        {
            var prefixId = context.Lookup<object>("prefixId");
            var enableVif = context.Lookup<object>("Form.enableVif");

            if (bool.Parse(enableVif.ToString() ?? "true"))
            {
                return $" v-if=\"{InternalGenerateVif(vif?.ToString(), prefixId?.ToString())}\"";
            }

            return string.Empty;
        }

        private static string InternalGenerateClassesFromObject(IDictionary<object, object> dictComponent, string yamlKey,
                                                                string type, string attributeName, IDictionary<object, object>? form,
                                                                string defaultDictAttributeName)
        {
            if (form is null) { throw new ArgumentNullException(nameof(form) + " parameter is empty"); }

            form.TryGetValue(defaultDictAttributeName, out var defaultClassesObj);
            var defaultClasses = defaultClassesObj as IDictionary<object, object>;

            if (defaultClasses is null) defaultClasses = new Dictionary<object, object>();

            object? customClasses = null;

            dictComponent?.TryGetValue(yamlKey, out customClasses);

            var listInputCustom = customClasses?.ToString()?.Split(' ') ?? new string[] { };

            if (defaultClasses.TryGetValue(type, out var classesType))
            {
                return InternalGenerateClasses(attributeName, (classesType as string) ?? string.Empty, listInputCustom);
            }
            else if (defaultClasses.TryGetValue("default", out var classesDefault))
            {
                return InternalGenerateClasses(attributeName, (classesDefault as string) ?? string.Empty, listInputCustom);
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

        public static StubbleVisitorRenderer Stubble
        {
            get
            {
                if (!IsStubbleInitialized)
                {
                    var helpers = new Helpers()
                            .Register("GenerateClasses", generateClasses)
                            .Register("GenerateDefaults", generateDefaults)
                            .Register("GenerateVif", generateVif)
                            .Register("RecursiveComponents", recursiveComponents)
                            .Register("i18n", i18n)
                            .Register("JsObject", jsObject)
                            .Register("GenerateValidations", generateValidations);
                    _stubble = new StubbleBuilder()
                                .Configure(conf =>
                                {
                                    conf.AddHelpers(helpers);
                                    conf.SetIgnoreCaseOnKeyLookup(true);
                                })
                                .Build();
                }

                return _stubble ?? throw new Exception("_stubble not init.");
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
                            vueDict.Add(val.Key, FormHelpersExtensions.GetLocalizedObject(val.Value as Dictionary<object, object>));
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
            return FormHelpersExtensions.GetLocalizedObject(dict)?.ToString() ?? string.Empty;
        }

        public static string? InternalGenerateVif(string? vif, string? prefixId)
        {
            return vif?.Replace("prefixId$", prefixId ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string RecursiveComponents(HelperContext context, IEnumerable<dynamic> components)
        {
            string html = string.Empty;

            var form = new Dictionary<object, object>();
            form.TryAdd("Form", context.Lookup<object>("Form"));
            form.TryAdd("prefixId", context.Lookup<object>("prefixId"));

            foreach (var component in components)
            {
                if (context.Lookup<Dictionary<object, object>>("Form.templates").TryGetValue(component["type"], out object template))
                {
                    html += Stubble.Render(template.ToString(), FormHelpersExtensions.Combine(component, form));
                }
                else if (context.Lookup<Dictionary<object, object>>("Form.templates").TryGetValue("input", out object? inputTemplate))
                {
                    html += Stubble.Render(inputTemplate.ToString(), FormHelpersExtensions.Combine(component, form));
                }
            }

            return html;
        }

    }

    public static class FormHelpersExtensions
    {
        public static IDictionary<object, object>? GetComponent(this object form, string componentName)
        {
            var obj = form as IDictionary<object, object>;
            var obj2 = form as IList<object>;
            if (obj != null && obj.ContainsKey("sections"))
            {
                return obj["sections"].GetComponent(componentName);
            }
            else if (obj2 != null)
            {
                foreach (var item in obj2)
                {
                    var dictItem = item as IDictionary<object, object>;
                    if (dictItem is { } && dictItem.TryGetValue("components", out var components))
                    {
                        var returnComp = components.GetComponent(componentName);
                        if (returnComp != null)
                        {
                            return returnComp;
                        }
                    }

                    if (dictItem != null && dictItem.TryGetValue("name", out var name))
                    {
                        if (name.Equals(componentName))
                        {
                            return dictItem;
                        }
                    }

                }
            }

            return null;
        }

        public static dynamic Combine(dynamic item1, IDictionary<object, object> item2)
        {
            var dictionary1 = (IDictionary<object, object>)item1;
            var result = new Dictionary<object, object>();
            var d = result as IDictionary<object, object>;

            foreach (var pair in item2)
            {
                d.Add(pair.Key, pair.Value);
            }
            foreach (var pair in dictionary1)
            {
                d.Add(pair.Key, pair.Value);
            }

            return result;
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
                var first = dict.FirstOrDefault();
                return $"[{first.Key},{first.Value}]";
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

    class MyConverter : CustomCreationConverter<IDictionary<object, object>>
    {
        public override IDictionary<object, object> Create(Type objectType)
        {
            return new Dictionary<object, object>();
        }

        public override bool CanConvert(Type objectType)
        {
            // in addition to handling IDictionary<string, object>
            // we want to handle the deserialization of dict value
            // which is of type object
            return objectType == typeof(object) || base.CanConvert(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject
                || reader.TokenType == JsonToken.Null)
                return base.ReadJson(reader, objectType, existingValue, serializer);

            // if the next token is not an object
            // then fall back on standard deserializer (strings, numbers etc.)

            /*if (reader.TokenType == JsonToken.String && reader.Value.Equals("true"))
            {
                return serializer.Deserialize<bool>(reader);
            }*/

            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize<object[]>(reader);
            }

            return serializer.Deserialize(reader);
        }
    }
}