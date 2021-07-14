using Microsoft.AspNetCore.Mvc;
using FRW.TR.Contrats;
using FRW.SV.GestionFormulaires.SN.Pdf;
using FRW.TR.Contrats.ConversionDonnees;

namespace FRW.SV.GestionFormulaires.Controller
{
    /// <summary>
    /// Gestion Pdf
    /// </summary>
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
        /// FRWxxx - Produire un PDF avec gestion des données trop longues. - "Copie" de GCO
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /ProduirePdf
        ///     {
        ///       "config": {
        ///         "pdf": {
        ///           "rapetisserTexteTropLong": "true",
        ///           "redirigerAnnexeTexteTroplong": "true",
        ///           "pourcentageDepassementAnnexe": "20",
        ///           "verrouillerChampsPdf": "true"
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
        ///             "PerteRevenus1": "true",
        ///             "nom": "Ti-cass dans le bas de la côte la juste après le dépanneur"
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
        /// <returns>Fichier PDF</returns>
        [HttpPost]
        [Produces("application/pdf")]
        public ActionResult ProduirePdf(DonneesChargement donneesChargement)
        {
            var retourGestionPdf = _gestionPdf.FusionnerDonnees(donneesChargement);

            HttpContext.Response.Headers.Add("Content-Type", "application/pdf");
            HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=labels.pdf");

            return new FileContentResult(retourGestionPdf, "application/pdf");
        }
    }
}
