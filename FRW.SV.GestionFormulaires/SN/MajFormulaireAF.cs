using FRW.SV.GestionFormulaires.SN;
using FRW.TR.Commun;
using SV.GestionFormulaires.DAL;
using System.Threading.Tasks;

namespace FRW.SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Mettre à jour un formulaire
    /// </summary>
    public class MajFormulaireAF
    {
        private readonly DalFormulaires _dal;
        private readonly Courriel _courriel;

        //DalFormulaires dans startup
        public MajFormulaireAF(DalFormulaires dalFormulaires)
        {
            _dal = dalFormulaires;
        }

        /// <summary>
        /// Mettre à jour le formulaire déjà commencé
        /// </summary>
        /// <param name="noSeqForm">Numéro séquentiel du formulaire</param>
        /// <param name="contenuForm">Contenu du formulaire</param>
        /// <returns>Un numéro de confirmation</returns>
        public async Task<string> Traitement(string noSeqForm, string contenuForm, bool envoyerCourriel)
        {
            //TODO si le courriel n'est pas encore envoyé, le faire
            var confirmation = await _dal.MettreAJour(noSeqForm, contenuForm);
            await SuiviEtat.Creer(noSeqForm, Constantes.EtatMettreAJour);
            if (envoyerCourriel)
            {
                await _courriel.Envoyer();
            }

            return confirmation;
        }
    }
}
