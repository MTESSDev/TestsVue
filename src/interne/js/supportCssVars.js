/*! supportCssVars */
/* Traitements spécifiques au support des variables css.
*/

/** Ajoute le support "ponyfill" afin que l'on puisse utiliser des variables css dans les vieux fureteurs (ex. IE11) */
function ajouterSupportVariablesCss() {
    cssVars({
        include: '[data-include]'
    });
};

ajouterSupportVariablesCss();