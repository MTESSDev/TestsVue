using Stubble.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication2.Pages
{
    internal class FormHelpers
    {
        public static string JsObject(HelperContext context, IDictionary<object, object>? dict)
        {
            return "{" + string.Join(", ", dict.Select(kv => kv.Key + $": '{kv.Value.ToString().Replace("'", "\\'")}'").ToArray()) + "}";
        }

        public static string JsArray(HelperContext context, IDictionary<object, object>? dict)
        {
            return "[" + string.Join(", ", dict.Select(kv => $"['{kv.Key}', '{kv.Value}']").ToArray()) + "]";
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