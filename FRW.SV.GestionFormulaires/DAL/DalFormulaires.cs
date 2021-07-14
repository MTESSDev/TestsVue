using FRW.TR.Contrats;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SV.GestionFormulaires.DAL
{
    /// <summary>
    /// Accès aux données pour les formulaires
    /// </summary>
    public class DalFormulaires
    {
        /// <summary>
        /// Créer une entrée dans la table des formulaires en cours
        /// </summary>
        /// <param name="entrant"></param>
        /// <returns>Un numéro de confirmation</returns>
        public async Task<string> Creer(EntrantCreerFormulaire entrant)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mettre à jour une entrée dans la table des formulaires en cours
        /// </summary>
        /// <param name="noSeqForm"></param>
        /// <param name="contenuForm"></param>
        /// <returns>Un numéro de confirmation</returns>
        public async Task<string> MettreAJour(string noSeqForm, string contenuForm)
        {
            //TODO faire un EntrantMettreAJour
            throw new NotImplementedException();
        }


    }
}
