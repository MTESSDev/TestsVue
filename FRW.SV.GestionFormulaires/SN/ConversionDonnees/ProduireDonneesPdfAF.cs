using FRW.TR.Commun;
using FRW.SV.GestionFormulaires.SN;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRW.TR.Contrats.Assignateur;
using Newtonsoft.Json;
using FRW.TR.Commun.Utils;
using SmartFormat;
using System.Linq;
using Microsoft.Extensions.Primitives;
using System.Dynamic;

namespace FRW.SV.GestionFormulaires.SN.ConversionDonnees
{
    public class ProduireDonneesPdfAF
    {
        private readonly ObtenirConfiguration _obtenirConfiguration;

        public ProduireDonneesPdfAF(ObtenirConfiguration obtenirConfiguration)
        {
            _obtenirConfiguration = obtenirConfiguration;
        }

        public async Task Convertir(string typeFormulaire, string jsonData)
        {
            // Lire le fichier de binding
            //var config = await _obtenirConfiguration.ObtenirFichierConfig(typeFormulaire);
            //var cfg = OutilsYaml.DeserializerString<Binder>(config);

            var cfg = OutilsYaml.LireFicher<Binder>(@"C:\Users\Dany\Documents\Visual Studio 2012\Projects\TestsVue\FRW.PR.Extra\mapping\3003\ecsbind.yml");

            var modeleDonnees = JsonConvert.DeserializeObject<IDictionary<object, object>>(
                                   jsonData,
                                       new JsonConverter[] {
                                                new ConvertisseurFRW() }
                                   );

            var sortie = new List<Dictionary<string, string>>();

            if (cfg.Templates is { })
            {
                // pour chaque gabarit
                foreach (var template in cfg.Templates)
                {
                    if (!string.IsNullOrWhiteSpace(template["condition"]))
                    {
                        //TODO: plugger SmartFormat ici
                    }

                    var gabarit = new Dictionary<string, string>();
                    foreach (var bind in cfg.Bind![template["id"]])
                    {
                        string? valeur = null;

                        // Mode formule, ici on lit avec SmartFormat
                        if (bind.Value.Formule is { } && bind.Value.Champs is { })
                        {
                            valeur = ExecuterFormule(modeleDonnees, bind.Value.Champs, bind.Value.Formule);
                        }
                        else // Sinon on va simplement évaluer la value
                        {
                            valeur = string.Join(" ", TrouverValeur(modeleDonnees, bind.Value.Champs));
                        }

                        if (!string.IsNullOrWhiteSpace(valeur))
                        {
                            gabarit.Add(bind.Key, valeur);
                        }
                    }
                    sortie.Add(gabarit);
                }
            }

            // pour chaque champ dans la config
            // lire la source définie
            // ajouter les infos dans un objet "gabaritX" qui contient tous les champs ayant une valeur à mapper dans le pdf
            // retourner le tableau d'objet de mappage (une map par gabarit pdf)
        }

        private static StringValues? TrouverValeur(IDictionary<object, object>? source,
                                                    IEnumerable<string> champsSource)
        {
            var array = new StringValues();
            foreach (var champ in champsSource)
            {
                var elements = champ.Split('.');
                array += ChargerValeur(source, elements);
            }

            return array;
        }

        private static dynamic DictionaryToObject(Dictionary<string, object> dict)
        {
            IDictionary<string, object> eo = (IDictionary<string, object>)new ExpandoObject();
            foreach (KeyValuePair<string, object> kvp in dict)
            {
                eo.Add(kvp);
            }
            return eo;
        }

        public static string ExecuterFormule(IDictionary<object, object>? source,
                                              IEnumerable<string> champsSource,
                                              string formule)
        {
            var val = TrouverValeurs(source, champsSource);
            return Smart.Format(formule, val);
        }

        public static string ExecuterFormule(IDictionary<object, object>? source,
                                      string formule)
        {
            return Smart.Format(formule, source);
        }

        private static Dictionary<string, object> TrouverValeurs(IDictionary<object, object>? source,
                                                                    IEnumerable<string> champsSource)
        {
            var array = new Dictionary<string, object>();
            int pos = 0;
            foreach (var champ in champsSource)
            {
                var elements = champ.Split('.');

                array.Add(elements.Last(), ChargerValeur(source, elements));
            }

            return array;
        }

        private static object ChargerValeur(IDictionary<object, object>? source, string[] elements)
        {
            return KeyFinder(source, elements, 0);
        }

        private static object KeyFinder(IDictionary<object, object>? source,
                                            string[] elements,
                                            int position)
        {

            var currentElements = elements[position].Split("==");
            var currentElement = currentElements[0];
            object? currentValue = null;

            if (currentElements.Length == 2)
            {
                currentValue = currentElements[1];
            }

            source.TryGetValue(currentElement, out var finded);

            /*  if (elements.Length - 1 == position && currentValue is null)
              {
                  return finded as StringValues?;
              }*/

            if (finded is Dictionary<object, object> dictionary)
            {
                throw new NotSupportedException();
                return KeyFinder(dictionary, elements, position + 1);
            }
            else if (finded is object[] array)
            {
                if (currentValue is { })
                {
                    if (array.Length == 1)
                    {
                        if (currentValue.Equals(array[0]))
                        {
                            return "true";
                        }
                        else
                        {
                            return StringValues.Empty;
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    // si on est arrivé au bout de l'objet
                    if (elements.Length - 1 == position)
                    {
                        //var strList = Array.ConvertAll(array, x => x.ToString());
                        return array;
                    }
                    else
                    {
                        var test = array[int.Parse(elements[position + 1])];
                        return KeyFinder(test as Dictionary<object, object>, elements, position + 2);
                    }
                }
            }

            return StringValues.Empty;
        }
    }
}
