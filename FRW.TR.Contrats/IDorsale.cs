using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FRW.TR.Contrats
{
    public interface IDorsale
    {
        Task<AppelSortant<TRecevoir>> Recevoir<TRecevoir>(string serviceDestination, string addresseApi, string? codeNtForce = null, [CallerMemberName] string appelant = "");

        Task<AppelSortant> Envoyer<TEnvoyer>(TEnvoyer envoyer, string serviceDestination, string addresseApi, string? codeNtForce = null, [CallerMemberName] string appelant = "") where TEnvoyer : class;

        Task<AppelSortant> Executer(string serviceDestination, string addresseApi, string? codeNtForce = null, [CallerMemberName] string appelant = "");

        Task<AppelSortant<TRecevoir>> EnvoyerRecevoir<TEnvoyer, TRecevoir>(TEnvoyer envoyer, string serviceDestination, string addresseApi, string? codeNtForce = null, [CallerMemberName] string appelant = "") where TEnvoyer : class;
    }
}
