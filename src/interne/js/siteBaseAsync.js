
/*! siteBaseAsync */
/* Traitements de base disponibles dans toutes les pages du site qui sont chargés et exécutés APRÈS le chargement complet d'une page.
*/

//Traitement exécuté dès que le fichier JS est chargé.
bloquerScrollModalsBootsrap();
desactiverCopierColler();

/*======================================================================================================================*/
/* NOTIFICATIONS
/*======================================================================================================================*/
/**
 * Affiche une notification (toast Bootstrap).
 * @param {Object} parametres Paramètres.
 * @param {Object} parametres.type Type de notification (succes ou echec). Défaut "succes".
 * @param {Object} parametres.titre Titre de la notification (ex. Succès) Défaut "Succès" ou "Échec".
 * @param {Object} parametres.message Message de la notification (ex. Enregistrement effectué avec succès.) Défaut "".
 * @param {Object} parametres.delaiFermeture Délai (en ms) de fermeture automatique de la notification. Défaut 5000.
 */
function afficherNotification(parametres) {

    var valeursDefaut = {
        type: "succes",
        titre: "", //parametres.type == "echec" ? "Échec" : "Succès",
        message: "",
        delaiFermeture: 5000
    };

    if (parametres.type == "echec") {
        valeursDefaut.titre = "Échec";
    } else if (parametres.type == "information") {
        valeursDefaut.titre = obtenirTexteEdite("messageCommunTitreSuccesAnnulation");
    } else {
        valeursDefaut.titre = obtenirTexteEdite("messageCommunTitreConfirmationEnregistrement");
    }

    parametres = $.extend({}, valeursDefaut, parametres);

    var zoneNotifications = $("#zoneNotifications");
    var htmlNotification = afficherNotification_obtenirHtml(parametres);

    zoneNotifications.append(htmlNotification);

    var nouvelleNotification = zoneNotifications.find(".toast:last");
    nouvelleNotification.toast("show");

    // Solution permettant d'avoir une animation à l'affichage de la notification (ne fonctionne pas nativement avec BS. Fonctionne uniquement à la fermeture.)
    nouvelleNotification.on('shown.bs.toast', function () {
        var that = this;
        setTimeout(function () { $(that).addClass("visible"); }, 10);
    });

    // Permettre la fermeture du toast lors d'un click sur cette dernière.
    nouvelleNotification.on('click', function () {
        $(this).toast("hide");
    });

    //Supprimer l'élément du DOM une fois qu'il n'est plus affiché.
    nouvelleNotification.on('hidden.bs.toast', function () {
        $(this).remove();
    });
};

function afficherNotification_obtenirHtml(parametres) {
    if (parametres.type == "echec") {
        classeCouleurFond = "bg-danger";
        classeIcone = "erreur";
    } else if (parametres.type == "information") {
        classeCouleurFond = "bg-info";
        classeIcone = "inform";
    } else {
        classeCouleurFond = "bg-success";
        classeIcone = "succes";
    }

    var html = '\
        <div class="toast {0}" data-delay="{1}" role="alert" aria-live="assertive" aria-atomic="true">\
            <div class="toast-header">\
                <span class="icone-svg mr-3 {2}" aria-hidden="true"></span>\
                <span class="mr-auto titre">{3}</span>\
                <button type="button" class="close" data-dismiss="toast" aria-label="{4}">\
                    <span aria-hidden="true">&times;</span>\
                </button>\
            </div>\
            <div class="toast-body">\
                {5}\
            </div>\
        </div>'.format(classeCouleurFond, parametres.delaiFermeture, classeIcone, parametres.titre, obtenirTexteEdite("boutonCommunFermer"), parametres.message);
    return html;

}


