using Microsoft.AspNetCore.Mvc;
using FRW.SV.GestionFormulaires.SN;
using System.Threading.Tasks;
using FRW.SV.GestionFormulaires.SN.ConversionDonnees;
using FRW.TR.Contrats;
using FRW.TR.Contrats.ConversionDonnees;

namespace FRW.SV.GestionFormulaires.Controller
{
    [Route("/api/v1/[controller]/[action]")]
    [ApiController]
    public class FormulairesController : ControllerBase
    {
        private readonly CreerFormulaire _creerFormulaire;
        private readonly MajFormulaire _majFormulaire;
        private readonly FormConfig _Config;
        private readonly ConvertirDonneesAF _produireDonneesPdfAF;

        public FormulairesController(CreerFormulaire creer,
                                     MajFormulaire maj,
                                     FormConfig formConfig,
                                     ConvertirDonneesAF produireDonneesPdfAF)
        {
            _creerFormulaire = creer;
            _majFormulaire = maj;
            _Config = formConfig;
            _produireDonneesPdfAF = produireDonneesPdfAF;
        }

        [HttpGet]
        public async Task<ActionResult<AppelSortant<string>>> ObtenirConfiguration(string nomForm)
        {
            var a = await _Config.ObtenirConfig(nomForm);
            return Ok(a);
        }

        [HttpGet]
        public async Task<ActionResult<AppelSortant<string>>> ObtenirBinding(string nomForm)
        {
            var a = await _Config.ObtenirBinding(nomForm);
            return Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult<AppelSortant<string>>> Creer(string nomForm)
        {
            // Todo obtenir le idSys dans l'url de la requête ou le fichier de config
            var a = await _creerFormulaire.Traitement(nomForm, "IdSys", null);
            return Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult<AppelSortant<string>>> Maj(string noSeqForm)
        {
            //TODO vérifier le cookie pour déterminer si on envoie le courriel
            bool envoyerCourriel = true;
            var a = await _majFormulaire.Traitement(noSeqForm, null, envoyerCourriel);
            return Ok(a);
        }

        [HttpPost]
        public ActionResult<DonneesChargement> ConvertirDonnees(string typeFormulaire, string jsonData)
        {
            var donneesChargement = _produireDonneesPdfAF.Convertir(typeFormulaire, jsonData);
            return Ok(donneesChargement);
        }
    }
}
