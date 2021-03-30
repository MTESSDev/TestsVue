/*! siteBase */
/* Traitements de base disponibles dans toutes les pages du site.
*/

//Traitement exécuté dès que le fichier JS est chargé.
var ecs = ecs || {};
ecs.dataTables = ecs.dataTables || {};
ecs.dataTables.defaut = ecs.dataTables.defaut || {};

ecs.hauteurMinimaleSroll = 555;
ecs.dataTables.defaut.scrollY = "400px";
ecs.nbVerificationsChargementBS = 0;

definirTrappeErreurGlboale();


//Traitement exécuté lorsque le rendu de l'interface est complété.
$(document).ready(function () {

    ajouterRetourHautPage();

    ajusterAccessibiliteLiens();

    $(".espaceur-conteneurs:visible:last").addClass("dernier-visible");

    /** TODO À déplacer uniquement aux endroits requis et non de façon globale */
    setTimeout(function () { definirHauteurAutomatiqueIFrame(); }, 100);
});

function ajusterAccessibiliteLiens() {
    $("main").find('a[target="_blank"]').each(function () {
        var lien = $(this);

        //Ne pas traiter les liens sans href et les liens
        if (!lien.attr('href')) {
            return;
        }
        //Ne pas traiter les liens vers des fichiers ou les liens ayant explicitement une classe indiquant de ne pas les considérer comme lien externe(sans-lien-externe).  TODO éventuellement rafiner la condition afin de traiter uniquement les liens vers des pages web...
        if (lien.attr('href').endsWith(".pdf") || lien.hasClass("sans-lien-externe")) {
            return;
        }

        //Ajouter la classe lien-externe afin d'afficher l'icône d'ouverture dans une nouvelle fenêtre.
        if (!lien.hasClass("lien-externe") && !lien.find(".lien-externe").length) {
            lien.addClass("lien-externe");
        }

        //Ajouter le texte pour accessibilité nouvelle fenêtre si on détecte qu'il n'est pas là. TODO rafiner cette vérification éventuellement au besoin, il pourrait arriver qu'un .sr-only soit là pour autre chose dans le lien (très peu probable)
        if (lien.find(".sr-only").length === 0) {
            var texteAccessibilite = obtenirTexteEdite("txtcacheCommunOuvertureLienNouvelOnglet");
            lien.append('<span class="sr-only">{0}</span>'.format(texteAccessibilite));
        }
    });

};

/**
* Définit un traitement de gestion globale des erreurs de script pour l'application. (Trappe d'erreur globale pour toute l'application).
*/
function definirTrappeErreurGlboale() {

    window.onerror = function (msg, url, lineNo, columnNo, error) {
        var detailsTechniques = "";

        //Déterminer les détails techniques selon la disponibilité du paramètre error (non disponible dans tous les fureteurs)
        if (error) {
            var stack = error.stack;
            detailsTechniques = error.toString();

            if (stack) {
                detailsTechniques += '\n' + stack;
            }
        }
        else {

            detailsTechniques = "Message : " + msg + "\nLigne : " + lineNo + "\nColonne : " + columnNo
        }

        journaliserErreurJs(detailsTechniques);

        return false;
    };

    window.addEventListener('unhandledrejection', function (event) {
        journaliserErreurJs(event.reason.stack);
    });
};

function journaliserErreurJs(message) {
    executerRequeteAjax('/api/log/error', { message: message });
};

function journaliserInformationJs(message) {
    executerRequeteAjax('/api/log/information', { message: message });
};

/** TODO À déplacer uniquement aux endroits requis et non de façon globale */
function definirHauteurAutomatiqueIFrame() {
    if (window.addEventListener) {
        window.addEventListener('message', function (e) {
            var iframe = $("iframe.autoheight");
            var eventName = e.data[0];
            var hauteur = e.data[1];
            switch (eventName) {
                case 'setHeight':
                    iframe.height(hauteur);
                    break;
            }
        }, false);
    }
};

