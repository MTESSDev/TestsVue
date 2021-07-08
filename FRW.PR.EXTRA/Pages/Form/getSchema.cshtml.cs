using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FRW.PR.Extra.Pages
{
    public class GetSchemaModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public GetSchemaModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!System.IO.File.Exists(@$"schemas/ecsform.json"))
            {
                return NotFound();
            }

            string cfg;

            using (var configFile = new StreamReader(@$"schemas/ecsform.json"))
            {
                cfg = await configFile.ReadToEndAsync();
            }

            return Content(cfg);
        }
    }
}
