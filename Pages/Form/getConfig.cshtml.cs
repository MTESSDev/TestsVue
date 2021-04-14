using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ECSForm.Pages
{
    public class GetConfigModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public GetConfigModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            if (!System.IO.File.Exists(@$"schemas/{id}.ecsform.yml"))
            {
                return NotFound();
            }

            string cfg;

            using (var configFile = new StreamReader(@$"schemas/{id}.ecsform.yml"))
            {
                cfg = await configFile.ReadToEndAsync();
            }

            return Content(cfg);
        }
    }
}