function ajouterRetourHautPage() {
    if ($("#flecheHaut").length) {
        $(window).scroll(function () { retourHautPage(); });
    }

    $('#flecheHaut').on("click", function () {
        $("html, body").stop().animate({ scrollTop: 0 }, ecs.hauteurMinimaleSroll, 'swing', function () { });
    });
};

function retourHautPage() {
    if (document.body.scrollTop > ecs.hauteurMinimaleSroll || document.documentElement.scrollTop > ecs.hauteurMinimaleSroll) {
        $('#flecheHaut').fadeIn();
    } else {
        $('#flecheHaut').fadeOut();
    }
};

//Permet d'afficher un message de confirmation avant de décharger la page si un formulaire a été modifié
function confirmationDechargementPage(formulaireOriginal, nomFormulaire) {
    window.onbeforeunload = function (event) {
        if (formulaireOriginal != $(nomFormulaire).serialize()) {
            var message = obtenirTexteEdite('texteCommunChangementsPerdus');
            if (typeof event == 'undefined') {
                event = window.event;
            }
            if (event) {
                event.returnValue = message;
            }

            return message;
        }
    }
}

/** Définit la gestion du délai d'inactivité. */
function definirGestionDelaiInactivite(delaiAvertissementInactivite, delaiInactivite) {
    var timerAvertissement;
    var timerDeconnexion;

    var dfd = $.Deferred(function () {

        if (timerAvertissement) {
            clearTimeout(timerAvertissement);
        }

        if (timerDeconnexion) {
            clearTimeout(timerDeconnexion);
        }

        timerAvertissement = setTimeout(function () {
            var parametres = {
                type: "avertissement",
                titre: obtenirTexteEdite("titreGabaritDelaiInactivitePopUpConfirmation"),
                corps: obtenirTexteEdite("texteGabaritDelaiInactivitePopUpConfirmation"),
                texteBoutonPrimaire: obtenirTexteEdite("boutonGabaritDelaiInactivitePopUpConfQuitter"),
                texteBoutonSecondaire: obtenirTexteEdite("boutonGabaritDelaiInactivitePopUpConfRester"),
                forcerBoutons: true,
                afficherBoutonFermer: false
            };

            afficherMessage(parametres).done(function (resultat) {
                if (resultat.primaire) {
                    dfd.resolve();
                }
                else if (resultat.secondaire) {
                    dfd.reject();
                }
            });

        }, delaiAvertissementInactivite * 1000 * 60);

        timerDeconnexion = setTimeout(function () {
            dfd.reject();
        }, delaiInactivite * 1000 * 60);

    });

    var promesse = dfd.promise();

    promesse
        // Réinitialiser les timers 
        .done(function () {
            definirGestionDelaiInactivite(delaiAvertissementInactivite, delaiInactivite);
        })
        // Fermer la session
        .fail(function () {
            window.onbeforeunload = function () { };
            window.location.href = definirLienMultilingue('/Authentification/Deconnexion');
        });
}



//TODO faire un wrapper js pour les posts de VC
function executerRequeteAjaxBase(url, donnees) {

    donnees = donnees || {};

    return $.ajax({
        type: "POST",
        url: url,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]:first').val());
        },
        data: donnees
    });
};

function executerRequeteAjaxBaseGet(url, donnees, cache) {

    donnees = donnees || {};
    cache = cache !== false;

    return $.ajax({
        type: "GET",
        url: url,
        beforeSend: function (xhr) {
            if (!cache) {
                xhr.setRequestHeader("cache-control", "no-cache");
            } else {
                xhr.setRequestHeader("session_id", SESSION_ID);
            }
        },
        data: donnees
    });
};

function executerRequeteAjaxGet(url, donnees, conteneur, cache) {
    return executerRequeteAjax(url, donnees, conteneur, 'GET', cache)
};

