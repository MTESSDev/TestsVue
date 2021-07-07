<template>
  <div :class="[context.classes.groupRepeatable]">
    <div class="contenu-extensible instance-groupe card section-secondaire sans-bordure">
        <div class="card-title ">
            <a role="button" data-toggle="collapse" aria-expanded="true" v-bind:href="'#' + context.name +'-' + index" v-bind:aria-controls="context.name + '-' + index">
                <span class="libelle" v-bind:id="'label' + context.name +'-' + index">
                    <span class="libelle-instance">{{instanceLabel || this.context.label}}&nbsp;</span>
                    <span v-if="this.context.repeatable" class="numero">{{ index + 1 }}</span>
                </span>
                <span class="icone-svg md chevron-haut-texte" aria-hidden="true"></span>
            </a>
        </div>
        <div role="group" v-bind:aria-labelledby="'label' + context.name +'-' + index" v-bind:id="context.name + '-' + index" class="collapse show" >
            <div class="card-body">
                <slot v-if="context.removePosition === 'after'" />
                <FormulateSlot name="remove"
                               :context="context"
                               :index="index"
                               :remove-item="removeItem">
                    <component :is="context.slotComponents.remove"
                               :context="context"
                               :index="index"
                               :remove-item="removeItem"
                               v-bind="context.slotProps.remove" />
                </FormulateSlot>
                <slot v-if="context.removePosition === 'before'" />
            </div>
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
