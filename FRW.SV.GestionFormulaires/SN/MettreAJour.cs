using FRW.TR.Commun;
using SV.GestionFormulaires.DAL;
using System.Threading.Tasks;

namespace SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Mettre à jour un formulaire
    /// </summary>
    public class MettreAJour
    {
        private readonly CreerSuiviEtat _creerSuiviEtat;
        private readonly DalFormulaires _dal;

        //DalFormulaires dans startup
        public MettreAJour(CreerSuiviEtat creerSuiviEtat, DalFormulaires dalFormulaires)
        {
            _creerSuiviEtat = creerSuiviEtat;
            _dal = dalFormulaires;
        }

        /// <summary>
        /// Mettre à jour le formulaire déjà commencé
        /// </summary>
        /// <param name="noSeqForm">Numéro séquentiel du formulaire</param>
        /// <param name="contenuForm">Contenu du formulaire</param>
        /// <returns>Un numéro de confirmation</returns>
        public async Task<string> Traitement(string noSeqForm, string contenuForm)
        {
            return await _dal.MettreAJour(noSeqForm, contenuForm);
            await _creerSuiviEtat.Traitement(noSeqForm, Constantes.EtatMettreAJour);
        }
    }
}