function executerRequeteAjax(url, donnees, conteneur, methode, cache) {

    methode = methode || 'POST';
    cache = cache !== false;

    var dfd = $.Deferred(function () {

        donnees = donnees || {};

        if (conteneur && conteneur.find(".traitement-en-cours").length == 0) {
            afficherChargementEnCours(conteneur);

            //Si la requête ajax été initiée par un bouton on donne le focus au contrôle de chargement en cours (accessibilité).
            if (document.activeElement) {
                if ($(document.activeElement).is("button, input[type=submit]")) {
                    var spinner = conteneur.find(".traitement-en-cours");
                    spinner.focus();
                    spinner.get(0).scrollIntoView();
                    window.scrollBy(0, -48);
                }
            }
        }

        var executionRequeteAjax;
        switch (methode) {
            case "POST":
                executionRequeteAjax = executerRequeteAjaxBase;
                break;
            case "GET":
                executionRequeteAjax = executerRequeteAjaxBaseGet;
                break;
            default: alert('Méthode ' + methode + ' non gérée.');
                break;
        }

        executionRequeteAjax(url, donnees, cache)
            .done(function (retour) {
                if (retour && retour.resultat && retour.resultat === 'redirection' && retour.url) {
                    window.location.href = retour.url;
                    dfd.resolve(retour);
                } else {
                    if (conteneur) {

                        //Si le contrôle de traitement en cours a actuellement le focus, on donne le focus au premier élément focusable du conteneur. Si aucun élément focusable on tente de donner le focus au conteneur lui même. (accessibilité liée au chargement en cours).
                        var donnerFocusConteneur = document.activeElement && $(document.activeElement).is(".traitement-en-cours");

                        conteneur.fadeTo(50, 0.2, function () {
                            conteneur.html(retour);
                            dfd.resolve(retour);

                            conteneur.fadeTo(350, 1, function () {
                                if (donnerFocusConteneur) {
                                    var elementsFocusables = obtenirElementsFocusables(conteneur.get(0));
                                    if (elementsFocusables.length > 0) {
                                        elementsFocusables[0].focus();
                                    }
                                    else {
                                        conteneur.focus();
                                    }
                                }
                            });
                        });
                    } else {
                        dfd.resolve(retour);
                    }
                }
            })
            .fail(function (erreur) {
                //TODO Enlever l'appel test à afficherMessageErreur
                //afficherMessageErreurTest(erreur);

                if (conteneur) {
                    //Si le contrôle de traitement en cours a actuellement le focus, on donne le focus au premier élément focusable du conteneur. Si aucun élément focusable on tente de donner le focus au conteneur lui même. (accessibilité liée au chargement en cours).
                    var donnerFocusConteneur = document.activeElement && $(document.activeElement).is(".traitement-en-cours");

                    conteneur.fadeTo(50, 0.2, function () {
                        conteneur.html('<div class="info-non-disponible d-flex flex-column align-items-center" tabindex="0"><div aria-hidden="true" class="icone-svg averti lg"></div><div class="texte text-base">{0}</div></div>'.format(obtenirTexteEdite("messageCommunInformationNonDisponible")));
                        dfd.reject(erreur);

                        conteneur.fadeTo(350, 1, function () {
                            if (donnerFocusConteneur) {
                                var elementsFocusables = obtenirElementsFocusables(conteneur.get(0));
                                if (elementsFocusables.length > 0) {
                                    elementsFocusables[0].focus();
                                } else {
                                    conteneur.focus();
                                }
                            }
                        });
                    });
                } else {
                    dfd.reject(erreur);
                }
            });
    });

    return dfd.promise();
};

function afficherChargementEnCours(conteneur, parametres) {

    var valeursDefaut = {
        taille: "md",
        position: "text-center",
        afficherTexte: false,
        texte: obtenirTexteEdite('texteCommunChargementEnCours'),
        overlay: false,
        affichageDansBouton: false //non utilisé pour le moment
    };

    parametres = $.extend({}, valeursDefaut, parametres);

    var texte = '<div class="texte sr-only">{0}</div>'.format(parametres.texte);
    texte = parametres.afficherTexte ? texte.replace("sr-only", "") : texte;

    var classeOverlay = parametres.overlay ? 'avec-overlay' : '';

    var html = '<div class="traitement-en-cours {0} {1} {2}" tabindex="0" role="status">' +
        '<svg class="spinner" viewBox="0 0 50 50" aria-hidden="true"><circle class="path" cx="25" cy="25" r="20" fill="none"></circle></svg>' +
        '{3}</div>';
    html = html.format(classeOverlay, parametres.taille, parametres.position, texte);

    var controle = $(html);

    if (conteneur.height() > 0) {
        controle.width(conteneur.width());
        controle.height(conteneur.height());
    } else {
        controle.css("position", "relative");
    }

    conteneur.html(controle);
    controle.css("opacity", 1);
};


