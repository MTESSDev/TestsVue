using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ECSForm.Pages
{
    public class FormData
    {
        public FormData()
        {
            Inputs = new List<Inputs>();
        }

        public List<Inputs> Inputs { get; set; }
    }

    public class Inputs
    {
        public Inputs()
        {
            Attributes = new List<Attribute>();
            Validations = new List<ValidationAttribute>();
        }

        public void SetType(string? type)
        {
            switch (type?.ToUpper())
            {
                case "HIDDEN":
                case "TEXT":
                case "TEXTAREA":
                    Type = TypeInput.TEXT;
                    break;
                case "DATE":
                    Type = TypeInput.DATE;
                    break;
                case "SELECT":
                    Type = TypeInput.SELECT;
                    break;
                case "CHECKBOX":
                    Type = TypeInput.CHECKBOX;
                    break;
                case "RADIO":
                    Type = TypeInput.RADIO;
                    break;
                default:
                    Type = TypeInput.SKIP;
                    break;
            }
        }

        public string? Name { get; set; }
        public string? PrefixId { get; set; }
        public string? GroupName { get; set; }
        public bool? IsRepeatable { get; set; }
        public TypeInput Type { get; set; }
        public IDictionary<object, object>? AcceptedValues { get; set; }

        public List<ValidationAttribute> Validations { get; set; }

        public void ParseAttributes(IDictionary<object, object>? attr, bool parseValidations = true)
        {
            if (attr is null) return;
            Validations.Add(new RequiredAttribute() { });

            foreach (var item in attr)
            {
                switch (item.Key.ToString()?.ToUpper())
                {
                    case "NAME":
                        Name = item.Value.ToString();
                        break;
                    case "TYPE":
                        SetType(item.Value.ToString());
                        break;
                    case "VALIDATIONS":
                        if (!parseValidations) { break; };

                        var validationsDict = item.Value as IDictionary<object, object>;
                        if (validationsDict is { } && validationsDict.ContainsKey("optional"))
                        {
                            //Enlever le required par défaut
                            Validations.Clear();
                        }
                        var rules = ParseValidations(validationsDict);
                        Validations.AddRange(ConvertRules(rules));
                        break;
                    case "OPTIONS":
                        if (item.Value.ToString().Equals("yesno", StringComparison.InvariantCultureIgnoreCase))
                        {
                            AcceptedValues = new Dictionary<object, object>() { { "true", true }, { "false", false }, };
                        }
                        else
                        {
                            if (item.Value.GetType().Name.StartsWith("List"))
                            {
                                AcceptedValues = (item.Value as List<object>).ToDictionary(x => x, x => x);
                            }
                            else
                            {
                                AcceptedValues = item.Value as IDictionary<object, object>;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private IEnumerable<ValidationAttribute> ConvertRules(List<Rule> rules)
        {
            foreach (var item in rules)
            {
                switch (item.Name?.ToLower())
                {
                    case "required":
                        yield return new RequiredAttribute() { };
                        break;
                    case "max":
                        yield return new MaxLengthAttribute(int.Parse(item.Param ?? "0"));
                        break;
                    case "nas":
                        // Non géré côté serveur pour le moment
                        break;
                    case "optional":
                        break;
                    default:
                        throw new InvalidOperationException(item.Name + " validation type unknown");
                        //break;
                }
            }
        }

        private List<Rule> ParseValidations(IDictionary<object, object>? validationsDict)
        {
            List<Rule> rules = new List<Rule>();
            if (validationsDict is null) { return rules; }
            foreach (var item in validationsDict)
            {
                var newRule = new Rule();

                newRule.Name = item.Key?.ToString()?.Trim('^');

                if (item.Value != null && newRule.Name is { })
                {
                    newRule.Param = item.Value?.ToString();
                    var valueOptions = newRule.Param?.Split(',') ?? new string[0];

                    if (valueOptions.Length > 1)
                    {
                        newRule.Param = valueOptions[0];
                        newRule.Option = valueOptions[1];
                    }
                }
                rules.Add(newRule);
            }
            return rules;
        }

        public List<Attribute> Attributes { get; private set; }
    }

    public class ComponentBinding
    {
        public ComponentBinding()
        {
        }

        public string? Name { get; set; }
        public List<string>? NameValues { get; set; }
        public string? PrefixId { get; set; }
        public string? GroupName { get; set; }
        public string? SectionName { get; set; }
        public int? MaxOccur { get; set; }
        public bool? IsRepeatable { get; set; }
        public TypeInput Type { get; set; }
        public IDictionary<object, object>? AcceptedValues { get; set; }

        public void SetType(string? type)
        {
            switch (type?.ToUpper())
            {
                case "HIDDEN":
                case "TEXT":
                case "TEXTAREA":
                    Type = TypeInput.TEXT;
                    break;
                case "DATE":
                    Type = TypeInput.DATE;
                    break;
                case "SELECT":
                    Type = TypeInput.SELECT;
                    break;
                case "CHECKBOX":
                    Type = TypeInput.CHECKBOX;
                    break;
                case "RADIO":
                    Type = TypeInput.RADIO;
                    break;
                default:
                    Type = TypeInput.SKIP;
                    break;
            }
        }

        public void ParseAttributes(IDictionary<object, object>? attr, bool parseValidations = true)
        {
            if (attr is null) return;

            foreach (var item in attr)
            {
                switch (item.Key.ToString()?.ToUpper())
                {
                    case "NAME":
                        Name = item.Value.ToString();
                        break;
                    case "LIMIT":
                        MaxOccur = int.Parse(item.Value.ToString());
                        break;
                    case "TYPE":
                        SetType(item.Value.ToString());
                        break;
                    case "OPTIONS":
                        if (item.Value.ToString().Equals("yesno", StringComparison.InvariantCultureIgnoreCase))
                        {
                            AcceptedValues = new Dictionary<object, object>() { { "true", true }, { "false", false }, };
                        }
                        else
                        {
                            if (item.Value.GetType().Name.StartsWith("List"))
                            {
                                AcceptedValues = (item.Value as List<object>).ToDictionary(x => x, x => x);
                            }
                            else
                            {
                                AcceptedValues = item.Value as IDictionary<object, object>;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }


    public class Rule
    {
        public string? Name { get; set; }
        public string? Param { get; set; }
        public string? Option { get; set; }
    }
}


public enum TypeInput
{
    SKIP,
    SELECT,
    CHECKBOX,
    RADIO,
    TEXT,
    DATE
}
