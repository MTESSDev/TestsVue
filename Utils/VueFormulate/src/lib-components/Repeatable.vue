<template>
  <div :class="[context.classes.groupRepeatable]">
    <div class="tirroir">
      <a role="button" data-toggle="collapse" aria-expanded="true" v-bind:href="'#' + context.name +'-' + index" v-bind:aria-controls="context.name + '-' + index">
        <span class="libelle-instance">
          <span class="libelle">{{instanceLabel}}</span>
          <span class="numero">{{ index + 1 }}</span>
          <span class="icone-svg chevron-haut sm" aria-hidden="true"></span>
        </span>
      </a>
      <div v-bind:id="context.name + '-' + index" class="collapse show">
        <slot v-if="context.removePosition === 'after'" />
        <FormulateSlot
          name="remove"
          :context="context"
          :index="index"
          :remove-item="removeItem"
        >
          <component
            :is="context.slotComponents.remove"
            :context="context"
            :index="index"
            :remove-item="removeItem"
            v-bind="context.slotProps.remove"
          />
        </FormulateSlot>
        <slot v-if="context.removePosition === 'before'" />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    context: {
      type: Object,
      required: true
    },
    removeItem: {
      type: Function,
      required: true
    },
    index: {
      type: Number,
      required: true
    },
    instanceLabel: {
      type: String,
      required: true
    }
  }
}
</script>
