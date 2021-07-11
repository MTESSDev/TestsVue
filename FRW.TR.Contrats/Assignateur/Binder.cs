using System;
using System.Collections.Generic;
using System.Text;

namespace FRW.TR.Contrats.Assignateur
{
    public class Binder
    {
        public List<Dictionary<string, string>>? Templates { get; set; }
        public Dictionary<string, Dictionary<string, BindElement>>? Bind { get; set; }
    }

    public class BindElement
    {
        public IEnumerable<string>? Champs { get; set; }
        public string? Formule { get; set; }
    }
}
