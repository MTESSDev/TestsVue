using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace WebApplication2.Pages
{
    public class SectionBloc
    {
        public IDictionary<object, object>? Section { get; set; }
        public string Id { get; set; } = string.Empty;
        public IEnumerable<object>? Inputs { get; set; }

        public IEnumerable<object>? InputsHandled(){
            if (Inputs is null) yield break;

            foreach (dynamic input in Inputs)
            {
                yield return new { isGroup = input["type"] == "group", attributes = input };
            }
        }
    }
}