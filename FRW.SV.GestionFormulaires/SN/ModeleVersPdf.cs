﻿using FRW.TR.Commun;
using FRW.SV.GestionFormulaires.SN;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FRW.SV.GestionFormulaires.SN
{
    /// <summary>
    /// FRW313 - Préparer la production du PDF associé à un formulaire dynamique
    /// </summary>
    public class ModeleVersPdf
    {
        /// <summary>
        /// Envoyer un courriel de récupération du formulaire
        /// </summary>
        /// <returns></returns>
        public async Task EnvoyerCourriel()
        {
            //TODO Envoyer le courriel

            //TODO mettre le bon numéro
            await SuiviEtat.Creer("", Constantes.EtatCourriel);

            throw new NotImplementedException();
        }
    }
}
