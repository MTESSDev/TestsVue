using System;
using System.Collections.Generic;
using System.Text;

namespace FRW.TR.Contrats
{
    public class EntrantMajFormulaire
    {
        public string NsFormulaire { get; set; } = string.Empty;

        public IDictionary<object, object> Data = new Dictionary<object, object>();

        public bool EnvoyerCourriel { get; set; }
    }
}
