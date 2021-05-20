<template>
    <label :id="`${context.id}_label`" :class="[context.classes.label, tooltip ? 'avec-tooltip' : '']" :for="context.id">
        <span>{{context.label}}</span><span v-if="isRequired" aria-hidden="true" class="icone-champ-requis">&nbsp;{{requiredFieldIndicator}}</span><span v-if="tooltip" class="conteneur-tooltip">&#xfeff;
    <button type="button" class="tooltip-toggle" data-toggle="tooltip" :title="tooltip">
        <span class="conteneur-puce">
            <span class="puce" aria-hidden="true">
                <span class="icone-svg question" aria-hidden="true"></span>
            </span>
        </span>
    </button>
</span>
        <span v-if="isRequired" class="sr-only">.&nbsp;Obligatoire.</span>
        <span v-if="context.help" class="sr-only"><span v-if="!isRequired">.</span>&nbsp;{{context.help}}</span>
        <span v-if="hasValidationRules && messagesErreur" class="sr-only" aria-live="polite"> {{messagesErreur}}</span>
    </label>
</template>

<script>
    //Note Importante! Il ne doit y avoir aucun espace entre le <label> et le <span> d'indicateur de champ obligatoire (seul moyen mettre sur même ligne comme c'est fait actuellement) Idem avec le span .conteneur-tooltip
    //aussi les &nbsp et &#xfeff;(espace insécable zero width) dans icone-champ-requis et conteneur-tooltip sont essentiels dans la stratégie css permettant de ne pas afficher un * et/ou un tooltip seuls sur une ligne.
    export default {
        props: {
            context: {
                type: Object,
                required: true
            },
            tooltip: {
                type: String,
                required: false
            }
        },
        computed: {
            messagesErreur() {
                return this.context.visibleValidationErrors.join(' ')
            }
        },
        created() {
            this.hasValidationRules = this.context.rules.length > 0
            this.isRequired = this.context.rules.findIndex((element) => element.ruleName === 'required') >= 0            
            this.requiredFieldIndicator = this.isRequired ? '*' : ''

            // Ne pas lire la précision au lecteur écran car nous l'avons ajoutée dans le label.
            if (this.context.help) {
                this.$nextTick(function () {
                    const help = this.$parent.$el.querySelector(".formulate-input-help")
                    if (help) {
                        help.setAttribute('aria-hidden', 'true')
                    }
                })
            }
        }
    }
</script>
