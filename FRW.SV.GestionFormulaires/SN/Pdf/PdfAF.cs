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
    public class GestionPdf
    {
        /// <summary>
        /// 
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

            _ = donneesChargement.Config.TryGetValue("pdf", out var configPdf);

            if (configPdf is null) { configPdf = new Dictionary<string, string>(); }

            var refDict = new Dictionary<string, string>();
            var refId = 1;

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

                var bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                pdfFormFields.AddSubstitutionFont(bf);

                foreach (var champ in gabarit.Champs)
                {
                    var textField = new TextField(null, null, null);
                    var valueName = pdfFormFields.GetFieldItem(champ.Key);
                    var fieldType = pdfFormFields.GetFieldType(champ.Key);

                    if (fieldType == AcroFields.FIELD_TYPE_CHECKBOX ||
                        fieldType == AcroFields.FIELD_TYPE_RADIOBUTTON)
                    {
                        // Si la valeur prévue est vraie, on récupère la valeur vrai du champ courrant (pourrait être "On", "Oui" ou autres)
                        if (bool.TryParse(champ.Value, out var setValue))
                        {
                            var trueValue = string.Empty;
                            foreach (var checkval in pdfFormFields.GetAppearanceStates(champ.Key))
                            {
                                if (checkval != "Off")
                                {
                                    trueValue = checkval;
                                    break;
                                }
                            }
                            pdfFormFields.SetField(champ.Key, trueValue);
                        }
                    }
                    else
                    {
                        pdfFormFields.DecodeGenericDictionary(valueName.GetMerged(0), textField);

                        var texteTest = new Font(textField.Font, textField.FontSize);
                        var currentTextSize = texteTest.GetCalculatedBaseFont(true)
                                                        .GetWidthPointKerned(champ.Value, texteTest.CalculatedSize);

                        var fieldPosition = pdfFormFields.GetFieldPositions(champ.Key);
                        var pourcentageDepassement = ObtenirPourcentageDebordement(fieldPosition, currentTextSize, textField.BorderWidth);

                        var valeurChamp = champ.Value;

                        if (pourcentageDepassement > 0)
                        {
                            //Changer la police pour une police compatible "autosize"
                            pdfFormFields.SetFieldProperty(champ.Key, "textfont", bf, null);
                            //Appliquer la taille "autosize" (valeur 0)
                            pdfFormFields.SetFieldProperty(champ.Key, "textsize", 0f, null);

                            if (pourcentageDepassement > (configPdf.TryGetValue("pourcentRapetissement", out var pourcentRapetissement)
                                      ? int.Parse(pourcentRapetissement) : 20))
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
                pdfStamper.FormFlattening = configPdf.TryGetValue("verrouillerChampsPdf", out var verrouillerPdf)
                                                ? bool.Parse(verrouillerPdf) : true;
                // close the pdf  
                pdfStamper.Close();

                gabaritsPdf.Add(("application/pdf", currentMemoryStream.ToArray()));
            }

            if (refDict.Count > 0)
            {
                gabaritsPdf.Add(("application/pdf", GenererPageAnnexe(refDict)));
            }

            var final = itextSharpPdfMerger.Create(gabaritsPdf);

            //Retourner le document complet, en byte array
            return final;
        }

        private double ObtenirPourcentageDebordement(float[] fieldPosition, float currentTextSize, float borderWidth)
        {
            //Calculer la largeur
            var fieldWidth = fieldPosition[3] - fieldPosition[1];
            var pourcentageDepassement = (Math.Ceiling(currentTextSize) * 100 / (fieldWidth - (borderWidth * 2))) - 100;
            return pourcentageDepassement;
        }

        private byte[] GenererPageAnnexe(Dictionary<string, string> refDict)
        {
            //Create a byte array that will eventually hold our final PDF
            Byte[] bytes;

            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                var doc = new Document(PageSize.Letter);
                //{

                //Create a writer that's bound to our PDF abstraction and our stream
                var writer = PdfWriter.GetInstance(doc, ms);
                //{
                //Open the document for writing
                doc.Open();
                var roll = string.Empty;
                /*for (int i = 0; i < 100; i++)
                 {*/
                foreach (var item in refDict)
                {
                    roll += $"<tr> <td scope=\"row\">{item.Key}</td> <td colspan=\"5\">{item.Value}</td> </tr>";
                }
                /*}*/

                //Our sample HTML and CSS
                var example_html = $"<h1>Annexe des valeurs trop longues</h1><br>" +
                    $"<table style=\"repeat-header:yes; \" width=\"100%\">" +
                    $"<tr> <th scope=\"col\" style=\"color: white; vertical-align: middle;\" bgcolor=\"#3f87a6\">Référence</th> <th scope=\"col\" style=\"color: white;\" bgcolor=\"#3f87a6\" colspan=\"5\">Valeur</th> </tr> " +
                    $"{roll} " +
                    $"</table> ";

                StringReader sr = new StringReader(example_html);
                StyleSheet styles = new StyleSheet();

                var objects = HtmlWorker.ParseToList(sr, styles);

                foreach (IElement element in objects)
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
