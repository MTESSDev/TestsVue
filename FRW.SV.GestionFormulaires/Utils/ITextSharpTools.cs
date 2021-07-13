using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FRW.SV.GestionFormulaires.Utils
{
    public static class itextSharpTools
    {
        /// <summary>
        /// Retourne la valeur vraie du champ PDF en paramètre.
        /// </summary>
        /// <param name="acroFields">Objet acrofields</param>
        /// <param name="fieldName">Nom du champ à vérifier (doit etre un checkbox)</param>
        /// <returns>La valeur du champ si vrai, ex: On, Oui, True...</returns>
        public static string ObtenirValeurVraieChamp(this AcroFields acroFields, string fieldName)
        {
            var trueValue = string.Empty;
            foreach (var checkval in acroFields.GetAppearanceStates(fieldName))
            {
                if (checkval != "Off")
                {
                    trueValue = checkval;
                    break;
                }
            }
            return trueValue;
        }

        public static double ObtenirPourcentageDebordement(this AcroFields acroFields, string fieldName, string testValue)
        {
            var champPdf = acroFields.GetFieldItem(fieldName);
            var champTexteBidon = new TextField(null, null, null);
            acroFields.DecodeGenericDictionary(champPdf.GetMerged(0), champTexteBidon);

            var texteTest = new Font(champTexteBidon.Font, champTexteBidon.FontSize);
            var currentTextSize = texteTest.GetCalculatedBaseFont(true)
                                            .GetWidthPointKerned(testValue, texteTest.CalculatedSize);

            var fieldPosition = acroFields.GetFieldPositions(fieldName);

            //Calculer la largeur
            var fieldWidth = fieldPosition[3] - fieldPosition[1];
            var pourcentageDepassement = (Math.Ceiling(currentTextSize) * 100 / (fieldWidth - (champTexteBidon.BorderWidth * 2))) - 100;
            return pourcentageDepassement;
        }
    }
}
