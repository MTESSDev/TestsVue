/*! CAC.ValidationsClient */
$.validator.setDefaults({
    ignore: [],
    onkeyup: false,
    onfocusout: false,

    //Personnalisation du sommaire des erreurs et appel à la fonction par défaut pour les erreurs inline.
    showErrors: function (errorMap, errorList) {

        var container = $(this.currentForm).find("[data-valmsg-summary=true]");
        var listeErreurs = container.find("ul");

        if (listeErreurs && listeErreurs.length && errorList.length) {
            listeErreurs.empty();
            container.addClass("validation-summary-errors").removeClass("validation-summary-valid").attr("tabindex", "0");

            //MAJ du contenu de la liste des erreurs (texte du message)
            var validator = this;

            $.each(errorList, function () {
                html = '<li><a href="#" onclick="donnerFocusChampErreur(\'' + this.element.id + '\'); return false;">' + this.message + '</a></li>';
                $(html).appendTo(listeErreurs);

                validator.showLabel(this.element, this.message);
            });

        }

        //Fonction par défaut pour les erreurs inline
        this.defaultShowErrors();
    }
});


//$.validator.setDefaults({
//    ignore: [],
//    onkeyup: false,
//    onfocusout: false,

//    //Personnalisation du sommaire des erreurs et appel à la fonction par défaut pour les erreurs inline.
//    showErrors: function (errorMap, errorList) {

//        var texteEnteteErreurs = obtenirTexteEdite('titreCommunErreursFormulaire');

//        var container = $(this.currentForm).find("[data-valmsg-summary=true]");
//        var listeErreurs = container.find("ul");

//        if (listeErreurs && listeErreurs.length && errorList.length) {
//            listeErreurs.empty();
//            container.addClass("validation-summary-errors").removeClass("validation-summary-valid").attr("tabindex", "0");

//            if (container.find(".entete").length == 0) {
//                var htmlEnteteErreurs = '<div class="d-flex align-items-center entete">' +
//                    '<span class="icone" aria-hidden="true"></span>' +
//                    '<h2 class="titre">' + texteEnteteErreurs + '</h2></div>';

//                container.prepend(htmlEnteteErreurs);
//            }

//            var validator = this;

//            $.each(errorList, function () {
//                html = '<li><a href="#" onclick="donnerFocusChampErreur(\'' + this.element.id + '\'); return false;">' + this.message + '</a></li>';
//                $(html).appendTo(listeErreurs);

//                validator.showLabel(this.element, this.message);
//            });

//        }

//        //Fonction par défaut pour les erreurs inline
//        this.defaultShowErrors();
//    }
//});





