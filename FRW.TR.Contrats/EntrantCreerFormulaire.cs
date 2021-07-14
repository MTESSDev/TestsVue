using System;
using System.Collections.Generic;
using System.Text;

namespace FRW.TR.Contrats
{
    public class EntrantCreerFormulaire
    {
        public string TypeFormulaire { get; set; } = string.Empty;

        public IDictionary<object, object> Data { get; set; } = new Dictionary<object, object>();

        public string? NoFormPublic { get; set; } = null;

        public string? IdSysAppelant { get; set; } = null;
    }
}
