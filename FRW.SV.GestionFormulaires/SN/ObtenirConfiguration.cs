using FRW.TR.Commun;
using System;
using System.Collections.Generic;
using System.Text;

namespace SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Obtenir la configuration du formulaire
    /// </summary>
    public class ObtenirConfiguration
    {
        public ObtenirConfiguration() { }

        //TODO 
        public string ObtenirFichierConfig(string typeFormulaire)
        {
            switch (typeFormulaire)
            {
                case Constantes.DemandeAide:
                case Constantes.DepotDirect:
                case Constantes.ChangementSituation:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
