using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FRW.TR.Contrats
{
    /// <summary>
    /// Classe générique d'appel sortant
    /// </summary>
    [DataContract]
    public class AppelSortant
    {
        /// <summary>
        /// Liste des erreurs
        /// </summary>
        [DataMember]
        public IEnumerable<Erreur>? Erreurs { get; set; }

    }

    /// <summary>
    /// Classe générique d'appel sortant
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class AppelSortant<T> : AppelSortant
    {
        /// <summary>
        /// Données de l'appel
        /// </summary>
        [DataMember]
        public T Sortie { get; set; } = default!;

    }
}
