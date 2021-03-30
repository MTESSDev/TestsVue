/*! siteAdditionnel */
/* Traitements spécifiques qui doivent être disponibles lorsqu'on utilise les fonctionnalités additionnelles(ex. datatables, select2).
* Il s'agit de fonctions utilitaires pour ces fonctionnaités ou d'override. Ce script doit donc être chargé APRÈS.
*/

//Traitement exécuté dès que le fichier JS est chargé.
definirParametresDefautSelect2();
definirParametresDefautDatatables();


//Traitement exécuté lorsque le rendu de l'interface est complété.
$(document).ready(function () {
    definirTraitementEnCoursSoumissionForm();
});

//Permet d'activer une fonctionnalité qui va ajouter un overlay de soumission (qui bloque toute possibilité de clic pendant la soumisssion).
function activerOverlaySoumissionForm(formulaire) {
    formulaire.data("overlaysoumission", true);
};

/** Définit les événements et le traitement requis lors de la soumission d'un formulaire. */
function definirTraitementEnCoursSoumissionForm() {

    $(document).on('click', '[type=submit]', function () {
        var form = $(this).prop('form') || $(this).closest('form')[0];
        $(form.elements).filter('[type=submit]').removeAttr('clicked');
        $(this).attr('clicked', true);
    });

    $(document).on("submit", "form", function (e) {

        $("#zoneNotificationsLecteurEcran").text(obtenirTexteEdite('txtcacheCommunMajEnCours'));

        var elementSourceSoumission = $(this.elements).filter('[clicked]');

        if (elementSourceSoumission.length) {

            //Ajouter un overlay global pendant la soumission (si activé)
            if ($(this).data("overlaysoumission")) {
                elementSourceSoumission.data("zindex", elementSourceSoumission.css("z-index")).data("position", elementSourceSoumission.css("position"));
                elementSourceSoumission.css({ "z-index": 99999, "position": "relative" });

                $("body").append("<div class='overlay-soumission-form'></div>");
            }

            //Ajouter un spinner sur l'élément à la source de la soumission (ex. un bouton) et le désactiver
            elementSourceSoumission.addClass("d-flex justify-content-center align-items-center");
            var htmlTraitementEnCours = '<svg class="spinner" viewBox="0 0 50 50"><circle class="path" cx="25" cy="25" r="20" fill="none"></circle></svg>';

            if (elementSourceSoumission.html() !== htmlTraitementEnCours) {
                elementSourceSoumission.css("color", "rgba(255, 255, 255, .2)");
                elementSourceSoumission.append(htmlTraitementEnCours);
            }

            setTimeout(function () {
                elementSourceSoumission.addClass('disabled').attr('disabled', 'disabled');
            });
        }
    });
};


function definirParametresDefautSelect2() {
    if ($.fn.select2 != undefined) {
        $.fn.select2.defaults.set("theme", "bootstrap4");
        $.fn.select2.defaults.set("language", $('html').attr('lang') || 'fr');
    }
};

function ajusterAccessibiliteSelect2Multiple(controlesSelect2) {

    //Bien que l'initialisation d'un champ avec select2 soit synchrone, pour une raison obscure il semble que le tabindex du span de sélection est remis à -1 après l'initialisation... Le setTimeout nous garantie que notre traitement va passer au bon moment... Peut-être à revoir éventuellement.
    setTimeout(function () {
        $(controlesSelect2).each(function () {
            var conteneurControle = $(this).parent();
            var spanSelection = conteneurControle.find(".select2-selection.select2-selection--multiple");

            //Permettre focus sur span de sélection et aussi lui ajouter un libellé  (afin d'avoir le nom du champ au lecteur écran au focus). Cache le libellé au lecteur écran (ne pouvait pas être lié au champ, ce qu'on vient de corriger avec aria-label, donc était une information en double au lecteur écran à la lecture séquentielle du formulaire.)
            var libelle = spanSelection.closest(".select2-container").prevAll("label:first");
            spanSelection.attr("aria-label", libelle.text()).attr("tabindex", "0");
            libelle.attr("aria-hidden", "true");

            //TODO trouver un moyen pour donner le focus au span "combobox" qui est focusable lors du click sur le libellé, mais ça ne semble pas fonctionner.
            //libelle.on("mousedown", function () {
            //    selection.focus();
            //});

            //On désactive la saisie manuelle
            conteneurControle.find('.select2-search__field').prop('disabled', true);

            //Pills des éléments sélectionnés. À chaque sélection, les pills sont reconstruites. On doit masquer le X au lecteur écran.
            $(this).on('select2:select select2:unselect', function (e) {
                var conteneurControle = $(this).parent();
                conteneurControle.find(".select2-selection__choice__remove").attr("aria-hidden", "true");
            });
        });
    }, 100);
};


