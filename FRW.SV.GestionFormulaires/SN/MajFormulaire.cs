using FRW.SV.GestionFormulaires.SN;
using FRW.TR.Commun;
using FRW.TR.Contrats;
using SV.GestionFormulaires.DAL;
using System.Threading.Tasks;

namespace FRW.SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Mettre à jour un formulaire
    /// </summary>
    public class MajFormulaire
    {
        private readonly DalFormulaires _dal;
        private readonly ModeleVersPdf _modeleVersPdf;

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
        /// <param name="envoyerCourriel">True si pas encore envoyé</param>
        /// <returns>Un numéro de confirmation</returns>
        public async Task<string> Traitement(EntrantMajFormulaire entrant)
        {
            //TODO si le courriel n'est pas encore envoyé, le faire
            var confirmation = await _dal.MettreAJour(entrant.NsFormulaire, entrant.Data);
            await SuiviEtat.Creer(entrant.NsFormulaire, Constantes.EtatMettreAJour);
            if (entrant.EnvoyerCourriel)
            {
                await _modeleVersPdf.EnvoyerCourriel();
            }

            return confirmation;
        }
    }
}
