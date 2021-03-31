<template>
  <label :id="`${context.id}_label`" :class="context.classes.label" :for="context.id">
    <span>{{context.label}}</span>
    <span v-if="isRequired" class="sr-only">.&nbsp;Obligatoire</span>
    <span v-if="isRequired" aria-hidden="true" class="icone-champ-requis">{{requiredFieldIndicator}}</span>
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
    requiredFieldIndicator () {
      return this.isRequired ? '*' : ''
    },
    isRequired () {
      return (this.context.rules.findIndex((element) => element.ruleName === 'required') >= 0)
    }
  }
}
</script>