function definirParametresDefautDatatables() {

    /* Datatables - Options par défaut */
    if ($.fn.DataTable == undefined) {
        return;
    }

    $.fn.DataTable.ext.pager.numbers_length = 5;

    $.extend(true, $.fn.dataTable.defaults, {
        language: $('html').attr('lang') === 'fr' ? datatables.language.french : datatables.language.english,
        autoWidth: false,
        lengthMenu: [1, 2, 5, 10, 25, 50],
        pageLength: 25,
        lengthChange: false,
        paging: false,
        pagingType: "full_numbers_no_ellipses",
        searching: false,
        ordering: true,
        info: true,
        responsive: {
            details: false
        }
    });

    //Options par défaut à définir uniquement une fois la page chargée.
    $(document).ready(function () {
        $.extend(true, $.fn.dataTable.defaults, {
            "initComplete": function(settings, json) {

                // Lors d'un événement de pagination, redonner le focus au tableau et repositionner l'écran au haut du tableau.
                $(this).on('page.dt', function () {

                    var table = $(this);

                    // S'assurer que la table peut recevoir le focus.
                    table.attr("tabindex", "-1");

                    table.focus();
                    window.scroll(0, table.offset().top);

                    // TODO : Éventuellement remettre en place si sticky header réactivé.
                    //var hauteurMenuFlottant = $(".sticky-top").height();
                    //window.scroll(0, table.offset().top - hauteurMenuFlottant);
                });
            }
        });
    });
};


/**
 * Initialise les validations pour un formulaire. (jQuery unobstrusive)
 * @param {object} form Objet form pour lequel il faut initialiser les validations.
 */
function initialiserScriptsValidation(form) {
    if (!form) {
        return;
    }

    if (form.attr("data-executed")) {
        return;
    }

    form.removeData("validator");
    $.validator.unobtrusive.parse(form);
    form.attr("data-executed", "true");
};

/**
 * Effectue différents traitement de base pour un formulaire. Notamment :
 *  - Initialiser les validations
 *  - Appliquer le visuel pour les champs obligatoires
 *  - Appliquer le visuel pour les champs avec longueur maximale 
 *  - Activer la fonctionnalité d'overlay pendant la soumission (selon paramètre activerOverlaySoumission). Bloque la possibilité de cliquer des liens et des boutons pendant la soumission.
 *  
 * @param {object} form Objet form à initialiser.
 * @param {boolean} activerOverlaySoumission Indique si la fonctionnalité d'overlay doit être activée ou non.
 */
function initialiserFormulaire(form, activerOverlaySoumission) {
    initialiserScriptsValidation(form);
    ajouterElementsVisuelsChampsAvecLongueurMaximale(form);
    ajouterElementsVisuelsChampsObligatoires(form);

    if (activerOverlaySoumission) {
        activerOverlaySoumissionForm(form);
    }
};

/**
 * Applique le visuel pour les champs obligatoires. Soit ajouter l'astérisque rouge et les éléments d'accessibilité sur le libellé.
 *
 * @param {object} form Objet form contenant les champs obligatoires.
 */
