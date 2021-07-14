using FRW.PR.Extra.Model;
using FRW.PR.Extra.Pages;
using FRW.PR.Extra.Services;
using FRW.PR.Extra.Utils;
using FRW.PR.Model.Components;
using FRW.TR.Commun;
using FRW.TR.Commun.Utils;
using Jint;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FRW.PR.Extra.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SoumettreController : ControllerBase
    {
        private readonly FormulairesService _formulairesService;

        public SoumettreController(FormulairesService formService)
        {
            _formulairesService = formService;
        }

        /// <summary>
        /// FRW311 - Gérer la soumission d'un formulaire dynamique
        /// </summary>
        /// <param name="typeFormulaire">Type/identifiant de configuration/formulaire</param>
        /// <returns></returns>
        [HttpPost("{typeFormulaire}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(string typeFormulaire)
        {
            string jsonDataFromUser;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonDataFromUser = await reader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<IDictionary<object, object>>(
                                    jsonDataFromUser,
                                        new JsonConverter[] {
                                                new ConvertisseurFRW() }
                                    );

            if (data is null) { throw new Exception("No data received."); }

            //TODO: appeler le backend pour obtenir le fichier
            var dynamicForm = OutilsYaml.LireFicher<DynamicForm>(@$"schemas/{typeFormulaire}.ecsform.yml");

            var context = new ValidationContext(data, serviceProvider: null, items: null);

            var inputs = new List<ComponentValidation>();

            GetEffectiveComponents(data, dynamicForm.Form?["sectionsGroup"], ref inputs, null, null);

            foreach (var item in inputs)
            {
                if (item is { } && item.Validations != null)
                {
                    foreach (var validation in item.Validations.ToList())
                    {
                        if (!string.IsNullOrEmpty(item.Name))
                        {
                            object? val = null;

                            if (item.GroupName != null 
                                && data.TryGetValue(item.PrefixId + item.GroupName, out var groupData) 
                                && groupData is Array arrayItem)
                            {
                                if (arrayItem != null && arrayItem.Length == 1)
                                {
                                    (arrayItem.GetValue(0) as Dictionary<object, object>)?.TryGetValue(item.PrefixId + item.Name, out val);
                                }
                            }

                            if (val is null)
                            {
                                data.TryGetValue(item.PrefixId + item.Name, out val);
                            }

                            if (!validation.IsValid(val))
                            {
                                return BadRequest($"{validation.FormatErrorMessage(item.Name)}");
                            }
                        }
                    }
                }
            }
            return Ok();
        }

        /// <summary>
        /// FRW311 - Gérer la sauvegarde d'un formulaire dynamique sans validation
        /// </summary>
        /// <param name="typeFormulaire">Type/identifiant de configuration/formulaire</param>
        /// <returns></returns>
        [HttpPost("{typeFormulaire}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Sauvegarde(string typeFormulaire)
        {
            string jsonDataFromUser;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonDataFromUser = await reader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<IDictionary<object, object>>(
                                    jsonDataFromUser,
                                        new JsonConverter[] {
                                                new ConvertisseurFRW() }
                                    );

            if (data is null) { throw new Exception("No data received."); }

            //TODO: appeler le backend pour obtenir le fichier
            var dynamicForm = OutilsYaml.LireFicher<DynamicForm>(@$"schemas/{typeFormulaire}.ecsform.yml");

            var confirmation = _formulairesService.Creer(typeFormulaire);
            return Ok();
        }

        public enum ComponentType
        {
            None = 0,
            group = 1
        }

        public static void GetEffectiveComponents(IDictionary<object, object> data, object components, ref List<ComponentValidation> inputs, string? prefixId, string? groupName)
        {
            //var obj = components as IDictionary<object, object>;
            var obj2 = components as IList<object>;

            /*if (obj != null && obj.ContainsKey("sections"))
             {
                 return GetEffectiveComponents(data, obj["sections"], ref formData, null);
             }
             else */
            if (obj2 != null)
            {
                foreach (var item in obj2)
                {
                    var dictItem = item as IDictionary<object, object>;

                    if (dictItem != null)
                    {
                        if (dictItem.TryGetValue("sections", out var sections))
                        {
                            dictItem.TryGetValue("prefixId", out var currentPrefixId);
                            GetEffectiveComponents(data, sections, ref inputs, currentPrefixId?.ToString(), null);
                        }

                        if (dictItem.TryGetValue("v-if", out var vif))
                        {
                            //Run v-if
                            try
                            {
                                var options = new Options();
                                options.DebugMode();
                                options.DebuggerStatementHandling(Jint.Runtime.Debugger.DebuggerStatementHandling.Clr);

                                var result = new Engine(options)
                                    .SetValue("log", new Action<object?>(DebugWriteLineCustom))
                                    .SetValue("ArrayisArray", new Func<object?, bool>(ArrayIsArray))
                                    .SetValue("index", 0)
                                    .SetValue("name", groupName)
                                    .SetValue("form", data)
                                    //.Execute($"form.EvenementsDerniereAnnee")
                                    .Execute(@"
                                            function val(idChamp) { 
                                                var debug = '';
                                                const champs = idChamp.split('.')
                                                let objetAValider = this.form

                                                for (champ of champs) {

                                                    //TODO à modifier éventuellement pour groupes répétables
                                                    objetAValider = Array.isArray(objetAValider) ? objetAValider[0] : objetAValider

                                                    if (!objetAValider[`${champ}`] && objetAValider[`${champ}`] !== false ) {
                                                        return ''
                                                    }
                                                    objetAValider = objetAValider[`${champ}`]
                                                }              

                                                return objetAValider || ''
                                            };"
                                            + $" ({vif} ? true : false)")
                                    .GetCompletionValue()
                                    .AsBoolean();


                                /*Jint.Runtime.Debugger..Step += (sender, info) =>
                                {
                                    Console.WriteLine("{0}: Line {1}, Char {2}", info.CurrentStatement.ToString(), info.Location.Start.Line, info.Location.Start.Char);
                                };*/

                                if (!result)
                                {
                                    continue;
                                }
                            }
#pragma warning disable CS0168 // La variable est déclarée mais jamais utilisée
                            catch (Exception ex)
#pragma warning restore CS0168 // La variable est déclarée mais jamais utilisée
                            {
                                throw;
                            }

                        }

                        if (dictItem.TryGetValue("components", out var innerComponents))
                        {
                            if (dictItem.TryGetValue("type", out var componentType))
                            {
                                if (componentType != null && componentType.ToString()!.EndsWith("group", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    groupName = dictItem["name"].ToString();
                                }
                            }

                            GetEffectiveComponents(data, innerComponents, ref inputs, prefixId, groupName);
                        }
                    }

                    var inputV = new ComponentValidation();
                    inputV.ParseAttributes(dictItem);
                    inputV.GroupName = groupName;
                    inputV.PrefixId = prefixId;
                    if (inputV.Type != TypeInput.SKIP)
                    {
                        inputs.Add(inputV);
                    }
                }
            }
        }

        private static bool ArrayIsArray(object? element)
        {
            if (element is Array)
                return true;
            //if (element is Dictionary<object,object>) return true;

            return false;
        }

        private static void DebugWriteLineCustom(object? obj)
        {
            Debug.WriteLine(obj);
        }
    }
}
