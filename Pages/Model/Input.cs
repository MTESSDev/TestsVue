using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace WebApplication2.Pages
{
    public class Input
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string ValidationName { get; set; } = string.Empty;
        public string Help { get; set; } = string.Empty;
        public string AddLabel { get; set; } = string.Empty;
        public Dictionary<string, string>? Validations { get; set; }
        public bool Repeatable { get; set; }
        public Dictionary<string, string>? Options { get; set; }
        public IEnumerable<Input>? Inputs { get; set; }
    }
}