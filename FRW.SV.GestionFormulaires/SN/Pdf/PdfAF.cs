using FRW.TR.Commun;
using FRW.SV.GestionFormulaires.SN;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRW.TR.Contrats.ConversionDonnees;

namespace FRW.SV.GestionFormulaires.SN.Pdf
{
    public class GestionPdf
    {
        public byte[] FusionnerDonnees(DonneesChargement donneesChargement)
        {
           
            // Pour chaque gabarit à remplir
            // Ouvrir PDF
            // Remplir (Si la value dépasse la longeur du champ, moins de 20%, diminuer taille, plus de 20%, envoyer en annexe)
            // Next

            //Retourner le document complet, en byte array
            return new byte[0];
        }
    }
}
