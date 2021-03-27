using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApplication2.Pages
{
    internal class FormData
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
            //AcceptedValues = new List<string>();
        }

        public void SetType(string type)
        {
            switch (type.ToUpper())
            {
                case "SELECT":
                    Type = TypeInput.SELECT;
                    break;
                case "CHECKBOX":
                    Type = TypeInput.CHECKBOX;
                    break;
                default:
                    Type = TypeInput.TEXT;
                    break;
            }
        }

        public string Name { get; set; }
        public TypeInput Type { get; set; }
        public Dictionary<string, string> AcceptedValues { get; set; }

        public List<ValidationAttribute> Validations { get; set; }

        public void ParseAttribute(Attribute attr)
        {
            switch (attr.Name.ToUpper())
            {
                case "NAME":
                    Name = attr.Value;
                    break;
                case "TYPE":
                    SetType(attr.Value);
                    break;
                case "V-IF":
                    Vif = true;
                    break;
                case "VALIDATION":
                    var rules = ParseValidations(attr.Value);
                    Validations = ConvertRules(rules).ToList();
                    break;
                case ":OPTIONS":
                    var val = JsonConvert.DeserializeObject<Dictionary<string, string>>(attr.Value);
                    AcceptedValues = val;
                    break;
                default:
                    break;
            }
            /* if (attr.Name.Equals("TYPE", System.StringComparison.InvariantCultureIgnoreCase))
             {
                 SetType(attr.Value);
             }
             else if (attr.Name.Equals("validation", System.StringComparison.InvariantCultureIgnoreCase))
             {

             }
             else if (attr.Name.Equals(":options", System.StringComparison.InvariantCultureIgnoreCase))
             {
                 var val = JsonConvert.DeserializeObject<Dictionary<string, string>>(attr.Value);
                 AcceptedValues = val;
             }*/

            Attributes.Add(attr);
        }

        private IEnumerable<ValidationAttribute> ConvertRules(List<Rule> rules)
        {
            foreach (var item in rules)
            { 
                switch (item.Name.ToLower())
                {
                    case "required":
                        yield return new RequiredAttribute() {  };
                        break;
                    default:
                        break;
                }
            }
        }

        private List<Rule> ParseValidations(string value)
        {
            List<Rule> rules = new List<Rule>();

            var list = value.Split('|');
            foreach (var item in list)
            {
                var newRule = new Rule();

                var keyPair = item.Split(':');
                newRule.Name = keyPair[0].Trim('^');
                if (keyPair.Length > 1)
                {
                    newRule.Param = keyPair[1];
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
        public bool Vif { get; private set; }
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
    SELECT,
    CHECKBOX,
    RADIO,
    TEXT
}
