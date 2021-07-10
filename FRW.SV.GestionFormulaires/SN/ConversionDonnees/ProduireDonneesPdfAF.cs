using FRW.TR.Commun;
using FRW.SV.GestionFormulaires.SN;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FRW.SV.GestionFormulaires.SN.ConversionDonnees
{
    public class ProduireDonneesPdfAF
    {
        private readonly ObtenirConfiguration _obtenirConfiguration;

        public ProduireDonneesPdfAF(ObtenirConfiguration obtenirConfiguration)
        {
            _obtenirConfiguration = obtenirConfiguration;
        }

        public async Task Convertir(string typeFormulaire)
        {
            // Lire le fichier de binding
            var config = await _obtenirConfiguration.ObtenirFichierConfig(typeFormulaire);
            // pour chaque gabarit
            

            // pour chaque champ dans la config
            // lire la source définie
            // ajouter les infos dans un objet "gabaritX" qui contient tous les champs ayant une valeur à mapper dans le pdf
            // retourner le tableau d'objet de mappage (une map par gabarit pdf)
        }
    }
}
