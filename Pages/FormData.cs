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
            Form = new InputV();
            Inputs = new List<InputV>();
        }

        public InputV Form { get; set; }
        public List<InputV> Inputs { get; set; }
    }

    public class InputV
    {
        public InputV()
        {
            Attributes = new List<Attribute>();
            Validations = new List<ValidationAttribute>();
            //AcceptedValues = new List<string>();
        }

        public void SetType(string type)
        {
            switch (type.ToUpper())
            {
                case "TEXT":
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

        public string Name { get; set; }
        public string? GroupName { get; set; }
        public TypeInput Type { get; set; }
        public IDictionary<object, object> AcceptedValues { get; set; }

        public List<ValidationAttribute> Validations { get; set; }

        public void ParseAttributes(IDictionary<object, object> attr)
        {
            Validations.Add(new RequiredAttribute() { });

            foreach (var item in attr)
            {
                switch (item.Key.ToString().ToUpper())
                {
                    case "NAME":
                        Name = item.Value.ToString();
                        break;
                    case "TYPE":
                        SetType(item.Value.ToString());
                        break;
                    case "VALIDATIONS":
                        var validationsDict = item.Value as IDictionary<object, object>;
                        if (validationsDict.ContainsKey("optional"))
                        {
                            //Enlever le required par défaut
                            Validations.Clear();
                        }
                        var rules = ParseValidations(validationsDict);
                        Validations.AddRange(ConvertRules(rules));
                        break;
                    case "OPTIONS":
                        AcceptedValues = item.Value as IDictionary<object, object>;
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
                switch (item.Name.ToLower())
                {
                    case "required":
                        yield return new RequiredAttribute() { };
                        break;
                    case "max":
                        yield return new MaxLengthAttribute(int.Parse(item.Param));
                        break;
                    case "optional":
                        break;
                    default:
                        throw new InvalidOperationException(item.Name + " unknown");
                        break;
                }
            }
        }

        private List<Rule> ParseValidations(IDictionary<object, object> valdationsDict)
        {
            List<Rule> rules = new List<Rule>();

            //var list = value.Split('|');
            foreach (var item in valdationsDict)
            {
                var newRule = new Rule();

                //var keyPair = item.Split(':');
                newRule.Name = item.Key.ToString().Trim('^');
                if (item.Value != null)
                {
                    newRule.Param = item.Value.ToString();
                    var valueOptions = newRule.Param.Split(',');

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
        //public bool Vif { get; private set; }
    }

    public class Rule
    {
        public string Name { get; set; }
        public string Param { get; set; }
        public string Option { get; set; }
    }

    public class Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class TypeValidation : IValidationTemplate
    {
        public string NomValidation { get; set; }
        public string Value { get; set; }
    }

    public interface IValidationTemplate
    {
        string NomValidation { get; set; }
        string Value { get; set; }
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
