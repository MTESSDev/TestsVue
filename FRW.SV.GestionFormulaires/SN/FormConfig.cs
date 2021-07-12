using FRW.TR.Commun;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FRW.SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Obtenir la configuration du formulaire
    /// </summary>
    public class FormConfig
    {
        //TODO vérifier si besoin d'injection de dépendances
        public FormConfig() { }
                
        //TODO Obtenir bon chemin répertoire
        public async Task<string> ObtenirConfig(string typeFormulaire)
        {
            var repertoireConfig = ConfigurationManager.AppSettings["CheminRepertoireConfiguration"];
            if(Directory.Exists(repertoireConfig))
            {
                //DO something
            }

            return string.Empty;
        }

        //TODO Obtenir bon chemin répertoire
        public async Task<string> ObtenirBinding(string typeFormulaire)
        {
            var repertoireBinding = ConfigurationManager.AppSettings["CheminRepertoireBinding"];
            if(Directory.Exists(repertoireBinding))
            {
                //DO something
            }

            return string.Empty;
        }
    }
}