function afficherNotificationSuccesEnregistrement(message) {
    message = message || obtenirTexteEdite("messageCommunSuccesEnregistrement");
    afficherNotification({ message: message });
};

function afficherNotificationSuccesAnnulation(message) {
    message = message || obtenirTexteEdite("messageCommunSuccesAnnulation");
    afficherNotification({ type: "information", message: message });
};

function afficherNotificationEchecEnregistrement(message) {
    message = message || "Une erreur est survenue lors de l'enregistrement.";
    afficherNotification({ type: "echec", message: message });
};


/**
 * Affiche une fenêtre de message permettant à l'utilisateur de confirmer s'il désire annuler ou non les modifications effectuées.
 * */
function afficherMessageConfirmationAnnulationEdition(parametres) {
    var valeursDefaut = {
        type: "avertissement",
        titre: obtenirTexteEdite("messageCommunTitreConfirmationAnnulation"),
        corps: obtenirTexteEdite("messageCommunConfirmationAnnulation"),
        texteBoutonPrimaire: obtenirTexteEdite("boutonCommunAnnulerModification"),
        texteBoutonSecondaire: obtenirTexteEdite("boutonCommunPoursuivreEdition")
    };
    parametres = $.extend({}, valeursDefaut, parametres);

    return afficherMessage(parametres);
}

/**
 * Affiche une fenêtre de message permettant à l'utilisateur de confirmer s'il désire supprimer ou non un élément.
 * */
function afficherMessageConfirmationSuppression(parametres) {
    var valeursDefaut = {
        type: "avertissement",
        titre: obtenirTexteEdite("messageCommunTitreConfirmationSuppression"),
        corps: obtenirTexteEdite("messageCommunConfirmationSuppression"),
        texteBoutonPrimaire: obtenirTexteEdite("boutonCommunSupprimer"),
        texteBoutonSecondaire: obtenirTexteEdite("boutonCommunAnnuler")
    };
    parametres = $.extend({}, valeursDefaut, parametres);
    return afficherMessage(parametres);
}

/**
 * Affiche une fenêtre de message permettant à l'utilisateur de confirmer s'il désire supprimer ou non un élément.
 * */
function afficherMessageErreur(parametres) {
    var valeursDefaut = {
        type: "erreur",
        titre: obtenirTexteEdite("messageCommunTitreErreur"),
        corps: obtenirTexteEdite("messageCommunErreur"),
        texteBoutonPrimaire: obtenirTexteEdite("boutonCommunFermer")
    };
    parametres = $.extend({}, valeursDefaut, parametres);
    return afficherMessage(parametres);
}

function afficherMessageErreurTest(erreur) {
    var proprietes = erreur.readySate;
    //proprietes += "; " + erreur.getResponseHeader();
    proprietes += "; " + erreur.getAllResponseHeaders();
    proprietes += "; " + erreur.statusCode;
    proprietes += "; " + erreur.state;
    proprietes += "; " + erreur.responseText;
    proprietes += "; " + erreur.status;
    proprietes += "; " + erreur.statusText;

    var parametres = {
        type: "erreur",
        titre: "AffichageErreurTest",
        corps: proprietes,
        texteBoutonPrimaire: obtenirTexteEdite("boutonCommunFermer")
    };
    return afficherMessageErreur(parametres);
}

function repositionnerEcranCommeAvantAffichageModal() {
    var scrollY = document.body.style.top;
    document.body.style.position = '';
    document.body.style.top = '';
    window.scrollTo(0, parseInt(scrollY || '0') * -1);
}