$(function () {

    /*
    Section override du summary
    */
    /*
   
    var validator = $('form').data('validator');
    validator.settings.errorPlacement = function (error, element) {
    // do your custom error placement
    };



    function onErrors(error, inputElement) {
    var container = $(this).find("[data-valmsg-summary=true]"),
    list = container.find("ul");

    if (list && list.length && validator.errorList.length) {
    list.empty();
    container.addClass("validation-summary-errors").removeClass("validation-summary-valid");

    $.each(validator.errorList, function () {
    //$("<li />").html(this.message).appendTo(list);
    $("<li />").append($("<label />").attr("for", this.element.id).html(this.message)).appendTo(list);
    });
    }
    }
    */

    /*
    Fin section override du summary
    */

    var originalMethods = {
        date: $.validator.methods.date,
        number: $.validator.methods.number
    };

    function isDate(txtDate) {
        var currVal = txtDate;
        if (currVal == '')
            return false;

        var rxDatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})$/; //Declare Regex
        var dtArray = currVal.match(rxDatePattern); // is format OK?

        if (dtArray == null)
            return false;

        //Checks for mm/dd/yyyy format.
        dtMonth = dtArray[3];
        dtDay = dtArray[5];
        dtYear = dtArray[1];

        if (dtMonth < 1 || dtMonth > 12)
            return false;
        else if (dtDay < 1 || dtDay > 31)
            return false;
        else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
            return false;
        else if (dtMonth == 2) {
            var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
            if (dtDay > 29 || (dtDay == 29 && !isleap))
                return false;
        }
        return true;
    }


    //override de la méthode de base
    $.validator.methods.date = function (value, element) {
        /*
        if (Modernizr.inputtypes.date) {
        return originalMethods.date.call(this, value, element);
        }
        */

        if (this.optional(element))
            return true;

        return isDate(value);
    }
    //
    //Override de la fonction formatAndAdd 
    //
    $.validator.prototype.focusInvalid = function () {

        var container = $(".validation-summary-errors")[0];
        if (container) {
            container.focus();
        }

        /*container.trigger("focusin");*/
        /*
        if (this.settings.focusInvalid) {
            try {
                $(this.findLastActive() || this.errorList.length && this.errorList[0].element || [])
					        .filter(":visible")
					        .focus()
                // manually trigger focusin event; without it, focusin handler isn't called, findLastActive won't have anything to find
					        .trigger("focusin");
            } catch (e) {
                // ignore IE throwing errors when focusing hidden elements
            }
        }*/
    }
    //
    //Override de la fonction formatAndAdd 
    //
    $.validator.prototype.formatAndAdd = function (element, rule) {

        var message = this.defaultMessage(element, rule.method),
            theregex = /\$?\{(\d+)\}/g;
        if (typeof message === "function") {
            message = message.call(this, rule.parameters, element);
        } else if (theregex.test(message)) {
            message = $.validator.format(message.replace(theregex, "{$1}"), rule.parameters);
        }

        /* CUSTOM */

        if (message.indexOf("{Occurence}") != -1) {

            var groupe = element.name.split(".");
            var control = $("input[name='" + groupe[0] + ".Occurence']");
            var occurence = control.val();

            message = message.replace("{Occurence}", occurence);

        } else if (message.indexOf("{OccurenceTexte}") != -1) {

            var groupe = element.name.split(".");
            var control = $("input[name='" + groupe[0] + ".OccurenceTexte']");
            var occurence = control.val();

            message = message.replace("{OccurenceTexte}", occurence);

        } else {
            message = message;

        }

        /* FIN CUSTOM */

        this.errorList.push({
            message: message,
            element: element,
            method: rule.method
        });

        this.errorMap[element.name] = message;
        this.submitted[element.name] = message;
    }

});

$(function () {

    function endsWith(str, suffix) {
        str = str.toString();
        return str.indexOf(suffix, str.length - suffix.length) !== -1;
    };

    //Override de la validation par défaut
    $.validator.methods.number = function (value, element) {
        //Ici on triche pour éviter que le client ait un message d'erreur si jamais il tape un nombre et tape le point ou la virgule
        if (endsWith(value, ".") || endsWith(value, ",")) {
            value = value + "0";
        }
        if ($(element).attr("data-type") != undefined && $(element).attr("data-type") == "entier") {
            return this.optional(element) || (new RegExp("^[0-9]+?$", "g")).test(value);
        } else if ($(element).attr("pattern") != undefined && $(element).attr("pattern") != "") {
            return this.optional(element) || (new RegExp("^" + $(element).attr("pattern") + "$", "g")).test(value);
        } else {
            return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(value);
        }
    }
});

$(function () {
    $.validator.methods.range = function (value, element, param) {
        var tmp = value.replace(",", ".");
        return this.optional(element) || (tmp >= param[0] && tmp <= param[1]);
    }
});


///////////////////////////////////////////////////////////////////////////////////////////////
// requissi validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('requissi',
    function (value, element, parameters) {
        var id = parameters['dependentproperty'];
        // get the target value (as a string, 
        // as that's what actual value will be)
        var targetvalue = parameters['targetvalue'];


        // get the actual value of the target control
        // note - this probably needs to cater for more 
        // control types, e.g. radios
        var control = $("input[name='" + id + "']");
        var controltype = control.attr('type');
        var actualvalue =
            controltype === 'checkbox' ?
                control.attr('checked') ? "true" : "false" :
                controltype === 'radio' ? control.filter(':checked').val() : control.val();

        if (targetvalue == "") {

            if (actualvalue != null && actualvalue != "" && actualvalue != undefined)
                return $.validator.methods.required.call(this, value, element, parameters);

        } else {

            targetvalue = (targetvalue == null ? '' : targetvalue).toString();
            var targetvaluearray = targetvalue.split('|');

            for (var i = 0; i < targetvaluearray.length; i++) {

                // if the condition is true, reuse the existing 
                // required field validator functionality
                if (actualvalue != undefined && targetvaluearray[i].toLowerCase() === actualvalue.toLowerCase()) {
                    return $.validator.methods.required.call(this, value, element, parameters);
                }
            }
        }


        return true;
    }
);

