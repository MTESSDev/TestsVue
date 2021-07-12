using SmartFormat.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRW.TR.Commun.Utils
{
    /// <summary>
    /// Formatteur custom pour SmartFormat pour verifier si un Array de string
    /// ou un string inclue ou vaut une valeur spécifique
    /// </summary>
    public class SmartFormatInclude : IFormatter
    {
        private string[] names = new[] { "include" };
        public string[] Names { get { return names; } set { this.names = value; } }

        public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
        {
            bool exist = false;

            if (formattingInfo.CurrentValue is object[] array)
            {
                exist = Array.Exists(array, element => (element.ToString() ?? string.Empty).Equals(formattingInfo.FormatterOptions));
            }
            else
            {
                if ((formattingInfo.CurrentValue ?? string.Empty).Equals(formattingInfo.FormatterOptions))
                {
                    exist = true;
                }
            }

            if (exist)
            {
                formattingInfo.Write(formattingInfo.Format?.RawText ?? string.Empty);
            }

            return true;
        }
    }
}
