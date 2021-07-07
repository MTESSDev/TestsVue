using System;
using System.Collections.Generic;
using System.Linq;

namespace ECSForm.Utils
{
    public class VueParser : IVueParser
    {
        public Dictionary<string, object?> ParseData<TModel>(TModel model)
        {
            var props = model?.GetType().GetProperties();
            var result = new Dictionary<string, object?>();

            if(props is null) { return result; }

            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes(typeof(VueData), true)?.FirstOrDefault()
                    as VueData;

                if (attr == null)
                {
                    continue;
                }

                result.Add(attr.Name, prop?.GetValue(model) ?? "");
            }

            return result;
        }
    }
}