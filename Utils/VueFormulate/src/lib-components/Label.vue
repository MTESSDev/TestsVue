<template>
    <label :id="`${context.id}_label`" :class="context.classes.label" :for="context.id" :style="[tooltip ? {'position': 'relative'} : {}]">
        <span>{{context.label}}</span>
        <span v-if="isRequired" class="sr-only">.&nbsp;Obligatoire.</span>
        <span v-if="context.help" class="sr-only"><span v-if="!isRequired">.</span>&nbsp;{{context.help}}</span>
        <span v-if="hasValidationRules && messagesErreur" class="sr-only" aria-live="polite"> {{messagesErreur}}</span>
        <span v-if="isRequired" aria-hidden="true" class="icone-champ-requis">{{requiredFieldIndicator}}</span>
        <span class="conteneur-tooltip">
            <button v-if="tooltip" type="button" class="tooltip-toggle" data-toggle="tooltip" :title="tooltip">
                <span class="puce" aria-hidden="true">
                    <span class="icone-svg question" aria-hidden="true"></span>
                </span>
            </button>
        </span>
    </label>
</template>

<script>
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
