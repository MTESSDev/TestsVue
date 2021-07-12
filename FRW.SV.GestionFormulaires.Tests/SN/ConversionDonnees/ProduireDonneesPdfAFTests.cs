using Microsoft.VisualStudio.TestTools.UnitTesting;
using FRW.SV.GestionFormulaires.SN.ConversionDonnees;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using FRW.TR.Commun.Utils;

namespace FRW.SV.GestionFormulaires.SN.ConversionDonnees.Tests
{
    [TestClass()]
    public class ProduireDonneesPdfAFTests
    {

        [TestMethod()]
        public void ListerElementsDunArray()
        {
            // Data du formulaire client
            var data = LireJson("..\\..\\..\\cas_de_tests\\ListerElementsDunArray.json");

            // Executer la formule
            var result = ConvertirDonneesAF.ExecuterFormule(data,
                             "{inscription.0.cours:list|, | et }");

            //Vérifier le résultat
            Assert.AreEqual("Yoga, Tennis et Soccer", result);
        }

        [TestMethod()]
        public void RecupererUnElementArray()
        {
            // Data du formulaire client
            var data = LireJson("..\\..\\..\\cas_de_tests\\ListerElementsDunArray.json");

            // Executer la formule
            var result = ConvertirDonneesAF.ExecuterFormule(data,
                                    "{inscription.0.cours.1}");

            //Vérifier le résultat
            Assert.AreEqual("Tennis", result);
        }

        [TestMethod()]
        public void ModeleComplet()
        {
            // Data du formulaire client
            var data = LireJson("..\\..\\..\\cas_de_tests\\ListerElementsDunArray.json");

            // Executer la formule
            var result = ConvertirDonneesAF.ExecuterFormule(data,
                                  "{inscription.0.cours.1}");

            //Vérifier le résultat
            Assert.AreEqual("Tennis", result);
        }

        [TestMethod()]
        public void TestDate()
        {
            // Data du formulaire client
            var data = LireJson("..\\..\\..\\cas_de_tests\\ListerElementsDunArray.json");

            // Executer la formule
            var result = ConvertirDonneesAF.ExecuterFormule(data,
                                                              "{date:MMMM d}");

            //Vérifier le résultat
            Assert.AreEqual("avril 23", result);
        }

        private static IDictionary<object, object> LireJson(string path)
        {
            //On utilise newtonsoft parce qu'il est le seul à pouvoir faire ce que nous voulons
            //System.Text.Json n'est pas à la hauteur.
            var jsonData = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<IDictionary<object, object>>(
                                     jsonData,
                                         new JsonConverter[] {
                                                new ConvertisseurFRW() }
                                     );
        }
    }
}