function faqDevelopperTout() {
    $('.faq .categorie.collapse:not(.show)').one('shown.bs.collapse', function () {
        $(this).find('.reponse.collapse').collapse('show');
    }).collapse('show');
    $('.faq .categorie.collapse.show .reponse.collapse').collapse('show');
};

function faqReduireTout() {
    $('.faq .collapse').collapse('hide');
};

function faqReduireToutSansAffichageCategorie() {
    $('.faq .categorie.collapse.show .reponse.collapse').collapse('hide');
}

function afficherFaqMobile() {
    //TODO Voir si meilleure solution, car si resize, les attributs vont rester et attributs absents si bouton pas encore cliqué... pas la fin du monde, mais...
    $("#faq").attr("role", "dialog").attr("aria-labelled-by", "titreFAQ");

    $('.faq').addClass('slide-in');
    //    setTimeout(function () { $('.lien-faq-mobile-fermer').focus(); }, 100);  TODO valider si focus bouton fermer ou 1ère question... Devrait être 1ère question.
    setTimeout(function () { $('.question:first a').focus(); }, 100);
    conserverFocusElement($('.faq-contenu').get(0));
};

function fermerFaqMobile() {
    $('.faq').removeClass('slide-in');
    $('.lien-faq-mobile-afficher').focus();
};

function verifierFaqRechercher(event) {
    if (event.keyCode == 13 || event.which == 13) {
        faqRechercher();
    }
};

function motCleAccepte(motCle) {
    return motCle.length > 2;
}

function motCleIgnore(motCle) {
    return !motCleAccepte(motCle)
}

function faqRechercher() {
    // Réinitialiser les surlignements
    $('.faq mark').contents().unwrap();
    $('#faqMotsClesIgnores').addClass('d-none');

    if ($('#faqRecherche').val() != '') {

        // Bâtir une liste de mots clés ignorés
        var motsClesIgnores = $('#faqRecherche').val()
            .split(' ')
            .filter(Boolean)
            .filter(motCleIgnore);

        // Ignorer les accents et la case sur les mots clés
        var motsCles = normaliserChaineCaracteres($('#faqRecherche').val())
            .split(' ')
            .filter(Boolean)
            .filter(motCleAccepte);

        // Pour chaque noeuds de textes
        $('.faq .recherche').each(function () {

            var nouveauTexte = $(this).text();

            // Pour chaque mot clé
            motsCles.forEach(function (motCle) {

                // Mettre à jour le texte avec le mot clé surligné
                nouveauTexte = surlignerMotCle(nouveauTexte, motCle);
            });

            // Mettre à jour le texte avec les mots clés surligné
            $(this).html(nouveauTexte);
        });

        // Affichage des mots clés ignorés
        if (motsClesIgnores.length > 0) {
            var listeMotsClesIgnores = obtenirTexteEdite('libelleFAQNombreMotsClesIgnores');
            listeMotsClesIgnores = listeMotsClesIgnores + ' ' + motsClesIgnores.join(', ');
            $('#faqMotsClesIgnores').html(listeMotsClesIgnores);
            $('#faqMotsClesIgnores').removeClass('d-none');
        }
    }

    // Affichage nombre d'occurrences
    var libelle = '';
    if ($('.faq mark').length == 0) {
        libelle = obtenirTexteEdite('libelleFAQNombreResultatAucun');
    }
    else {
        if ($('.faq mark').length == 1) {
            libelle = obtenirTexteEdite('libelleFAQNombreResultatSingulier');
        }
        else if ($('.faq mark').length > 1) {
            libelle = obtenirTexteEdite('libelleFAQNombreResultatPluriel');
        }
        libelle = libelle.replace("{0}", $('.faq mark').length);
    }

    $('#faqOccurrences').html(libelle);
    $('#faqOccurrences').removeClass('d-none');

    // Ouvrir les éléments contenant des mots surlignés, fermer les autres
    $('.faq').addClass('no-transition');
    $('.faq .categorie.collapse').each(function () {
        if ($(this).parent('.categorie-container').find('mark').length == 0) {
            $(this).collapse('hide');
        }
        else {
            $(this).collapse('show');
        }
    });
    $('.faq .reponse.collapse').each(function () {
        if ($(this).parent('.question').find('mark').length == 0) {
            $(this).collapse('hide');
        }
        else {
            $(this).collapse('show');
        }
    });
    $('.faq').removeClass('no-transition');
};

function surlignerMotCle(nouveauTexte, motCle) {

    var retour = '';

    // Ignorer les accents et la case pour la recherche
    var recherche = normaliserChaineCaracteres(nouveauTexte);

    // Tant que le mot clé est retrouvé
    var matchStart = recherche.indexOf(motCle);
    while (matchStart != -1) {

        // Surligné l'occurence trouvé
        var matchEnd = matchStart + motCle.length - 1;
        var beforeMatch = nouveauTexte.slice(0, matchStart);
        var matchText = nouveauTexte.slice(matchStart, matchEnd + 1);
        retour += beforeMatch + '<mark>' + matchText + '</mark>';

        // Texte suite à la première occurence
        nouveauTexte = nouveauTexte.slice(matchEnd + 1);

        // Rechercher la prochaine occurence
        recherche = normaliserChaineCaracteres(nouveauTexte);
        matchStart = recherche.indexOf(motCle);
    }

    retour += nouveauTexte;

    return retour;
};
