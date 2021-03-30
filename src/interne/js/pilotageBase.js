/*! pilotageBase */
/* Traitements spécifiques au volet "Pilotage".
*/

var ecs = ecs || {};
ecs.summernote = ecs.summernote || {};
ecs.summernote.defaut = ecs.summernote.defaut || {};

//Configuration par défaut de summernote (notre rich text editor)
definirConfigurationDefautSummernote();

//Configuration par défaut de summernote (notre rich text editor)
function definirConfigurationDefautSummernote() {
    ecs.summernote.defaut =
    {
        lang: 'fr-FR',
        toolbar: [
            ['style', ['style']],
            ['font', ['bold', 'clear']],
            ['para', ['ul', 'ol']],
            ['insert', ['link']],
            ['view', ['codeview', 'undo', 'redo']]
        ],
        styleTags: [
            'h2', 'h3', 'h4', 'h5', 'h6'
        ],
        callbacks: {
            onInit: function () {
                var form = $(this).closest("form");

                //Ajouter les éléments d'accessibilité requis lorsqu'un formulaire contenant un champ summernote est soumis.
                if (!form.hasClass("summernote-pret")) {
                    form.on("submit", function () {

                        var editeursHtml = form.find('.note-editable');

                        editeursHtml.each(function () {
                            var editeurHtml = $(this);
                            editeurHtml.find('a').each(function () {
                                var lien = $(this);
                                if (lien.is('[target = "_blank"]')) {
                                    lien.addClass("lien-externe");
                                    if (lien.find(".sr-only").length === 0) { 
                                        var texteAccessibilite = obtenirTexteEdite("txtcacheCommunOuvertureLienNouvelOnglet");
                                        lien.append('<span class="sr-only">{0}</span>'.format(texteAccessibilite));
                                    }
                                } else {
                                    lien.removeClass("lien-externe");
                                }
                            });

                            var textarea = editeurHtml.parents(".note-editor:first").siblings("textarea:first");

                            if (textarea.length === 1) {
                                textarea.val(editeurHtml.html());
                            }
                        });
                    });
                }

                form.addClass("summernote-pret");
            },
            onFocus: function () {
                $(this).parent().find(".lien-externe .sr-only").remove();
            }
        }
    };
}
