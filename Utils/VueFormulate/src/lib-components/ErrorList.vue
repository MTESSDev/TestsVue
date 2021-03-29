<template>
  <div v-if="visibleErrors.length > 0">
    <div v-if="type === 'form'">
      <div id="errorSummary" tabindex="-1" v-if="visibleErrors.length > 0">
        <h2>Des erreurs sont présentes dans le formulaire</h2>
        <ul :class="outerClass">
          <li
            v-for="(error, index)  in errorSummary"
            :key="index"
            :class="itemClass"
            :role="role"
            :aria-live="ariaLive">
            <a href="#" @click="setInputFocus(error.id)">{{error.message}}</a>
          </li>
        </ul>
      </div>
    </div>
    <div v-else>
      <ul :class="outerClass">
        <li
          v-for="error in visibleErrors"
          :key="error"
          :class="itemClass"
          :role="role"
          :aria-live="ariaLive"
          v-text="error"
        />
      </ul>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    visibleErrors: {
      type: Array,
      required: true
    },
    itemClass: {
      type: [String, Array, Object, Boolean],
      default: false
    },
    outerClass: {
      type: [String, Array, Object, Boolean],
      default: false
    },
    role: {
      type: [String],
      default: 'status'
    },
    ariaLive: {
      type: [String, Boolean],
      default: 'polite'
    },
    type: {
      type: String,
      required: true
    }
  },
  updated () {
    this.$nextTick(function () {
      if (this.type === 'form' && this.$root.$el.getAttribute('data-submit')) {
        this.$root.$el.removeAttribute('data-submit')
        document.getElementById('errorSummary').focus()
      }
    })
  },
  computed: {
    errorSummary () {
      return this.visibleErrors.length > 0 ? JSON.parse(this.visibleErrors) : []
    }
  },
  methods: {
    setInputFocus (id) {
      // setTimeout requis pour que le focus et le scroll se fasse.
      setTimeout(() => {
        document.getElementById(id).focus()
      }, 10)
      return false
    }
  }
}
</script>
