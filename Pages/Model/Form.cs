using System.Collections.Generic;

namespace WebApplication2.Pages
{
    public class Form
    {
        public IEnumerable<SectionBloc>? Sections { get; set; }
        public IDictionary<string, string> templates { get; set; }
    }
}