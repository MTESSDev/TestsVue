using FRW.TR.Contrats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRW.PR.Extra.Services
{ 
    public class FormulairesService
    {
        private readonly IDorsale _dorsale;
        private const string _serviceFormulaires = "FRW.SV.GestionFormulaires";
        private const string _apiPath = "api/v1/Formulaires";

        public FormulairesService(IDorsale dorsale)
        {
            _dorsale = dorsale;
        }

        public async Task<string> ObtenirConfiguration(string nomForm)
        {
            var a = await _dorsale.EnvoyerRecevoir<string, string>(nomForm, _serviceFormulaires, $"{_apiPath}/ObtenirConfiguration");
            
            return a?.Sortie ?? string.Empty;
        }

        public async Task<string> ObtenirBinding(string nomForm)
        {
            var a = await _dorsale.EnvoyerRecevoir<string, string>(nomForm, _serviceFormulaires, $"{_apiPath}/ObtenirBinding");

            return a?.Sortie ?? string.Empty;
        }

        public async Task<string> Creer(string nomForm)
        {
            var a = await _dorsale.EnvoyerRecevoir<string, string>(nomForm, _serviceFormulaires, $"{_apiPath}/Creer");

            return a?.Sortie ?? string.Empty;
        }

        public async Task<string> Maj(EntrantMajFormulaire entrant)
        {
            var a = await _dorsale.EnvoyerRecevoir<EntrantMajFormulaire, string>(entrant, _serviceFormulaires, $"{_apiPath}/Maj");

            return a?.Sortie ?? string.Empty;
        }
    }
}
