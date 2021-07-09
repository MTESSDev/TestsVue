using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FRW.PR.Extra.Pages
{
    public class SaveModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public SaveModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string id)
        {
            //HttpContext.Request.Cookies.TryGetValue("FRW" + id, out var cookie);
            /*if (cookie is null)
            {
                HttpContext.Response.Cookies.Append("FRW" + id, Guid.NewGuid().ToString());
            }
            */
            string jsonData;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonData = await reader.ReadToEndAsync();
            }

            HttpContext.Response.Cookies.Append("FRW" + id, jsonData);

            return new OkResult();
        }
    }
}
