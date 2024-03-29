﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FRW.PR.Extra.Pages
{
    public class GetPdfModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public GetPdfModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

       
        public async Task<IActionResult> OnGet(string id, string pdf)
        {
            var cleanId = id.Replace('@', '/');
            var cleanPdf = pdf.Replace('@', '/');
            if (!System.IO.File.Exists(@$"mapping/{cleanId}/{cleanPdf}.pdf"))
            {
                return NotFound();
            }

            var dataBytes = System.IO.File.ReadAllBytes(@$"mapping/{cleanId}/{cleanPdf}.pdf");

            return File(dataBytes, "application/pdf");
        }
    }
}
