using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FRW.PR.Model.Components
{
    public class ComponentBinding : BaseComponent
    {
        public ComponentBinding() : base(false)
        {
        }

        public List<string>? NameValues { get; set; }
        public string? SectionName { get; set; }
        public int? MaxOccur { get; set; }
    }
}
