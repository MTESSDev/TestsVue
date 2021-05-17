﻿using Newtonsoft.Json;
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
            Form = new Inputs();
            Inputs = new List<Inputs>();
        }

        public Inputs Form { get; set; }
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

        public string? Name { get; set; }
        public string? GroupName { get; set; }
        public TypeInput Type { get; set; }
        public IDictionary<object, object>? AcceptedValues { get; set; }

        public List<ValidationAttribute> Validations { get; set; }

        public void ParseAttributes(IDictionary<object, object>? attr)
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
                switch (item.Name?.ToLower())
                {
                    case "required":
                        yield return new RequiredAttribute() { };
                        break;
                    case "max":
                        yield return new MaxLengthAttribute(int.Parse(item.Param ?? "0"));
                        break;
                    case "optional":
                        break;
                    default:
                        throw new InvalidOperationException(item.Name + " unknown");
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
