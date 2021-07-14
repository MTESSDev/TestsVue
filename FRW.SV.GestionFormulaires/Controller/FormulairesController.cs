using Microsoft.AspNetCore.Mvc;
using FRW.SV.GestionFormulaires.SN;
using System.Threading.Tasks;
using FRW.SV.GestionFormulaires.SN.ConversionDonnees;
using FRW.TR.Contrats;
using FRW.TR.Contrats.ConversionDonnees;

namespace FRW.SV.GestionFormulaires.Controller
{
    /// <summary>
    /// Contrôleur pour les formulaires
    /// </summary>
    [Route("/api/v1/[controller]/[action]")]
    [ApiController]
    public class FormulairesController : ControllerBase
    {
        private readonly CreerFormulaire _creerFormulaire;
        private readonly MajFormulaire _majFormulaire;
        private readonly FormConfig _Config;
        private readonly ConvertirDonneesAF _produireDonneesPdfAF;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="creer"></param>
        /// <param name="maj"></param>
        /// <param name="formConfig"></param>
        /// <param name="produireDonneesPdfAF"></param>
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
        public async Task<ActionResult<AppelSortant<string>>> Creer(EntrantCreerFormulaire entrant)
        {
            entrant.IdSysAppelant = "idSys";
            // Todo obtenir le idSys dans l'url de la requête ou le fichier de config
            var a = await _creerFormulaire.Traitement(entrant);
            return Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult<AppelSortant<string>>> Maj(EntrantMajFormulaire entrant)
        {
            var a = await _majFormulaire.Traitement(entrant);
            return Ok(a);
        }

        /// <summary>
        /// FRW313 - Préparer les données associées à un formulaire dynamique pour chargement.
        /// </summary>
        /// <param name="typeFormulaire">Type de formulaire ex: 3003</param>
        /// <param name="jsonData">Json d'un formulaire web.</param>
        /// <returns>DonneesChargement</returns>
        [HttpPost]
        public ActionResult<DonneesChargement> ConvertirDonnees(string typeFormulaire, string jsonData)
        {
            var donneesChargement = _produireDonneesPdfAF.Convertir(typeFormulaire, jsonData);
            return Ok(donneesChargement);
        }
    }
}
