<template>
    <label :id="`${context.id}_label`" :class="[context.classes.label, tooltipText ? 'has-tooltip' : '']" :for="context.id">
        <span>{{context.label}}</span><span v-if="isRequired" aria-hidden="true" class="icone-champ-requis">&nbsp;{{requiredFieldIndicator}}</span><span v-if="tooltipText" class="conteneur-tooltip">&#xfeff;
    <button type="button" class="tooltip-toggle" data-toggle="modal" @click="showTooltip()">
        <span class="conteneur-puce">
            <span class="puce" aria-hidden="true">
                <span class="icone-svg question" aria-hidden="true"></span>
            </span>
        </span>
    </button>
</span>
        <span v-if="isRequired" class="sr-only">.&nbsp;Obligatoire.</span>        
        <span v-if="hasValidationRules && messagesErreur" class="sr-only" aria-live="polite"> {{messagesErreur}}</span>
    </label>
</template>

<script>
    //Note Importante! Il ne doit y avoir aucun espace entre le <label> et le <span> d'indicateur de champ obligatoire (seul moyen mettre sur m�me ligne comme c'est fait actuellement) Idem avec le span .conteneur-tooltip
    //aussi les &nbsp et &#xfeff;(espace ins�cable zero width) dans icone-champ-requis et conteneur-tooltip sont essentiels dans la strat�gie css permettant de ne pas afficher un * et/ou un tooltip seuls sur une ligne.
    export default {
        props: {
            context: {
                type: Object,
                required: true
            },
            tooltipTitle: {
                type: String,
                required: false
            },
            tooltipText: {
                type: String,
                required: false
            }
        },
        methods: {
            showTooltip() {
                this.$root.showTooltip(this.tooltipTitle, this.tooltipText)
            },
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

            // Ne pas lire la pr�cision au lecteur �cran car nous l'avons ajout�e dans le label.
            if (this.context.help) {
                this.$nextTick(function () {
                    console.log(this)

                    const help = this.$parent.$el.querySelector(".formulate-input-help")

                    if (help) {
                        //help.setAttribute('aria-hidden', 'true')
                        this.$el.setAttribute('aria-describedby', help.id)
                    }
                })
            }
        }
    }
</script>
