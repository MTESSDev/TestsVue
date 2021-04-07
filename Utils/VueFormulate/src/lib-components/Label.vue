<template>
    <label :id="`${context.id}_label`" :class="context.classes.label" :for="context.id">
        <span>{{context.label}}</span>
        <span v-if="isRequired" class="sr-only">.&nbsp;Obligatoire.</span>
        <span v-if="hasValidationRules && messagesErreur" class="sr-only" aria-live="polite"> {{messagesErreur}}</span>
        <span v-if="isRequired" aria-hidden="true" class="icone-champ-requis">&nbsp;{{requiredFieldIndicator}}</span>
    </label>
</template>

<script>
    export default {
        props: {
            context: {
                type: Object,
                required: true
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
        }
    }
</script>
