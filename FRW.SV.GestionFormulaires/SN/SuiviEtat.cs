using FRW.TR.Commun;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Créer suivi état formulaire
    /// </summary>
    public static class SuiviEtat
    {
        public static async Task Creer(string numeroForm, string etat)
        {
            //TODO Mettre la date dans l'enregistrement.
            switch (etat)
            {
                case Constantes.EtatCreer:
                case Constantes.EtatMettreAJour:
                case Constantes.EtatCourriel:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