/*======================================================================================================================*/
/* FENÊTRE DE MESSAGE
/*======================================================================================================================*/
/**
 * Affiche une fenêtre de message (dialog).
 * @param {Object} parametres Paramètres.
 * @param {Object} parametres.type Type de message ("erreur", "information", "avertissement", "succes"  pour l'instant, autres types à venir). Défaut "avertissement".
 * @param {Object} parametres.titre Titre du message. Texte brut ou HTML (ex. Annuler les modifications) Défaut "". 
 * @param {Object} parametres.corps Corps du message. Texte brut ou HTML (ex. Désirez-vous annuler les modifications ou poursuivre?.) Défaut "".
 * @param {Object} parametres.texteBoutonPrimaire Texte du bouton primaire. (Celui le plus à droite). Si vide n'est pas affiché.
 * @param {Object} parametres.texteBoutonSecondaire Texte du bouton secondaire. (Situé à la gauche du bouton primaire). Si vide n'est pas affiché.
 * @param {Object} parametres.forcerBoutons Forcer l'utilisateur à utiliser le bouton primaire ou secondaire pour fermer la fenêtre de message. Défaut false.
 * @param {Object} parametres.afficherBoutonFermer Afficher le bouton pour fermer la fenêtre de message. Défaut true.
 * @param {Object} parametres.idControleFocusFermeture Id du contrôle auquel on redonne le focus à la fermeture de la fenêtre de message.
 * @param {Object} parametres.afficherBoutonSecondaire Afficher le bouton secondaire. Défaut true.
 * @returns {Object} Une promesse jQuery qui contiendra éventuellement un objet contenant la raison de fermeture. (ex. objet.primaire ou objet.secondaire)
 * @example afficherMessage(parametres)
            .done(function (resultat) {
                if (resultat.primaire) {
                    alert("Très bon choix! Poursuivre aurait pu causer une rupture du continuum espace temps!");
                } else if (resultat.secondaire) {
                    alert("Mauvais choix! Vous auriez-du poursuivre. À cause de vous le continuum espace temps risque de se briser!");
                } else {
                    alert("Vous vous êtes contenté de fermer la fenêtre sans faire de choix... La prochaine fois assumez-vous! L'avenir du monde est entre vos mains!");
                }
            });
 * @references https://www.w3.org/TR/wai-aria-practices/examples/dialog-modal/alertdialog.html
 * @notes Le titre et les boutons sont lus 2 fois dans NVDA... ça semble faire partie du pattern (voir lien ci-dessus). 
 */
function afficherMessage(parametres) {

    var valeursDefaut = {
        type: "avertissement",
        titre: "",
        corps: "",
        texteBoutonPrimaire: "",
        texteBoutonSecondaire: "",
        forcerBoutons: false,
        afficherBoutonFermer: false,
        idControleFocusFermeture: null,
        afficherBoutonSecondaire: parametres.type && parametres.type !== "information" && parametres.type !== "erreur"
    };

    parametres = $.extend({}, valeursDefaut, parametres);

    parametres.idControleFocusFermeture = afficherMessage_obtenirIdControleFocusFermeture(parametres);

    var htmlMessage = afficherMessage_obtenirHtml(parametres);

    $("body").append(htmlMessage);

    var fenetreMessage = $(".modal:last");

    //Donner le focus au premier bouton de la fenêtre de message
    fenetreMessage.on('shown.bs.modal', function () {
        fenetreMessage.find("button:first").trigger('focus');
    });

    if (parametres.forcerBoutons) {
        fenetreMessage.modal({ backdrop: 'static', keyboard: false, show: true });
    }
    else {
        fenetreMessage.modal({ backdrop: 'static', show: true });
    }

    //Conserver le focus dans la fenêtre modale.
    conserverFocusElement(fenetreMessage.get(0));

    //Affecter la raison de fermeture de la fenêtre modale au click d'un bouton
    fenetreMessage.find(".modal-footer, .pied").find("button").on("click", function () {
        fenetreMessage.data("raison-fermeture", $(this).data("raison-fermeture"));
    });

    //Définir une promesse qui sera résolue à la fermeture de la fenêtre.
    var dfd = $.Deferred();
    afficherMessage_definirEvenementFermeture(fenetreMessage, dfd);

    return dfd.promise();
};

/* Obtient l'id du contrôle auquel il faut redonner le focus lors de la fermeture. */
function afficherMessage_obtenirIdControleFocusFermeture(parametres) {
    if (!parametres.idControleFocusFermeture) {
        if (document.activeElement) {
            var id = document.activeElement.id;

            if (!id) {
                id = genererId();
            }

            document.activeElement.id = id;
            parametres.idControleFocusFermeture = id;

            return id;
        }
    };
    return parametres.idControleFocusFermeture;
};

