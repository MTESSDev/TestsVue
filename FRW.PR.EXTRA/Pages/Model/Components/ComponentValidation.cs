using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FRW.PR.Model.Components
{
    public class ComponentValidation : BaseComponent
    {
        public ComponentValidation() : base(true)
        {
            Validations = new List<ValidationAttribute>();
        }
    }
}
