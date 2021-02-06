using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string Formulaire { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;


        }

        public void OnGet()
        {
            using (var r = new StreamReader(@"form.yml"))
            {
                var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();

                var yamlObject = deserializer.Deserialize(r);

                var serializer = new SerializerBuilder()
                 .JsonCompatible()
                 .Build();

                Formulaire = serializer.Serialize(yamlObject);
            }
        }
    }
}
