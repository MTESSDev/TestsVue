using System.Collections.Generic;

namespace FRW.TR.Contrats.ConversionDonnees
{
    public class DonneesChargement
    {
        public Dictionary<string, Dictionary<string, string>>? Config { get; set; }
        public List<Template>? Gabarits { get; set; }
    }
}