function ajouterElementsVisuelsChampsObligatoires(form) {

    //Attention, par défaut les checkbox sont écartés à cause des checkbox dans la visionneuse de pdf pour lesquels on ne veut pas ajouter d'indicateur de champ obligatoire. Si un checkbox doit avoir l'indicateur il faut lui ajouter la classe "requis" sur l'input. 
    var champsRequis = form.find("[data-val-required]:visible, [data-val-requishtml], [data-val-requisdate]:visible");

    //La présence de la classe requis sur un input "force" l'application des éléments visuels de champs obligatoires, même si input de type checkbox. 
    champsRequis = champsRequis.filter(function (index) {
        var champ = $(this);
        return !champ.is("input[type='checkbox']") || champ.hasClass("requis");
    });

    var nbChampsRequis = 0;
    var htmlIconeChampRequis = '<span class="icone-champ-requis" aria-hidden="true">&nbsp;*</span>';

    //Effectuer le traitement uniquement s'il n'a pas déjà été fait.
    if (form.find(".texte-explicatif-champs-obligatoires").length == 0) {

        //Ajouter l'indicateur de champ requis et l'attribut aria-required (on utilise l'attribut aria pour ne pas entrer en conflit avec la gestion des erreurs natives)
        champsRequis.each(function () {
            var champ = $(this);
            var id = champ.attr("id");
            var parent = champ.parent();
            var label = parent.find("label[for=" + id + "]");

            //Les checkbox requis sont traités avec un validator custom (DoitEtreVrai). TODO éventuellement trouver un moyen de faire fonctionner avec required natif au lieu d'un validator custom si possible... Car en ce moment rien n'indique au lecteur écran que c'est un champ obligatoire. L'utilisation du required natif sera nettement préférable.
            if (!champ.is("input[type='checkbox']")) {
                champ.prop("required", true);
            }

            //il s'agit d'un groupe de radio bouttons
            if (champ.is(":radio") && parent.is(".input-group")) {
                label = parent.parent().find("label[class=libelle-groupe-radio]");
            } else if (label.length == 0 && parent.is(".input-group")) { //Particularité pour input-group. Si le label n'a pas été trouvé, si c'est un input-group on doit remonter d'un niveau
                label = parent.parent().find("label[for=" + id + "]");
            }

            if (label.length > 0) {
                label.append(htmlIconeChampRequis);
                nbChampsRequis += 1;
            }
        });

        //Ajouter le texte en entête qui indique la signification de l'astérisque (champs obligatoires)
        if (nbChampsRequis > 0) {
            var sommaireValidation = form.find("[data-valmsg-summary]");

            if (sommaireValidation.length > 0) {
                sommaireValidation.before('<div class="texte-explicatif-champs-obligatoires d-flex justify-content-end" aria-hidden="true">' + htmlIconeChampRequis + '<span>' + obtenirTexteEdite('texteCommunIndiqueReponseObligatoire') + '</span></div>');
            }
        }
    }
};

/**
 * Applique le visuel pour les champs avec longueur maximale. 
 *
 * @param {object} form Objet form contenant les champs obligatoires.
 */
function ajouterElementsVisuelsChampsAvecLongueurMaximale(form) {
    var champs = form.find("[data-val-maxlength-max]");

    //Effectuer le traitement uniquement s'il n'a pas déjà été fait.
    if (form.find(".info-attendue").length == 0) {

        //Ajouter le texte du nb. max de caractères et l'attribut maxLength
        champs.each(function () {
            var champ = $(this);
            var id = champ.attr("id");
            var parent = champ.parent();
            var label = parent.find("label[for=" + id + "]");
            var longueurMax = champ.attr("data-val-maxlength-max");

            champ.attr("maxlength", longueurMax);

            //Particularité pour input-group. Si le label n'a pas été trouvé, si c'est un input-group on doit remonter d'un niveau.
            if (label.length == 0 && parent.is(".input-group")) {
                label = parent.parent().find("label[for=" + id + "]");
            }

            if (label.length > 0) {
                var htmlLongueurMaxFormate = '<span class="info-attendue">' + obtenirTexteEdite('texteCommunNbCaracteresMaximum', longueurMax) + '</span>';
                label.append(htmlLongueurMaxFormate);
            }
        });
    }
};

