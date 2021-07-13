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

namespace FRW.SV.GestionFormulaires.SN.Pdf
{
    /// <summary>
    /// Classe de production de PDF avec gestion des données trop longues.
    /// </summary>
    public class GestionPdf
    {
        /// <summary>
        /// Fusionner données au(x) Pdf(s)
        /// </summary>
        /// <inheritdoc>
        ///   -Pour chaque gabarit à remplir
        ///   -Ouvrir PDF
        ///   -Remplir (Si la value dépasse la longeur du champ, moins de 20%, diminuer taille, plus de 20%, envoyer en annexe)
        ///   -Next
        /// </inheritdoc>
        /// <param name="donneesChargement"></param>
        /// <returns></returns>
        public byte[]? FusionnerDonnees(DonneesChargement donneesChargement)
        {
            if (donneesChargement.Gabarits is null) { return null; }
            if (donneesChargement.Config is null) { return null; }

            donneesChargement.Config.TryGetValue("pdf", out var configPdf);
            if (configPdf is null) { configPdf = new Dictionary<string, string>(); }

            var refId = 1;
            var refDict = new Dictionary<string, string>();
            var gabaritsPdf = new List<(string, byte[])>();

            // Pour chaque gabarit à remplir
            foreach (var gabarit in donneesChargement.Gabarits)
            {
                if (gabarit.Proprietes is null) { continue; }
                if (gabarit.Champs is null || gabarit.Champs.Count == 0) { continue; }

                gabarit.Proprietes.TryGetValue("pdf", out var pdfName);

                var currentMemoryStream = new MemoryStream();

                PdfReader pdfReader = new PdfReader($@"C:\Users\Dany\Documents\Visual Studio 2012\Projects\TestsVue\FRW.PR.EXTRA\bin\Debug\netcoreapp3.1\mapping\3003\{pdfName}.pdf");
                PdfStamper pdfStamper = new PdfStamper(pdfReader, currentMemoryStream);
                AcroFields pdfFormFields = pdfStamper.AcroFields;

                // Police de remplacement
                var bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                pdfFormFields.AddSubstitutionFont(bf);

                foreach (var champ in gabarit.Champs)
                {
                    var champPdfType = pdfFormFields.GetFieldType(champ.Key);

                    if (champPdfType == AcroFields.FIELD_TYPE_CHECKBOX ||
                        champPdfType == AcroFields.FIELD_TYPE_RADIOBUTTON)
                    {
                        // Si la valeur prévue est vraie, on récupère la valeur vraie du
                        // champ courant (pourrait être "On", "Oui" ou autres)
                        if (bool.TryParse(champ.Value, out var setValue))
                        {
                            var trueValue = pdfFormFields.ObtenirValeurVraieChamp(champ.Key);
                            pdfFormFields.SetField(champ.Key, trueValue);
                        }
                    }
                    else
                    {
                        var pourcentageDepassement = pdfFormFields.ObtenirPourcentageDebordement(champ.Key, champ.Value);
                        var rapetisserTexteTropLong = !configPdf.TryGetValue("rapetisserTexteTropLong", out var cfgRapetisserTexteTropLong)
                                                            || bool.Parse(cfgRapetisserTexteTropLong);

                        var valeurChamp = champ.Value;

                        if (pourcentageDepassement > 0)
                        {
                            if (rapetisserTexteTropLong)
                            {
                                //Changer la police pour une police compatible "autosize"
                                pdfFormFields.SetFieldProperty(champ.Key, "textfont", bf, null);
                                //Appliquer la taille "autosize" (valeur 0)
                                pdfFormFields.SetFieldProperty(champ.Key, "textsize", 0f, null);
                            }

                            var pourcentRapetissement = (configPdf.TryGetValue("pourcentageDepassementAnnexe", out var cfgPourcentRapetissement)
                                                            ? int.Parse(cfgPourcentRapetissement) : 20);

                            if (!rapetisserTexteTropLong 
                                || pourcentageDepassement > pourcentRapetissement)
                            {
                                //On va rediriger l'info dans une page annexe, on
                                //ajoute un code de référence dans le champ texte
                                valeurChamp = $"REF{refId++}";
                                refDict.Add(valeurChamp, champ.Value);
                            }
                        }

                        pdfFormFields.SetField(champ.Key, valeurChamp);
                    }
                }
                // flatten the form to remove editting options, set it to false  
                // to leave the form open to subsequent manual edits  
                pdfStamper.FormFlattening = !configPdf.TryGetValue("verrouillerChampsPdf", out var verrouillerPdf)
                                                || bool.Parse(verrouillerPdf);

                pdfStamper.Close();

                gabaritsPdf.Add(("application/pdf", currentMemoryStream.ToArray()));
            }

            // Si nous avons des références à mettre en annexe
            // on va produire l'annexe
            if (refDict.Count > 0)
            {
                gabaritsPdf.Add(("application/pdf", GenererPageAnnexe(refDict)));
            }

            var final = itextSharpPdfMerger.Create(gabaritsPdf);

            //Retourner le document complet, en byte array
            return final;
        }

        /// <summary>
        /// Produire l'annexe
        /// </summary>
        private byte[] GenererPageAnnexe(Dictionary<string, string> refDict)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.Letter);
                var writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();
                var tr = string.Empty;

                foreach (var item in refDict)
                {
                    tr += $"<tr> " +
                          $" <td scope=\"row\">{item.Key}</td> " +
                          $" <td colspan=\"5\">{item.Value}</td> " +
                          $"</tr>";
                }

                var html = $"<h1>Annexe des valeurs trop longues</h1><br>" +
                    $"<table style=\"repeat-header:yes; \" width=\"100%\">" +
                    $"<tr> " +
                    $" <th scope=\"col\" style=\"color: white; vertical-align: middle;\" bgcolor=\"#3f87a6\">Référence</th> " +
                    $" <th scope=\"col\" style=\"color: white;\" bgcolor=\"#3f87a6\" colspan=\"5\">Valeur</th> " +
                    $"</tr> " +
                    $"{tr} " +
                    $"</table> ";

                StringReader sr = new StringReader(html);
                StyleSheet styles = new StyleSheet();

                var objects = HtmlWorker.ParseToList(sr, styles);

                foreach (IElement? element in objects)
                {
                    doc.Add(element);
                }

                doc.Close();
                bytes = ms.ToArray();

                return bytes;
            }
        }
    }
}
