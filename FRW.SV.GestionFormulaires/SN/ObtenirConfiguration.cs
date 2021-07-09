using FRW.TR.Commun;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FRW.SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Obtenir la configuration du formulaire
    /// </summary>
    public class ObtenirConfiguration
    {
        //TODO vérifier si besoin d'injection de dépendances
        public ObtenirConfiguration() { }
                
        //TODO Obtenir chemin répertoires
        public async Task<string> ObtenirFichierConfig(string typeFormulaire)
        {
            //TODO
            throw new NotImplementedException();
        }

        //TODO Obtenir chemin répertoires
        public async Task<string> ObtenirFichierBinding(string typeFormulaire)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