/** Réinitialise l'affiche des éléments impactés par un traitement en cours (ex. soumission de formulaire). */
function terminerTraitementEnCoursSoumissionForm() {
    
    $("#zoneNotificationsLecteurEcran").text(obtenirTexteEdite('txtcacheCommunMajTerminee'));

    var spinner = $("button[type=submit] > .spinner, input[type=submit] > .spinner");

    if (spinner.length > 0) {
        var bouton = spinner.parent();

        bouton.removeClass(["d-flex", "justify-content-center"]);
        bouton.css("color", "");
        bouton.removeAttr("disabled").removeClass("disabled");
        spinner.remove();

        //Retrait du overlay de soumission (qui bloque toute possibilité de clic pendant la soumisssion) si actif.
        var formulaire = bouton.prop('form') || bouton.closest('form')[0];
        if ($(formulaire).data("overlaysoumission")) {
            bouton.css({ "z-index": bouton.data("zindex"), "position": bouton.data("position") });
            $(".overlay-soumission-form").remove();
        }
    }
};


function donnerFocusChampErreur(idElement) {
    var controle = $("#" + idElement);

    if (controle && controle.hasClass("summernote")) {

        //Si le contrôle validé est caché et qu'il existe un editeur html on donne le focus à ce dernier. Le scrollIntoView est nécessaire pour ce type de contrôle afin de s'assurer qu'il soit visible à l'écran le scroll ne se fait pas toujours automatiquement.
        controle.parent().find(".note-editable").focus().get(0).scrollIntoView();

    } else {
        controle.focus();
    }
};

/**
 * Donne le focus au premier champ du formulaire spécifié.
 * @param {object} formulaire L'objet javascript ou jQuery représentant le conteneur (form ou autre)
 * @param {boolean} repositionnerEcran Indique s'il faut repositionner l'écran (appliquable uniquement si appelé en contexte de retour d'une fenêtre modale.)
 * @returns {boolean} Booléen indiquant si un champ de formulaire a été trouvé.
 */
function donnerFocusPremierChampFormulaire(formulaire, repositionnerEcran) {
    formulaire = $(formulaire);

    //Vérifier si une modal est en cours de fermeture. Si c'est le cas, on repositionne l'écran comme il était avant l'affichage du modal(la méthode qui traite ça n'aura pas encore été exécutée et ça nous empêche de gérer le focus  à cause du position:fixed du body)
    if (repositionnerEcran  && document.body.style.position == "fixed") {
        repositionnerEcranCommeAvantAffichageModal();
    }

    // Ajouter au besoin les sélecteurs de contrôles disponibles dans nos formulaires.
    var premierChampFormulaire = formulaire.find(".form-control:not([disabled]), .select2-selection, .custom-control-input");

    if (premierChampFormulaire.length > 0) {
        premierChampFormulaire.first().focus();
        return true;
    } else {
        return false;
    }
};

/*======================================================================================================================*/
/* FONCTIONS UTILITAIRES POUR SUMMERNOTE
/*======================================================================================================================*/

function associerControlesEditionSummernoteAvecLibelles(conteneur) {

    if (conteneur) {

        var editeurs = conteneur.find(".note-editable");

        editeurs.each(function () {
            var parent = $(this).parent().parent().parent();
            var label = parent.find("label:first");

            if (label) {
                var idLabel = label.attr("id");

                if (!idLabel) {
                    idLabel = label.attr("for") + "Label";
                    label.attr("id", idLabel);
                }

                $(this).attr("aria-labelledby", idLabel);
            }
        });
    }
};

