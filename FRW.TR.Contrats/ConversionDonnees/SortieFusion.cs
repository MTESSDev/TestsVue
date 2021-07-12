using System.Collections.Generic;

namespace FRW.TR.Contrats.ConversionDonnees
{
    public class SortieFusion
    {
        public Dictionary<string, Dictionary<string, string>>? Config { get; set; }
        public List<Template>? Gabarits { get; set; }
    }
}