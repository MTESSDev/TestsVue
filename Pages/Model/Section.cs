using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ECSForm.Model
{
    public class SectionBloc
    {
        public IDictionary<object, object>? Section { get; set; }
        public string? Classes { get; set; }
        public string Id { get; set; } = string.Empty;
        public IEnumerable<object>? components { get; set; }

        public IEnumerable<object>? InputsHandled(){
            if (components is null) yield break;

            foreach (dynamic input in components)
            {
                yield return new { isGroup = input["type"] == "group", attributes = input };
            }
        }
    }
}