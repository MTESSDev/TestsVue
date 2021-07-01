using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ECSForm.Pages
{
    public class SaveBindingModel : PageModel
    {
        private readonly ILogger<SaveBindingModel> _logger;

        public SaveBindingModel(ILogger<SaveBindingModel> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> OnPost(string id, string gabarit, [FromBody] Dictionary<string, BindElement> data)
        {
            var cleanId = id.Replace('@', '/');
            //var cleanPdf = pdf.Replace('@', '/');
            if (!System.IO.File.Exists(@$"mapping/{cleanId}/ecsbind.yml"))
            {
                return NotFound();
            }

            var bind = BindingIndexModel.ReadYamlCfg(@$"mapping/{cleanId}/ecsbind.yml");

            if (bind.Bind.ContainsKey(gabarit))
            {
                bind.Bind[gabarit] = data;
            }
            else
            {
                bind.Bind.Add(gabarit, data);
            }

            BindingIndexModel.SaveYamlCfg(bind, @$"mapping/{cleanId}/ecsbind.yml");
        
            return Page();
        }
    }
}