/**
 * Gestion du bouton "Fermer la session".
 * */
function fermerSession() {
    afficherMessageConfirmationFermerSession()
        .done(function (resultat) {
            if (resultat.primaire) {
                /*window.onbeforeunload = function () { };*/
                window.location.href = definirLienMultilingue('/Authentification/Deconnexion');
            }
        });
}

/**
 * Affiche une fenêtre de message permettant à l'utilisateur de confirmer s'il désire fermer la session ou non.
 * */
function afficherMessageConfirmationFermerSession() {
    var parametres = {
        type: "avertissement",
        titre: obtenirTexteEdite('titreGabaritSessionFermeePopUpConfirmation'),
        corps: "<p>" + obtenirTexteEdite('texteGabaritSessionFermeePopUpConfirmation') + "</p>",
        texteBoutonPrimaire: obtenirTexteEdite('boutonGabaritSessionFermeePopUpConfQuitter'),
        texteBoutonSecondaire: obtenirTexteEdite('boutonGabaritSessionFermeePopUpConfRester')
    };

    return afficherMessage(parametres);
}


/**
 * Obtient un texte édité à partir de l'id fourni. 
 * @param {string} id Identifiant du texte edité.
 * @param {any} arguments optionnels à remplacer dans le texte édité. (Remplacement des éléments entre accolades {0}, {1}, {2}, etc.)
 * @returns {string} Champ description (français ou anglais) selon la langue d'affichage courante du site.
 * @example 
 * var monTexteEdite = obtenirTexteEdite("libelleCommunicationNombreElement", 10, 20, 100);
 * Si la description du texte édité était: "Affichage des éléments {0} à {1} sur {2} éléments."
 * Le résultat serait : "Affichage des éléments 10 à 20 sur 100 éléments."
 */
function obtenirTexteEdite(id) {

    //On retire le paramètre id, des paramètres reçus
    var parametres = Array.prototype.slice.call(arguments, 1);

    //On obtient le texte edité dans l'objet global à partir de son id, et on applique le formatage des paramètres s'il y a lieu.
    var texteEdite = ECS.textesEdites[id] || id;
    return texteEdite.format.apply(texteEdite, parametres);
};

//====================================================================
//format
//
//Permet de modifier de façon dynamique le contenu d'une chaîne de caractères, exactement comme le fait string.format() en c#.
//
//Retourne : La chaîne de caractère formatée.
//
//Exemple(s) d'utilisation :
// var test = "Un petit texte avec insertion dynamique. Valeur dynamique1 = {0}. Valeur dynamique2 = {1}".format(maVariableContenantValeur, maVariableContenantValeur2);
//====================================================================
// Vérifier si la fontion est déjà implémentée, si ce n'est pas le cas on l'implémente.
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match
                ;
        });
    };
}


//====================================================================
//injecterScriptDynamique
//
// * @param {string} chemin du script.
//
//Permet d'injecter un script de façon dynamique.
//====================================================================
function injecterScriptDynamique(cheminScript) {
    var element = document.createElement("script");
    element.src = cheminScript;
    document.body.appendChild(element);
};



/**
 * Permet de modifier une URL relative vers une page interne de l'espace client pour qu'elle respecte la langue d'affichage.
 * @param {string} url URL relative vers une page interne de l'espace client.
 * @returns {string} URL multilingue.
 * */
function definirLienMultilingue(url) {
    var urlMultilingue = url;

    // Pas besoin de modifier l'URL si on est français car c'est la langue par défaut
    var langue = $('html').attr('lang');
    if (langue != 'fr') {

        // S'assurer que l'URL respecte le format attendu
        if (!urlMultilingue.startsWith('/')) {
            urlMultilingue = '/' + urlMultilingue;
        }

        // S'assurer que l'URL commence par la langue d'affichage
        if (!urlMultilingue.toLowerCase().startsWith('/' + langue)) {
            urlMultilingue = '/' + langue + '/' + urlMultilingue.substring(1);
        }
    }

    return urlMultilingue;
};


