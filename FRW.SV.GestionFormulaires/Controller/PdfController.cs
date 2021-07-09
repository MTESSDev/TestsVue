using Microsoft.AspNetCore.Mvc;
using FRW.SV.GestionFormulaires.SN;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRW.TR.Contrats;

namespace FRW.SV.GestionFormulaires.Controller
{
    [Route("/api/v1/[controller]/[action]")]
    [ApiController]
    public class PdfController: ControllerBase
    {

        public PdfController()
        {
        }

        /// <summary>
        /// Copie de GCO
        /// </summary>
        /// <param name="nomForm"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<AppelSortant<string>> ProduirePdf(string nomForm)
        {
            return NotFound();
        }
    }
}
