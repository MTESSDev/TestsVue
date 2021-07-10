using FRW.TR.Commun;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FRW.PR.Extra.Pages
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

            var bind = OutilsYaml.ReadYamlCfg<Binder>(@$"mapping/{cleanId}/ecsbind.yml");

            if (bind.Bind.ContainsKey(gabarit))
            {
                bind.Bind[gabarit] = data;
            }
            else
            {
                bind.Bind.Add(gabarit, data);
            }

            OutilsYaml.SaveYamlCfg(bind, @$"mapping/{cleanId}/ecsbind.yml");
        
            return Page();
        }
    }
}
