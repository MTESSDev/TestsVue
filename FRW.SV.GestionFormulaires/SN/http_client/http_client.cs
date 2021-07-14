using FRW.TR.Commun;
using FRW.SV.GestionFormulaires.SN;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRW.TR.Contrats.ConversionDonnees;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using FRW.SV.GestionFormulaires.Utils;
using System.Net.Http;

namespace FRW.SV.GestionFormulaires.SN.http_client
{
    /// <summary>
    /// Classe de production de PDF avec gestion des données trop longues.
    /// </summary>
    public class YamlHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public YamlHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> AppelerService(string url)
        {
            // Rechercher dans le YAML la config qui match

            // Instancier un client

            // Ajouter les params

            // Appeler l'url

            var client = _httpClientFactory.CreateClient();
        
            //client.BaseAddress = new Uri("http://api.github.com");
            string result = await client.GetStringAsync("/");
            return result;
        }
    }
}
