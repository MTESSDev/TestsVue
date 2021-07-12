using System.Collections.Generic;

namespace FRW.SV.GestionFormulaires.SN.ConversionDonnees
{
    public class SortieFusion
    {
        public Dictionary<string, Dictionary<string, string>>? Config { get; set; }
        public List<Template>? Gabarits { get; set; }

    }

    public class Template
    {
        public Dictionary<string, string>? Proprietes { get; set; }
        public Dictionary<string, string>? Champs { get; set; }

    }
}