$.validator.unobtrusive.adapters.add(
    'requissi',
    ['dependentproperty', 'targetvalue'],
    function (options) {
        options.rules['requissi'] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['requissi'] = options.message;
    });

///////////////////////////////////////////////////////////////////////////////////////////////
// validerdateretraitapresdatecourante validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('ValiderDateRetraitApresDateCourante',
    function (value, element, params) {
        if (value == null || value == "" || value == undefined) {
            return true;
        }
        else if (value <= $('#dateCACContext').val()) {

            return false;

        }

        return true;
    }
);
$.validator.unobtrusive.adapters.add("ValiderDateRetraitApresDateCourante", [], function (options) {
    options.rules['ValiderDateRetraitApresDateCourante'] = {};
    options.messages['ValiderDateRetraitApresDateCourante'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// champincomplet validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('champincomplet',
    function (value, element, params) {
        if (element.validity.badInput) {
            return false;
        } else {
            return true;
        }
    }
);

$.validator.unobtrusive.adapters.add("champincomplet", [], function (options) {
    options.rules['champincomplet'] = {};
    options.messages['champincomplet'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// champrequis validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('requisdate',
    function (value, element, params) {
        return value != "";
    }
);

$.validator.unobtrusive.adapters.add("requisdate", [], function (options) {
    options.rules['requisdate'] = {};
    options.messages['requisdate'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// requishtml validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('requishtml',
    function (value, element, params) {
        var texteSansHtml = value.replace(/<\/?[^>]+(>|$)/g, "");
        return texteSansHtml.trim() != "";
    }
);

$.validator.unobtrusive.adapters.add("requishtml", [], function (options) {
    options.rules['requishtml'] = {};
    options.messages['requishtml'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// validerdateinactivesipublierattribute validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('validerdatenullesipublier',
    function (value, element, params) {
        if ($('#EnPublication').val() == "true") {
            if (value == null || value == undefined || value == "") {
                return false;
            }
        }

        return true;
    }
);

$.validator.unobtrusive.adapters.add("validerdatenullesipublier", [], function (options) {
    options.rules['validerdatenullesipublier'] = {};
    options.messages['validerdatenullesipublier'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// validerdatediPublier validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('validerdateplusgrandquedatejourSipublier',
    function (value, element, params) {
        if ($('#EnPublication').val() == "true") {
            if (value.toString() < $('#dateCACContext').val()) {
                return false;
            }
        }

        return true;
    }
);

$.validator.unobtrusive.adapters.add("validerdateplusgrandquedatejourSipublier", [], function (options) {
    options.rules['validerdateplusgrandquedatejourSipublier'] = {};
    options.messages['validerdateplusgrandquedatejourSipublier'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// validerdatesiconditionpubliee validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('validerdatesiconditionpubliee',
    function (value, element, params) {
        if ($('#IndicateurPublication').val() == "True" && $('#Etat').val() == "À venir") {
            if (value.toString() < $('#dateCACContext').val()) {
                return false;
            }
        }

        return true;
    }
);

$.validator.unobtrusive.adapters.add("validerdatesiconditionpubliee", [], function (options) {
    options.rules['validerdatesiconditionpubliee'] = {};
    options.messages['validerdatesiconditionpubliee'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// validersiconditionexiste validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('validersiconditionexiste',
    function (value, element, params) {
        var table = $('#TableauConditionsUtilisation').DataTable();
        var valide = true;
        if ($('#IndicateurPublication').val() == "True" || $('#EnPublication').val() == "true") {
            table.rows().every(function () {
                if (value.toString() == this.data()[2] && this.id() != table.rows('.selected').ids()[0] && this.data()[4] == "True") {
                    valide = false;
                }
            });
        }

        return valide;
    }
);

$.validator.unobtrusive.adapters.add("validersiconditionexiste", [], function (options) {
    options.rules['validersiconditionexiste'] = {};
    options.messages['validersiconditionexiste'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// validerdatesiConditionpublieeestinactive validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('validerdatesiConditionpublieeestinactive',
    function (value, element, params) {
        if ($('#IndicateurPublication').val() == "True" && value != element.defaultValue) {
            if (element.defaultValue <= $('#dateCACContext').val() || value == null || value == undefined) {
                return false;
            }
        }

        return true;
    }
);

$.validator.unobtrusive.adapters.add("validerdatesiConditionpublieeestinactive", [], function (options) {
    options.rules['validerdatesiConditionpublieeestinactive'] = {};
    options.messages['validerdatesiConditionpublieeestinactive'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// validerformatjson validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('validerformatjson',
    function (value, element, params) {
        try {
            JSON.parse(value);
        } catch (e) {
            return false;
        }
        return true;
    }
);

$.validator.unobtrusive.adapters.add("validerformatjson", [], function (options) {
    options.rules['validerformatjson'] = {};
    options.messages['validerformatjson'] = options.message;
});



///////////////////////////////////////////////////////////////////////////////////////////////
// groupe validateur
///////////////////////////////////////////////////////////////////////////////////////////////


$.validator.addMethod('validationgroupe',
    function (value, element, parameters) {

        var nomGroupe = parameters['nom'];


        var props = parameters['props'];
        props = (props == null ? '' : props).toString();

        var propsarray = props.split('|');

        for (var i = 0; i < propsarray.length; i++) {

            // get the actual value of the target control
            // note - this probably needs to cater for more 
            // control types, e.g. radios
            var control = $('#' + propsarray[i]);
            var controltype = control.attr('type');
            var actualvalue =
                controltype === 'checkbox' ?
                    control.attr('checked') ? "true" : "false" :
                    control.val();

            //var test = $.validator.methods.requissi.call(this, value, control[0], parameters);

            if ($.validator.methods.required.call(this, value, control[0], parameters)) {
                return true;
            }
        }

        return false;
    }
);

$.validator.unobtrusive.adapters.add(
    'validationgroupe',
    ['nom', 'typevalidation', 'props', 'dependentproperty', 'targetvalue'],
    function (options) {
        options.rules['validationgroupe'] = {
            nom: options.params['nom'],
            typevalidation: options.params['typevalidation'],
            props: options.params['props'],
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['validationgroupe'] = options.message;
    });


///////////////////////////////////////////////////////////////////////////////////////////////
// comparaisonvaleur validateur
///////////////////////////////////////////////////////////////////////////////////////////////


//Ajout des adapteurs en boucle
$.each(["PlusPetitQue", "PlusPetitEgalA", "EgaleA", "PlusGrandQue", "PlusGrandEgaleA"], function (index, regle) {


    $.validator.addMethod('comparaisonvaleur' + regle.toLowerCase(),
        function (value, element, parameters) {

            var id = parameters['dependentproperty'];
            var actualvalue = null;
            if (id !== undefined) {
                var control = $("input[name$='" + id + "']");
                actualvalue = control.val();
            } else {
                actualvalue = parameters['valeur'];
            }

            var typeComparaison = parameters['typecomparaison'];
            var ignoreCasse = parameters['ignorercasse'];

            if (value == "" || value == undefined || actualvalue == "" || actualvalue == undefined)
                return true;

            if (parameters['typeobjet'] == "DateTime") {

                // Champ à valider
                var champHeure = $("#" + $(element).attr('id') + "_Heure");
                var heure = champHeure.val()
                if (heure != "" && heure != undefined) {
                    value += ' ' + heure;
                }

                // Champ de référence
                if (id !== undefined) {
                    var champHeure = $("#" + id + "_Heure");
                    var heure = champHeure.val()
                    if (heure != "" && heure != undefined) {
                        actualvalue += ' ' + heure;
                    }
                }
            }

            var valide = true;

            switch (parameters['typeobjet']) {
                case "Date":
                case "String":
                    if (ignoreCasse === 'true') {
                        value = value.toLowerCase();
                        actualvalue = actualvalue.toLowerCase();
                    }
                    break;
                case "DateTime":
                    var yy = actualvalue.split('-')[0];
                    var mm = actualvalue.split('-')[1];
                    var dd = actualvalue.split('-')[2];
                    if (dd.length > 2) {
                        dd = dd.substring(0, 2);
                        var HH = actualvalue.substring(11, 13);
                        var MM = actualvalue.substring(14, 16);
                    } else {
                        var HH = 0;
                        var MM = 0;
                    }

                    mm = Number(mm) - 1;

                    actualvalue = new Date(yy, mm, dd, HH, MM, 0, 0);

                    var yy = value.split('-')[0];
                    var mm = value.split('-')[1];
                    mm = Number(mm) - 1;
                    var dd = value.split('-')[2];
                    if (dd.length > 2) {
                        dd = dd.substring(0, 2);
                        var HH = value.substring(11, 13);
                        var MM = value.substring(14, 16);
                    } else {
                        var HH = 0;
                        var MM = 0;
                    }

                    value = new Date(yy, mm, dd, HH, MM, 0, 0);
                    break;

                case "Numerique":

                    actualvalue = parseFloat(actualvalue.replace(",", "."));
                    break;

                default:
                    alert("typeobjet " + parameters['typeobjet'] + " invalide.");
            }


            switch (typeComparaison) {
                case "PlusPetitQue":
                    valide = value < actualvalue;
                    break;
                case "PlusPetitEgalA":
                    valide = value <= actualvalue;
                    break;
                case "EgaleA":
                    valide = value == actualvalue;
                    break;
                case "PlusGrandQue":
                    valide = value > actualvalue;
                    break;
                case "PlusGrandEgaleA":
                    valide = value >= actualvalue;
                    break;
            }

            return valide;
        }
    );


    $.validator.unobtrusive.adapters.add(
        'comparaisonvaleur-' + regle.toLowerCase(),
        ['dependentproperty', 'typecomparaison', 'valeur', 'typeobjet', 'ignorercasse'],
        function (options) {

            options.rules['comparaisonvaleur' + regle.toLowerCase()] = {
                dependentproperty: options.params['dependentproperty'],
                valeur: options.params['valeur'],
                typeobjet: options.params['typeobjet'],
                typecomparaison: regle,
                ignorercasse: options.params['ignorercasse']
            };
            var message = "";

            if (options.message.indexOf("{Occurence}") != -1) {

                var groupe = options.element.name.split(".");
                var control = $("input[name='" + groupe[0] + ".Occurence']");
                var occurence = control.val();

                message = options.message.replace("{Occurence}", occurence);

            } else if (options.message.indexOf("{OccurenceTexte}") != -1) {

                var groupe = options.element.name.split(".");
                var control = $("input[name='" + groupe[0] + ".OccurenceTexte']");
                var occurence = control.val();

                message = options.message.replace("{OccurenceTexte}", occurence);

            } else {
                message = options.message;

            }

            options.messages['comparaisonvaleur' + regle.toLowerCase()] = message;

        });

});

///////////////////////////////////////////////////////////////////////////////////////////////
// dateproduction validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('dateproduction',
    function (value, element, params) {

        if (value == "" || value == undefined)
            return true;

        // Champ à valider
        var champHeure = $("#" + $(element).attr('id') + "_Heure");
        if (champHeure.length > 0) {
            var heure = champHeure.val()
            if (heure != "" && heure != undefined) {
                value += ' ' + heure;
            }
        }

        var yy = value.split('-')[0];
        var mm = value.split('-')[1];
        var dd = value.split('-')[2];
        if (dd.length > 2) {
            dd = dd.substring(0, 2);
            var HH = value.substring(11, 13);
            var MM = value.substring(14, 16);
        } else {
            var HH = 0;
            var MM = 0;
        }

        mm = Number(mm) - 1;

        value = new Date(yy, mm, dd, HH, MM, 0, 0);

        var valide = true;

        switch (params['typecomparaison']) {
            case "PlusPetitQue":
                valide = value < DATE_PRODUCTION;
                break;
            case "PlusPetitEgalA":
                valide = value <= DATE_PRODUCTION;
                break;
            case "EgaleA":
                valide = value == DATE_PRODUCTION;
                break;
            case "PlusGrandQue":
                valide = value > DATE_PRODUCTION;
                break;
            case "PlusGrandEgaleA":
                valide = value >= DATE_PRODUCTION;
                break;
        }

        return valide;
    }
);

$.validator.unobtrusive.adapters.add("dateproduction", ['typecomparaison'], function (options) {
    options.rules['dateproduction'] = { typecomparaison: options.params['typecomparaison'] };
    options.messages['dateproduction'] = options.message;
});

///////////////////////////////////////////////////////////////////////////////////////////////
// doitetrevrai validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('doitetrevrai',
    function (value) {

        if (value == "" || value == undefined)
            return false;

        return value.toLowerCase() == "true";
    }
);

$.validator.unobtrusive.adapters.add("doitetrevrai", [], function (options) {
    options.rules['doitetrevrai'] = {};
    options.messages['doitetrevrai'] = options.message;
});


///////////////////////////////////////////////////////////////////////////////////////////////
// mutuellementobligatoire validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('mutuellementobligatoire',
    function (value, element, parameters) {
        var id = parameters['proprietedependante'];

        var champ = $("[name='" + id + "']");
        var typeChamp = champ.attr('type');
        var autreValeur =
            typeChamp === 'checkbox' ?
                champ.attr('checked') ? "true" : "false" :
                typeChamp === 'radio' ? champ.filter(':checked').val() : champ.val();

        if (autreValeur != undefined && autreValeur.trim() != '') {
            return $.validator.methods.required.call(this, value, element, parameters);
        }

        return true;
    }
);

$.validator.unobtrusive.adapters.add(
    'mutuellementobligatoire',
    ['proprietedependante'],
    function (options) {
        options.rules['mutuellementobligatoire'] = {
            proprietedependante: options.params['proprietedependante']
        };
        options.messages['mutuellementobligatoire'] = options.message;
    });

///////////////////////////////////////////////////////////////////////////////////////////////
// nbmaximal validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('nbmaximal',
    function (value, element, parameters) {
        var nb = parameters['nb'];
        return fichiers[element.id].length <= nb;
    }
);

$.validator.unobtrusive.adapters.add(
    'nbmaximal',
    ['nb'],
    function (options) {
        options.rules['nbmaximal'] = {
            nb: options.params['nb']
        };
        options.messages['nbmaximal'] = options.message;
    });

///////////////////////////////////////////////////////////////////////////////////////////////
// format validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('format',
    function (value, element, parameters) {
        var extensions = new RegExp('(' + parameters['extensions'].toString().replace(/\./g, '\\.') + ')$', "i");

        for (var i = 0; i < fichiers[element.id].length; i++) {
            if (!extensions.test(fichiers[element.id][i].name)) {
                return false;
            }
        }

        return true;
    }
);

$.validator.unobtrusive.adapters.add(
    'format',
    ['extensions'],
    function (options) {
        options.rules['format'] = {
            extensions: options.params['extensions']
        };
        options.messages['format'] = options.message;
    });

///////////////////////////////////////////////////////////////////////////////////////////////
// format validateur
///////////////////////////////////////////////////////////////////////////////////////////////

$.validator.addMethod('taillemaximal',
    function (value, element, parameters) {
        var taillemaximal = parameters['mb'];

        for (var i = 0; i < fichiers[element.id].length; i++) {
            if (fichiers[element.id][i].size > (taillemaximal * 1024 * 1024)) {
                return false;
            }
        }

        return true;
    }
);

$.validator.unobtrusive.adapters.add(
    'taillemaximal',
    ['mb'],
    function (options) {
        options.rules['taillemaximal'] = {
            mb: options.params['mb']
        };
        options.messages['taillemaximal'] = options.message;
    });