<template>
    <div v-if="visibleErrors.length > 0">
        <div v-if="type === 'form'">
            <div id="errorSummary" tabindex="-1" v-if="visibleErrors.length > 0">
                <h2>Des erreurs sont présentes dans le formulaire</h2>
                <ul :class="outerClass">
                    <li v-for="(error, index)  in errorSummary"
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
            <ul :class="outerClass" aria-hidden="true">
                <li v-for="error in visibleErrors"
                    :key="error"
                    :class="itemClass"
                    :role="role"
                    :aria-live="ariaLive"
                    v-text="error" />
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
        updated() {
            this.$nextTick(function () {
                if (this.type === 'form' && this.$root.$el.getAttribute('data-submit')) {
                    this.$root.$el.removeAttribute('data-submit')
                    document.getElementById('errorSummary').focus()
                }
            })
        },
        computed: {
            errorSummary() {
                return this.visibleErrors.length > 0 ? JSON.parse(this.visibleErrors) : []
            }
        },
        methods: {
            setInputFocus(id) {
                // setTimeout requis pour que le focus et le scroll se fasse.
                setTimeout((controlId) => {
                    const errorControl = document.getElementById(controlId)

                    if (errorControl) {
                        errorControl.focus()
                    } else {
                        // Nous n'avons pas trouvé le contrôle (ex. radio button). On recherche les contrôles dont l'id débute par notre id, et on conserve le premier contrôle de type input.
                        const errorControls = document.querySelectorAll('*[id^="' + controlId + '"]')
                        for (let i = 0; i < errorControls.length; i++) {
                            const control = errorControls[i]
                            if (control.tagName.toLowerCase() === 'input') {
                                control.focus()
                                break
                            }
                        }
                    }
                }, 10, id)
                return false
            }
        }
    }
</script>
