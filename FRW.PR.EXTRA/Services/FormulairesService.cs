using ECS.TR.Contrats;
using FRW.TR.Contrats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRW.PR.Extra.Services
{
    // Ouverture de la page
    // Appeler SN : Obtenir la configuration du formulaire
    // Créer cookie de session
    // Appeler SN : Créer formulaire

    // Bouton "Enregistrer"
    // Vérifier dans cookie si courriel envoyé
    // Si oui Appeler SN : Mettre à jour un formulaire
    // Si non
    // Récupérer contenu popup
    // Appeler Mettre à jour
    // Envoyer courriel 
    public class FormulairesService
    {
        private readonly IDorsale _dorsale;

        public FormulairesService(IDorsale dorsale)
        {
            _dorsale = dorsale;
        }

        public async Task<AppelSortant<string>> ObtenirConfigurationFormulaire(string nomForm)
        {
            //todo Vérifier si ça a besoin d'être exposé
            throw new NotImplementedException();
        }
        public async Task<AppelSortant<string>> ObtenirBindingFormulaire(string nomForm)
        {
            //todo Vérifier si ça a besoin d'être exposé
            throw new NotImplementedException();
        }

        //public async Task CreerCookie()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task CreerFormulaire()
        {
            throw new NotImplementedException();
        }

        //public async Task VerifierCookie()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task MajFormulaire()
        {
            throw new NotImplementedException();
        }

        //public async Task EnvoyerCouriel()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
