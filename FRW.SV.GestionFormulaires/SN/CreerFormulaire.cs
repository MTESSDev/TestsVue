using FRW.TR.Commun;
using FRW.TR.Contrats;
using SV.GestionFormulaires.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FRW.SV.GestionFormulaires.SN
{
    /// <summary>
    /// SN - Créer formulaire
    /// </summary>
    public class CreerFormulaire
    {
        private readonly DalFormulaires _dal;

        /// <summary>
        /// Todo configurer les dépendances dans startup
        /// </summary>
        /// <param name="dalFormulaires">Accès aux données</param>
        public CreerFormulaire(DalFormulaires dalFormulaires)
        {
            _dal = dalFormulaires;
        }

        /// <summary>
        /// Traiter la création d'un formulaire
        /// <param name="entrant">EntrantCreerFormulaire</param>param>
        /// </summary>
        public async Task<string> Traitement(EntrantCreerFormulaire entrant)
        {
            entrant.NoFormPublic = GenererNumeroFormulaire();
            var numeroConf = await Enregistrer(entrant);
            await SuiviEtat.Creer(entrant.NoFormPublic, Constantes.EtatCreer);
            return numeroConf;
        }

        /// <summary>
        /// Créer enregistrement dans la table formulaire
        /// </summary>
        /// <param name="entrant">Informations cénessaires à la création du formulaire</param>
        private async Task<string> Enregistrer(EntrantCreerFormulaire entrant)
        {
            return await _dal.Creer(entrant);
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
