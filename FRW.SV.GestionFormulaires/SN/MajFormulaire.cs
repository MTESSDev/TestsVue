using FRW.TR.Commun;
using SV.GestionFormulaires.DAL;
using System.Threading.Tasks;

namespace SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Mettre à jour un formulaire
    /// </summary>
    public class MajFormulaire
    {
        private readonly DalFormulaires _dal;

        //DalFormulaires dans startup
        public MajFormulaire(DalFormulaires dalFormulaires)
        {
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
            var confirmation = await _dal.MettreAJour(noSeqForm, contenuForm);
            await SuiviEtat.Creer(noSeqForm, Constantes.EtatMettreAJour);
            return confirmation;
        }
    }
}
