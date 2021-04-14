using System.Collections.Generic;

namespace ECSForm.Model
{
    public class Form
    {
        public IEnumerable<SectionBloc>? Sections { get; set; }
        public IDictionary<string, string>? Templates { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string>? InputDefaultClasses { get; set; } = new Dictionary<string, string>();
    }
}