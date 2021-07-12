using Microsoft.AspNetCore.Mvc;
using FRW.TR.Contrats;
using FRW.SV.GestionFormulaires.SN.Pdf;
using FRW.TR.Contrats.ConversionDonnees;
using Swashbuckle.Examples;

namespace FRW.SV.GestionFormulaires.Controller
{
    [Route("/api/v1/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PdfController: ControllerBase
    {
        private readonly GestionPdf _gestionPdf;

        public PdfController(GestionPdf gestionPdf)
        {
            _gestionPdf = gestionPdf;
        }

        /// <summary>
        /// "Copie" de GCO
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /ProduirePdf
        ///     {
        ///       "config": {
        ///         "pdf": {
        ///           "pourcentRapetissement": "20",
        ///           "modeFusionDepassement": "rapetissementEtAnnexe"
        ///         }
        ///       },
        ///       "gabarits": [
        ///         {
        ///           "proprietes": {
        ///             "id": "01_a1",
        ///             "condition": null,
        ///             "name": "3003-01 - Adulte 1",
        ///             "pdf": "3003-01_ProjetDynDEV"
        ///           },
        ///           "champs": {
        ///             "DM_AFDR_PerteRevenus": "true",
        ///             "PerteRevenus1": "true"
        ///           }
        ///         },
        ///         {
        ///         "proprietes": {
        ///             "id": "01_a2",
        ///             "condition": null,
        ///             "name": "3003-01 - Adulte 2",
        ///             "pdf": "3003-01_ProjetDynDEV"
        ///           },
        ///           "champs": { }
        ///         }
        ///       ]
        ///     }
        ///
        /// </remarks>
        /// <param name="donneesChargement">Données à charger dans les gabarits de PDF.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AppelSortant<byte[]>> ProduirePdf(DonneesChargement donneesChargement)
        {
            var retourGestionPdf = _gestionPdf.FusionnerDonnees(donneesChargement);

            return new AppelSortant<byte[]>() { Sortie = retourGestionPdf };
        }
    }
}
