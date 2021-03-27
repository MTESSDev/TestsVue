using Stubble.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication2.Pages
{
    internal class FormHelpers
    {
        public static string JsObject(HelperContext context, IDictionary<string, string>? dict)
        {
            return "{" + string.Join(", ", dict.Select(kv => kv.Key + $": '{kv.Value.Replace("'", "\\'")}'").ToArray()) + "}";
        }

        public static string JsArray(HelperContext context, IDictionary<string, string>? dict)
        {

            return "[" + string.Join(", ", dict.Select(kv => $"['{kv.Key}', {(kv.Value.Contains(",") ? $"'{kv.Value}'" : kv.Value)}]").ToArray()) + "]";
        }
        /*
         :validation="[
            ['required'],
            ['max', 10],
            ['min', 5]
          ]"
         * */
    }
}