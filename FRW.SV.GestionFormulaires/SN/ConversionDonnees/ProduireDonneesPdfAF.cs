using FRW.TR.Commun;
using System.Collections.Generic;
using FRW.TR.Contrats.Assignateur;
using Newtonsoft.Json;
using FRW.TR.Commun.Utils;
using SmartFormat;
using FRW.TR.Contrats.ConversionDonnees;

namespace FRW.SV.GestionFormulaires.SN.ConversionDonnees
{
    /// <summary>
    /// Convertir Données formulaire vers autre structure.
    /// </summary>
    public class ConvertirDonneesAF
    {
        private readonly FormConfig _obtenirConfiguration;

        public ConvertirDonneesAF(FormConfig obtenirConfiguration)
        {
            _obtenirConfiguration = obtenirConfiguration;
        }

        /// <summary>
        /// Traiter la conversion d'un modèle VueFormulate vers une cible externe, soit un autre format.
        /// </summary>
        /// <param name="typeFormulaire">Ex: 3003</param>
        /// <param name="jsonData">Données du formulaire.</param>
        /// <remarks>
        /// -Lire le fichier de binding
        /// -Pour chaque gabarit
        /// -pour chaque champ dans la config
        /// -lire la source définie
        /// -ajouter les infos dans un objet "gabaritX" qui contient tous les champs ayant une valeur à mapper dans le pdf
        /// -retourner le tableau d'objet de mappage (une map par gabarit pdf)
        /// </remarks>
        /// <returns></returns>
        public DonneesChargement Convertir(string typeFormulaire, string jsonData)
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

            var sortie = new DonneesChargement
            {
                Config = cfg.Config,
                Gabarits = new List<Template>()
            };

            if (cfg.Templates is { })
            {
                // pour chaque gabarit
                foreach (var template in cfg.Templates)
                {
                    if (!string.IsNullOrWhiteSpace(template["condition"]))
                    {
                        var result = ExecuterFormule(modeleDonnees, template["condition"]);
                        // Si la formule retourne un true dans le texte on continue.
                        // Ex: plusieurs formules pourraient produire : truefalsefalse comme retour.
                        if (!result.Contains("true"))
                        {
                            // Ne pas continuer l'exécution du module
                            continue;
                        }
                    }

                    var gabarit = new Dictionary<string, string>();
                    foreach (var bind in cfg.Bind![template["id"]])
                    {
                        string? valeur = null;

                        // Mode formule, ici on lit avec SmartFormat
                        if (bind.Value.Formule is { } && bind.Value.Champs is { })
                        {
                            valeur = ExecuterFormule(modeleDonnees, bind.Value.Formule);
                        }
                        else // Sinon on va simplement évaluer la value
                        {
                            var formule = string.Empty;

                            if (bind.Value.Champs is { })
                            {
                                foreach (var champ in bind.Value.Champs)
                                {
                                    if (champ.Contains("=="))
                                    {
                                        var keyVal = champ.Split("==");
                                        formule += "{" + keyVal[0] + ":include(" + keyVal[1] + "):true}";
                                    }
                                    else
                                    {
                                        if (!champ.StartsWith('<') && !champ.EndsWith('>'))
                                        {
                                            formule += "{" + champ + "}";
                                        }
                                    }
                                }
                            }

                            if (string.IsNullOrWhiteSpace(formule))
                            {
                                valeur = ExecuterFormule(modeleDonnees, formule);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(valeur))
                        {
                            gabarit.Add(bind.Key, valeur);
                        }
                    }

                    sortie.Gabarits.Add(new Template()
                    {
                        Proprietes = template,
                        Champs = gabarit
                    });
                }
            }

            return sortie;
        }

        /// <summary>
        /// Executer Smart.Format si la source est non null.
        /// </summary>
        /// <param name="source">Dictionnaire de données</param>
        /// <param name="formule">Formule Smart.Format à exécuter</param>
        /// <returns></returns>
        public static string ExecuterFormule(IDictionary<object, object>? source,
                                             string formule)
        {
            if (source is null) return string.Empty;

            return Smart.Format(formule, source);
        }
    }
}
