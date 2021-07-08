using FRW.TR.Commun;
using SV.GestionFormulaires.DAL;
using System;
using System.Threading.Tasks;

namespace SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Créer formulaire
    /// </summary>
    public class CreerFormulaireAF
    {
        private readonly DalFormulaires _dal;

        /// <summary>
        /// Todo configurer les dépendances dans startup
        /// </summary>
        /// <param name="creerSuiviEtat"></param>
        public CreerFormulaireAF(DalFormulaires dalFormulaires)
        {
            _dal = dalFormulaires;
        }

        /// <summary>
        /// Traiter la création d'un formulaire
        /// </summary>
        /// <param name="typeFormulaire">Type de formulaire</param>
        /// <param name="idSysAppelant">Identifiant du système appelant</param>
        /// <param name="contenuFormulaire">Contenu à sauvegarder</param>
        public async Task<string> Traitement(string typeFormulaire, string idSysAppelant, string? contenuFormulaire = null)
        {
            var numeroForm = GenererNumeroFormulaire();
            var numeroConf = await Enregistrer(typeFormulaire, idSysAppelant, numeroForm, contenuFormulaire);
            await SuiviEtat.Creer(numeroForm, Constantes.EtatCreer);
            return numeroConf;
        }

        /// <summary>
        /// Créer enregistrement dans la table formulaire
        /// </summary>
        /// <param name="typeFormulaire">Type de formulaire</param>
        /// <param name="idSysAppelant">Identifiant du système appelant</param>
        /// <param name="numeroForm">Numéro public du formulaire</param>
        /// <param name="contenuFormulaire">Contenu à sauvegarder</param>
        private async Task<string> Enregistrer(string typeFormulaire, string idSysAppelant, string numeroForm, string contenuFormulaire)
        {
            return await _dal.Creer(typeFormulaire, idSysAppelant, numeroForm, contenuFormulaire);
        }

        /// <summary>
        /// Générer un numéro public unique de type GUID
        /// </summary>
        /// <returns>un numéro unique</returns>
        private string GenererNumeroFormulaire()
        {
            throw new NotImplementedException();
        }
    }
}
