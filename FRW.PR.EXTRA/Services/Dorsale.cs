using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FRW.TR.Contrats;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECS.PR.Extra.Services
{
    public class Dorsale : IDorsale
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        //private readonly IContexte _contexte;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private static Serilog.ILogger Log => Serilog.Log.ForContext<Dorsale>();

        public Dorsale(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            //var cert = new X509Certificate2("C:\\Installation\\ECS.Proxy.un.pfx", "C1password");
            _httpClient?.DefaultRequestHeaders.Add("Accept", "application/x-msgpack");
            //_httpClient?.DefaultRequestHeaders.Add("X-ARR-ClientCert", cert.GetRawCertDataString());
        }

        public async Task<AppelSortant<TRecevoir>> Recevoir<TRecevoir>(string serviceDestination, string addresseApi, string? codeNtForce = null, [CallerMemberName] string appelant = "")
        {
            return await EnvoyerRecevoir<Rien, TRecevoir>(new Rien(), serviceDestination, addresseApi, codeNtForce, appelant);
        }

        public async Task<AppelSortant> Envoyer<TEnvoyer>(TEnvoyer envoyer, string serviceDestination, string addresseApi, string? codeNtForce = null, [CallerMemberName] string appelant = "") where TEnvoyer : class
        {
            return await EnvoyerRecevoir<TEnvoyer, Rien>(envoyer, serviceDestination, addresseApi, codeNtForce, appelant);
        }

        public async Task<AppelSortant> Executer(string serviceDestination, string addresseApi, string? codeNtForce = null, [CallerMemberName] string appelant = "")
        {
            return await EnvoyerRecevoir<Rien, Rien>(new Rien(), serviceDestination, addresseApi, codeNtForce, appelant);
        }

        public async Task<AppelSortant<TRecevoir>> EnvoyerRecevoir<TEnvoyer, TRecevoir>(TEnvoyer envoyer, string serviceDestination, string addresseApi, string? codeNtForce = null, [CallerMemberName] string appelant = "") where TEnvoyer : class
        {
            return await EnvoyerRecevoir<TEnvoyer, TRecevoir>(envoyer, serviceDestination, addresseApi, codeNtForce, true, appelant);
        }

        public async Task<AppelSortant<TRecevoir>> EnvoyerRecevoir<TEnvoyer, TRecevoir>(TEnvoyer envoyer, string serviceDestination, string addresseApi, string? codeNtForce, bool passerECS, [CallerMemberName] string appelant = "") where TEnvoyer : class
        {
            //_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(EcsHttpMessageHandler.CleEnteteHttpUrlServiceBackEnd, out var urlDorsaleEnteteHttp);

            //var urlDorsale = string.IsNullOrWhiteSpace(urlDorsaleEnteteHttp.ToString()) ? _configuration["ECS:UrlDorsale"] : urlDorsaleEnteteHttp.ToString();
            //var uriComplete = urlDorsale + "/" + serviceDestination + addresseApi;

            //AppelSortant<TRecevoir> recevoir;

            //var codeNt = codeNtForce ?? _contexte.CAC?.CodeUtilisateurComplet;
            //Log.Debug("Appel service proxy pour {uriComplete}", uriComplete, codeNt, appelant);

            //try
            //{
            //    using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uriComplete) { Content = ObtenirContenuMessageEnvoyer(envoyer, passerECS) };
            //    httpRequestMessage.Headers.Add("CodeNT", codeNt);
            //    if (!string.IsNullOrWhiteSpace(urlDorsaleEnteteHttp.ToString()))
            //    {
            //        httpRequestMessage.Headers.Add(EcsHttpMessageHandler.CleEnteteHttpUrlServiceBackEnd, urlDorsaleEnteteHttp.ToString());
            //    }

            //    var result = await _httpClient.SendAsync(httpRequestMessage);
            //    var bytes = await result.Content.ReadAsByteArrayAsync();

            //    if (result.IsSuccessStatusCode)
            //    {
            //        recevoir = MessagePackSerializer.Deserialize<AppelSortant<TRecevoir>>(bytes);
            //    }
            //    else
            //    {
            //        if (result.Content.Headers != null &&
            //            result.Content.Headers.ContentType != null && result.Content.Headers.ContentType.MediaType.Equals("application/json"))
            //        {
            //            var erreur = System.Text.Json.JsonSerializer.Deserialize<Erreur>(bytes);
            //            throw new DorsaleException($"Objet erreur reçu seul", uriComplete, erreur, null);
            //        }
            //        else
            //        {
            //            throw new DorsaleException($"Erreur { result.StatusCode } - { result.ReasonPhrase}\r\n body : \r\n { result.Content.ReadAsStringAsync().Result}",
            //                               uriComplete, null, null);
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Fatal(ex, "Erreur avec l'appel à la dorsale {uriComplete}", new { uriComplete, codeNt, appelant });

            //    throw;
            //}

            //return recevoir;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtient le contenu du message à envoyer.
        /// </summary>
        /// <typeparam name="TEnvoyer">Type des données à envoyer.</typeparam>
        /// <param name="envoyer">Les données à envoyer.</param>
        /// <param name="passerECS"></param>
        /// <returns></returns>
        private HttpContent ObtenirContenuMessageEnvoyer<TEnvoyer>(TEnvoyer envoyer, bool passerECS) where TEnvoyer : notnull
        {
            //byte[] buffer;

            //if (envoyer is Rien)
            //{
            //    buffer = MessagePackSerializer.Serialize(new AppelEntrant()
            //    {
            //        ContexteCAC = (ContexteCAC)_contexte.CAC,
            //        ContexteECS = passerECS ? (ContexteECS?)_contexte.ECS : null
            //    });
            //}
            //else
            //{
            //    buffer = MessagePackSerializer.Serialize(new AppelEntrant<TEnvoyer>()
            //    {
            //        ContexteCAC = (ContexteCAC)_contexte.CAC,
            //        ContexteECS = (ContexteECS?)_contexte.ECS,
            //        Entree = envoyer
            //    });
            //}

            //var contenuMessage = new ByteArrayContent(buffer);
            //contenuMessage.Headers.Add("Content-Type", "application/x-msgpack");

            //return contenuMessage;
            throw new NotImplementedException();
        }
    }
}