function afficherMessage_obtenirHtml(parametres) {
    var classeIcone = afficherMessage_obtenirClasseIcone(parametres.type);

    var idControleFocusFermeture = parametres.idControleFocusFermeture || "";

    var html = ('\
        <div class="fenetre-message modal {0}" role="alertdialog" aria-labelledby="titreFenetreMessage" aria-describedby="contenuFenetreMessage" tabindex="-1" data-idfocus="{7}">\
          <div class="modal-dialog">\
            <div class="modal-content">\
              <div class="modal-header d-flex align-items-center">\
                <span class="icone-svg {1}" aria-hidden="true"></span>\
                <h1 id="titreFenetreMessage" class="modal-title ml-3" aria-hidden="true">{2}</h1>'

        + (parametres.afficherBoutonFermer ?
            '<button type="button" class="close" data-dismiss="modal" data-raison-fermeture="fermeture" aria-label="{6}" data-ga-action="{2}" data-ga-libelle="{6} popup {2}">\
                    <span aria-hidden="true">&times;</span>\
                </button>' : '<span class="close"></span>') +

        '</div>\
              <div id="contenuFenetreMessage" class="modal-body">\
                {3}\
              </div>\
              <div class="modal-footer pied">'

        + (parametres.afficherBoutonSecondaire ? '<button type="button" class="btn btn-secondaire" data-raison-fermeture="secondaire" data-dismiss="modal" data-ga-action="{2}">{4}</button>' : '') +

        '<button type="button" class="btn btn-secondaire ml-2" data-raison-fermeture="primaire" data-dismiss="modal" data-ga-action="{2}">{5}</button>\
              </div>\
            </div>\
          </div>\
        </div>').format(parametres.type, classeIcone, parametres.titre, parametres.corps, parametres.texteBoutonSecondaire, parametres.texteBoutonPrimaire, obtenirTexteEdite("boutonCommunFermer"), idControleFocusFermeture);
    return html;
};

function afficherMessage_obtenirClasseIcone(type) {
    switch (type) {
        case "erreur":
            return "erreur";
        case "avertissement":
            return "averti";
        case "succes":
            return "succes";
        default:
            return "inform";
    }
}

/**
 * Compléter la promesse indiquant de quelle façon la fenêtre s'est fermée et supprimer l'élément du DOM une fois qu'il n'est plus affiché.
 * @param {object} fenetreMessage Objet jQuery correspondant à la fenêtre de message.
 */
function afficherMessage_definirEvenementFermeture(fenetreMessage, promesse) {

    fenetreMessage.on('hidden.bs.modal', function (e) {
        var retour = {};
        var raisonFermeture = $(this).data("raison-fermeture");

        raisonFermeture = raisonFermeture || "fermeture";
        retour[raisonFermeture] = true;
        
        promesse.resolve(retour);

        //Redonne le focus à l'élément spécifié (normalement celui à l'origine de l'ouverture de la fenêtre de message)
        var idfocus = fenetreMessage.data("idfocus");
        if (idfocus) {
            $("#" + idfocus).focus();
        }

        //Destruction de la fenêtre modale
//        $(this).modal('dispose');
        $(this).remove();
    });
};


/* Bloque le scrolling du body lorsqu'une modal bootstrap est ouverte. 
   Permet également de conserver la position d'écran actuelle. (i.e. La modal est affichée sans provoquer un scroll au haut de l'écran, idem lorsqu'on la ferme.) */
function bloquerScrollModalsBootsrap() {
    $(window).on('show.bs.modal', function (e) {
        // Bloquer le scroll si ce n'est pas déja fait
        if (document.body.style.position !== "fixed") {
            var scrollY = window.scrollY;
            document.body.style.position = 'fixed';
            document.body.style.top = '-{0}px'.format(scrollY);
        }
        else {
            // Garder une trace du padding qui a été calculé par bootstrap pour le scroll bar
            $(document.body).data('paddingRight', document.body.style.paddingRight);
        }
    });

    $(window).on('hidden.bs.modal', function (e) {
        if ($('.modal.show').length >= 1) {
            // Remettre le style enlevé lors de la fermeture du 1er popup
            document.body.className = 'modal-open';
            document.body.style.paddingRight = $(document.body).data('paddingRight');
        }
        else {
            // Il n'y a plus de popup d'ouvert alors permettre le scroll
            repositionnerEcranCommeAvantAffichageModal();
        }
    });
};

/**
 * Désactiver l'action coller dans les champs ayant la classe css nopaste*/
function desactiverCopierColler() {
    $(".nopaste").each(function () {
        $(this).bind("paste", function (e) {
            e.preventDefault();
        });
    });
};