/**
 * Exécute la fonction spécifiée une fois que le javascript de Boostrap a été chargé.
 * @param {object} fonctionAExecuter Fonction à exécuter une fois le script BS chargé.
 * @param {object} options Différentes options supplémentaires.
 * @param {integer} options.delaiVerificationMs Nombre de millisecondes à attendre avant chaque vérification.
 * @param {integer} options.nbTentativesMax Nombre de tentatives maximal de vérifications.
 * 
 */
function executerApresChargementBS(fonctionAExecuter, options) {
    options = $.extend({}, {
        delaiVerificationMs: 100,   // Entier représentant le nombre de millisecondes entre chaque vérification
        nbTentativesMax: 20
    }, options);

    if (typeof ($.fn.popover) != 'function') {
     
        ecs.nbVerificationsChargementBS += 1;

        if (ecs.nbVerificationsChargementBS > options.nbTentativesMax) {
            return;
        }
        else {
            setTimeout(function () { executerApresChargementBS(fonctionAExecuter, options); }, options.delaiVerificationMs);
        }
    }
    else {
        fonctionAExecuter();
    }
};


/* ---- TODO DÉBUT - Bloc à vérifier si peut être déplacé dans Async ---- */

/**
 * Obtient les éléments focusables à l'intérieur de l'élément spécifié.
 * @param {object} element Objet javascript pour lequel on veut les éléments focusables.
 */
function obtenirElementsFocusables(element) {
    return element.querySelectorAll('a[href]:not([disabled]), button:not([disabled]), textarea:not([disabled]), input[type="text"]:not([disabled]), input[type="radio"]:not([disabled]), input[type="checkbox"]:not([disabled]), select:not([disabled]), [tabindex]:not([tabindex="-1"])');
};

/**
 * Conserve le focus dans l'objet javascript reçu en paramètre.
 * @param {object} element Objet javascript dans lequel on veut conserver le focus.
 */
function conserverFocusElement(element) {
    var elementsFocusables = obtenirElementsFocusables(element),
        premierElementFocusable = elementsFocusables[0];
    dernierElementFocusable = elementsFocusables[elementsFocusables.length - 1];
    var CODE_TAB = 9;

    element.addEventListener('keydown', function (e) {
        var toucheTabAppuyee = (e.key === 'Tab' || e.keyCode === CODE_TAB);

        if (!toucheTabAppuyee) {
            return;
        }

        if (e.shiftKey) /* SHIFT + TAB */ {
            if (document.activeElement === premierElementFocusable) {
                if ($(dernierElementFocusable).is(':visible')) {
                    dernierElementFocusable.focus();
                    e.preventDefault();
                }
            }
        } else /* TAB */ {
            if (document.activeElement === dernierElementFocusable) {
                if ($(premierElementFocusable).is(':visible')) {
                    premierElementFocusable.focus();
                    e.preventDefault();
                }
            }
        }
    });
};

//====================================================================
//Génère un id unique.
//
//Retourne : L'id unique généré
//
//====================================================================
function genererId() {
    return Date.now().toString(36) + '-' + Math.random().toString(36).substr(2, 9);
};

//====================================================================
//Remplacer retour chariot
//
//Permet de modifier les retours de chariots en espace lorsque l'on change le nombre de rows que le textArea possède
//
//Retourne : La chaîne de caractère sans retour de chariot
//
//====================================================================
function remplacerRetourChariot(description) {

    if (/\r|\n/.exec(description)) {
        description = description.replace(/(?:\r\n|\r|\n)/g, " ");
    }
    return description;
}


/**
 * Remplace les accents d'une chaîne de caractères.
 * @param {string} chaineCaracteres Chaîne de caractères.
 * */
function remplacerAccents(chaineCaracteres) {
    return chaineCaracteres.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
};

/**
* Normaliser les apostrophes d'une chaîne de caractères.
* @param {string} chaineCaracteres Chaîne de caractères.
* */
function normaliserApostrophes(chaineCaracteres) {
    return chaineCaracteres.replace(/[\u2018-\u2019]/g, '\u0027');
};

/**
 * Normalise une chaîne de caractères pour utilisation insensible à la case et aux accents.
 * @param {string} chaineCaracteres Chaîne de caractères.
 * */
function normaliserChaineCaracteres(chaineCaracteres) {
    return normaliserApostrophes(remplacerAccents(chaineCaracteres).toLowerCase());
};

/**
 * Supprime les caracteres speciaux d'une chaîne de caractères.
 * @param {string} chaineCaracteres Chaîne de caractères.
 * */
function supprimerCaracteresSpeciaux(chaineCaracteres) {
    return chaineCaracteres.normalize('NFD').replace(/[^\w\s]/gi, '');
};
/* ---- TODO FIN - Bloc à vérifier si peut être déplacé dans Async ---- */

/*======================================================================================================================*/
/* Google analytics
 * ga('send', 'event', [eventCategory], [eventAction], [eventLabel], [eventValue], [fieldsObject]);
/*======================================================================================================================*/
function gestionGoogleAnalytics(idGoogleAnalytics) {      
    (function (d, e, j, h, f, c, b) {
        d.GoogleAnalyticsObject = f;
        (d[f] =
            d[f] ||
            function () {
                (d[f].q = d[f].q || []).push(arguments);
            }),
            (d[f].l = 1 * new Date());
        (c = e.createElement(j)), (b = e.getElementsByTagName(j)[0]);
        c.async = 1;
        c.src = h;
        b.parentNode.insertBefore(c, b);
    })(window, document, "script", "//www.google-analytics.com/analytics.js", "ga");
    ga("create", idGoogleAnalytics, "auto");
    ga("set", "anonymizeIp", true);
    ga("send", "pageview");

    $(document).on('click', 'a[href], button, input[type=button], input[type=submit], th[class^="sorting"], #flecheHaut', function () {
        var element = $(this);
        gestionGoogleAnalyticSpecifique(element);
    });
};

function gestionGoogleAnalyticSpecifique(element) {

    // Catégorie d'événement
    var categorie = determinerCategorieEvenement(element);

    // Action d'événement
    var action = determinerActionEvenement(element);

    // Libellé d'événement
    var libelle = determinerLibelleEvenement(element);

    // Traçage de l'événement
    ga('send', 'event', categorie, action, libelle);
};

function determinerCategorieEvenement(element) {
    var categorie = element.data('ga-categorie') || document.title;
    return categorie;
};

function determinerActionEvenement(element) {
    var action = element.data('ga-action');
    if (!action) {
        if (element.parents(".piv-entete").length) {
            action = "Menu PIV Haut";
        }
        else if (element.parents(".conteneur-menu-identification").length) {
            action = "Menu Identification";
        }
        else if (element.parents(".piv-bas-page").length) {
            action = "Menu PIV Bas";
        }
        else {
            var entete = $('*')
                .slice(0, $('*').index(element))
                .filter(':header:not([data-valmsg-summary] h2):last');
            action = obtenirTexte(entete);
        }
    }
    return action;
};

function determinerLibelleEvenement(element) {
    var libelle = element.data('ga-libelle') || obtenirTexte(element) || '';
    if (libelle.length === 0) {
        libelle = element.find("img").attr("alt") || element.attr("value") || '';
    }
    if (element.is('.contenu-extensible [data-toggle="collapse"]')) {
        libelle = element.is('.collapsed') ? element.find('.developper .sr-only').text() : element.find('.reduire .sr-only').text();
    }
    if (element.is('#faq [data-toggle="collapse"]')) {
        libelle = element.is('.collapsed') ? libelle + ' ouvert' : libelle + ' fermer';
    }
    libelle = libelle.replace(obtenirTexteEdite('txtcacheCommunOuvertureLienNouvelOnglet'), '');
    return libelle;
};

function obtenirTexte(element) {
    return element.text().replace(/(\r\n|\n|\r)/gm, '').replace(/\s+/g, ' ').trim();
};
