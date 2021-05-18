//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
var script$3 = {
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
      return this.context.visibleValidationErrors.join(' ');
    }

  },

  created() {
    this.hasValidationRules = this.context.rules.length > 0;
    this.isRequired = this.context.rules.findIndex(element => element.ruleName === 'required') >= 0;
    this.requiredFieldIndicator = this.isRequired ? '*' : ''; // Ne pas lire la pr�cision au lecteur �cran car nous l'avons ajout�e dans le label.

    if (this.context.help) {
      this.$nextTick(function () {
        const help = this.$parent.$el.querySelector(".formulate-input-help");

        if (help) {
          help.setAttribute('aria-hidden', 'true');
        }
      });
    }
  }

};

function normalizeComponent(template, style, script, scopeId, isFunctionalTemplate, moduleIdentifier /* server only */, shadowMode, createInjector, createInjectorSSR, createInjectorShadow) {
    if (typeof shadowMode !== 'boolean') {
        createInjectorSSR = createInjector;
        createInjector = shadowMode;
        shadowMode = false;
    }
    // Vue.extend constructor export interop.
    const options = typeof script === 'function' ? script.options : script;
    // render functions
    if (template && template.render) {
        options.render = template.render;
        options.staticRenderFns = template.staticRenderFns;
        options._compiled = true;
        // functional template
        if (isFunctionalTemplate) {
            options.functional = true;
        }
    }
    // scopedId
    if (scopeId) {
        options._scopeId = scopeId;
    }
    let hook;
    if (moduleIdentifier) {
        // server build
        hook = function (context) {
            // 2.3 injection
            context =
                context || // cached call
                    (this.$vnode && this.$vnode.ssrContext) || // stateful
                    (this.parent && this.parent.$vnode && this.parent.$vnode.ssrContext); // functional
            // 2.2 with runInNewContext: true
            if (!context && typeof __VUE_SSR_CONTEXT__ !== 'undefined') {
                context = __VUE_SSR_CONTEXT__;
            }
            // inject component styles
            if (style) {
                style.call(this, createInjectorSSR(context));
            }
            // register component module identifier for async chunk inference
            if (context && context._registeredComponents) {
                context._registeredComponents.add(moduleIdentifier);
            }
        };
        // used by ssr in case component is cached and beforeCreate
        // never gets called
        options._ssrRegister = hook;
    }
    else if (style) {
        hook = shadowMode
            ? function (context) {
                style.call(this, createInjectorShadow(context, this.$root.$options.shadowRoot));
            }
            : function (context) {
                style.call(this, createInjector(context));
            };
    }
    if (hook) {
        if (options.functional) {
            // register for functional component in vue file
            const originalRender = options.render;
            options.render = function renderWithStyleInjection(h, context) {
                hook.call(context);
                return originalRender(h, context);
            };
        }
        else {
            // inject component registration as beforeCreate hook
            const existing = options.beforeCreate;
            options.beforeCreate = existing ? [].concat(existing, hook) : [hook];
        }
    }
    return script;
}

/* script */
const __vue_script__$3 = script$3;
/* template */

var __vue_render__$3 = function () {
  var _vm = this;

  var _h = _vm.$createElement;

  var _c = _vm._self._c || _h;

  return _c('label', {
    class: _vm.context.classes.label,
    style: [_vm.tooltip ? {
      'position': 'relative'
    } : {}],
    attrs: {
      "id": _vm.context.id + "_label",
      "for": _vm.context.id
    }
  }, [_c('span', [_vm._v(_vm._s(_vm.context.label))]), _vm._v(" "), _vm.isRequired ? _c('span', {
    staticClass: "sr-only"
  }, [_vm._v(". Obligatoire.")]) : _vm._e(), _vm._v(" "), _vm.context.help ? _c('span', {
    staticClass: "sr-only"
  }, [!_vm.isRequired ? _c('span', [_vm._v(".")]) : _vm._e(), _vm._v(" " + _vm._s(_vm.context.help))]) : _vm._e(), _vm._v(" "), _vm.hasValidationRules && _vm.messagesErreur ? _c('span', {
    staticClass: "sr-only",
    attrs: {
      "aria-live": "polite"
    }
  }, [_vm._v(" " + _vm._s(_vm.messagesErreur))]) : _vm._e(), _vm._v(" "), _vm.isRequired ? _c('span', {
    staticClass: "icone-champ-requis",
    attrs: {
      "aria-hidden": "true"
    }
  }, [_vm._v(_vm._s(_vm.requiredFieldIndicator))]) : _vm._e(), _vm._v(" "), _c('span', {
    staticClass: "conteneur-tooltip"
  }, [_vm.tooltip ? _c('button', {
    staticClass: "tooltip-toggle",
    attrs: {
      "type": "button",
      "data-toggle": "tooltip",
      "title": _vm.tooltip
    }
  }, [_vm._m(0)]) : _vm._e()])]);
};

var __vue_staticRenderFns__$3 = [function () {
  var _vm = this;

  var _h = _vm.$createElement;

  var _c = _vm._self._c || _h;

  return _c('span', {
    staticClass: "conteneur-puce"
  }, [_c('span', {
    staticClass: "puce",
    attrs: {
      "aria-hidden": "true"
    }
  }, [_c('span', {
    staticClass: "icone-svg question",
    attrs: {
      "aria-hidden": "true"
    }
  })])]);
}];
/* style */

const __vue_inject_styles__$3 = undefined;
/* scoped */

const __vue_scope_id__$3 = undefined;
/* module identifier */

const __vue_module_identifier__$3 = undefined;
/* functional template */

const __vue_is_functional_template__$3 = false;
/* style inject */

/* style inject SSR */

/* style inject shadow dom */

const __vue_component__$3 = /*#__PURE__*/normalizeComponent({
  render: __vue_render__$3,
  staticRenderFns: __vue_staticRenderFns__$3
}, __vue_inject_styles__$3, __vue_script__$3, __vue_scope_id__$3, __vue_is_functional_template__$3, __vue_module_identifier__$3, false, undefined, undefined, undefined);

//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
var script$2 = {
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
        this.$root.$el.removeAttribute('data-submit');
        const errorSummary = document.getElementById('errorSummary');

        if (errorSummary) {
          errorSummary.focus();
        }
      }
    });
  },

  computed: {
    errorSummary() {
      return this.visibleErrors.length > 0 ? JSON.parse(this.visibleErrors) : [];
    }

  },
  methods: {
    setInputFocus(name) {
      // setTimeout requis pour que le focus et le scroll se fasse.
      setTimeout(controlName => {
        const errorControl = document.getElementsByName(controlName);

        if (errorControl) {
          this.$root.effectuerNavigationParId(this.obtenirIdPage(errorControl[0]), controlName);
        }
      }, 10, name);
      return false;
    },

    obtenirIdPage(controle) {
      return $(controle).parents('.section:first').attr('data-id-page');
    }

  }
};

/* script */
const __vue_script__$2 = script$2;
/* template */

var __vue_render__$2 = function () {
  var _vm = this;

  var _h = _vm.$createElement;

  var _c = _vm._self._c || _h;

  return _vm.visibleErrors.length > 0 ? _c('div', [_vm.type === 'form' ? _c('div', [_vm.errorSummary.length > 0 ? _c('div', {
    staticClass: "validation-summary-errors",
    attrs: {
      "id": "errorSummary",
      "data-valmsg-summary": "true",
      "tabindex": "-1"
    }
  }, [_c('div', {
    staticClass: "message erreur text-sm"
  }, [_vm._m(0), _vm._v(" "), _c('div', {
    staticClass: "contenu zone-html"
  }, [_c('h2', {
    staticClass: "text-sm"
  }, [_vm._v("Des erreurs sont présentes dans le formulaire")]), _vm._v(" "), _c('ul', {
    class: _vm.outerClass
  }, _vm._l(_vm.errorSummary, function (error, index) {
    return _c('li', {
      key: index,
      class: _vm.itemClass,
      attrs: {
        "role": _vm.role,
        "aria-live": _vm.ariaLive
      }
    }, [_c('a', {
      attrs: {
        "href": "#"
      },
      on: {
        "click": function ($event) {
          return _vm.setInputFocus(error.name);
        }
      }
    }, [_vm._v(_vm._s(error.message))])]);
  }), 0)])])]) : _vm._e()]) : _c('div', [_c('ul', {
    class: _vm.outerClass,
    attrs: {
      "aria-hidden": "true"
    }
  }, _vm._l(_vm.visibleErrors, function (error) {
    return _c('li', {
      key: error,
      class: _vm.itemClass,
      attrs: {
        "role": _vm.role,
        "aria-live": _vm.ariaLive
      },
      domProps: {
        "textContent": _vm._s(error)
      }
    });
  }), 0)])]) : _vm._e();
};

var __vue_staticRenderFns__$2 = [function () {
  var _vm = this;

  var _h = _vm.$createElement;

  var _c = _vm._self._c || _h;

  return _c('div', {
    staticClass: "entete d-flex"
  }, [_c('div', {
    staticClass: "icone-svg",
    attrs: {
      "aria-hidden": "true"
    }
  })]);
}];
/* style */

const __vue_inject_styles__$2 = undefined;
/* scoped */

const __vue_scope_id__$2 = undefined;
/* module identifier */

const __vue_module_identifier__$2 = undefined;
/* functional template */

const __vue_is_functional_template__$2 = false;
/* style inject */

/* style inject SSR */

/* style inject shadow dom */

const __vue_component__$2 = /*#__PURE__*/normalizeComponent({
  render: __vue_render__$2,
  staticRenderFns: __vue_staticRenderFns__$2
}, __vue_inject_styles__$2, __vue_script__$2, __vue_scope_id__$2, __vue_is_functional_template__$2, __vue_module_identifier__$2, false, undefined, undefined, undefined);

//
//
//
//
//
//
//
//
//
//
//
//
var script$1 = {
  props: {
    index: {
      type: Number,
      default: null
    },
    context: {
      type: Object,
      required: true
    },
    removeItem: {
      type: Function,
      required: true
    }
  }
};

/* script */
const __vue_script__$1 = script$1;
/* template */

var __vue_render__$1 = function () {
  var _vm = this;

  var _h = _vm.$createElement;

  var _c = _vm._self._c || _h;

  return _vm.context.repeatable ? _c('button', {
    staticClass: "btn btn-secondaire",
    class: _vm.context.classes.groupRepeatableRemove,
    attrs: {
      "data-disabled": _vm.context.model.length <= _vm.context.minimum
    },
    domProps: {
      "textContent": _vm._s(_vm.context.removeLabel)
    },
    on: {
      "click": function ($event) {
        $event.preventDefault();
        return _vm.removeItem($event);
      },
      "keypress": function ($event) {
        if (!$event.type.indexOf('key') && _vm._k($event.keyCode, "enter", 13, $event.key, "Enter")) {
          return null;
        }

        return _vm.removeItem($event);
      }
    }
  }) : _vm._e();
};

var __vue_staticRenderFns__$1 = [];
/* style */

const __vue_inject_styles__$1 = undefined;
/* scoped */

const __vue_scope_id__$1 = undefined;
/* module identifier */

const __vue_module_identifier__$1 = undefined;
/* functional template */

const __vue_is_functional_template__$1 = false;
/* style inject */

/* style inject SSR */

/* style inject shadow dom */

const __vue_component__$1 = /*#__PURE__*/normalizeComponent({
  render: __vue_render__$1,
  staticRenderFns: __vue_staticRenderFns__$1
}, __vue_inject_styles__$1, __vue_script__$1, __vue_scope_id__$1, __vue_is_functional_template__$1, __vue_module_identifier__$1, false, undefined, undefined, undefined);

//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
var script = {
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
};

/* script */
const __vue_script__ = script;
/* template */

var __vue_render__ = function () {
  var _vm = this;

  var _h = _vm.$createElement;

  var _c = _vm._self._c || _h;

  return _c('div', {
    class: [_vm.context.classes.groupRepeatable]
  }, [_c('div', {
    staticClass: "contenu-extensible instance-groupe card section-secondaire sans-bordure"
  }, [_c('div', {
    staticClass: "card-title "
  }, [_c('a', {
    attrs: {
      "role": "button",
      "data-toggle": "collapse",
      "aria-expanded": "true",
      "href": '#' + _vm.context.name + '-' + _vm.index,
      "aria-controls": _vm.context.name + '-' + _vm.index
    }
  }, [_c('span', {
    staticClass: "libelle",
    attrs: {
      "id": 'label' + _vm.context.name + '-' + _vm.index
    }
  }, [_c('span', {
    staticClass: "libelle-instance"
  }, [_vm._v(_vm._s(_vm.instanceLabel) + " ")]), _vm._v(" "), this.context.repeatable ? _c('span', {
    staticClass: "numero"
  }, [_vm._v(_vm._s(_vm.index + 1))]) : _vm._e()]), _vm._v(" "), _c('span', {
    staticClass: "icone-svg md chevron-haut-texte",
    attrs: {
      "aria-hidden": "true"
    }
  })])]), _vm._v(" "), _c('div', {
    staticClass: "collapse show",
    attrs: {
      "role": "group",
      "aria-labelledby": 'label' + _vm.context.name + '-' + _vm.index,
      "id": _vm.context.name + '-' + _vm.index
    }
  }, [_c('div', {
    staticClass: "card-body"
  }, [_vm.context.removePosition === 'after' ? _vm._t("default") : _vm._e(), _vm._v(" "), _c('FormulateSlot', {
    attrs: {
      "name": "remove",
      "context": _vm.context,
      "index": _vm.index,
      "remove-item": _vm.removeItem
    }
  }, [_c(_vm.context.slotComponents.remove, _vm._b({
    tag: "component",
    attrs: {
      "context": _vm.context,
      "index": _vm.index,
      "remove-item": _vm.removeItem
    }
  }, 'component', _vm.context.slotProps.remove, false))], 1), _vm._v(" "), _vm.context.removePosition === 'before' ? _vm._t("default") : _vm._e()], 2)])])]);
};

var __vue_staticRenderFns__ = [];
/* style */

const __vue_inject_styles__ = undefined;
/* scoped */

const __vue_scope_id__ = undefined;
/* module identifier */

const __vue_module_identifier__ = undefined;
/* functional template */

const __vue_is_functional_template__ = false;
/* style inject */

/* style inject SSR */

/* style inject shadow dom */

const __vue_component__ = /*#__PURE__*/normalizeComponent({
  render: __vue_render__,
  staticRenderFns: __vue_staticRenderFns__
}, __vue_inject_styles__, __vue_script__, __vue_scope_id__, __vue_is_functional_template__, __vue_module_identifier__, false, undefined, undefined, undefined);

/* eslint-disable import/prefer-default-export */

var components = /*#__PURE__*/Object.freeze({
    __proto__: null,
    Label: __vue_component__$3,
    ErrorList: __vue_component__$2,
    RepeatableRemove: __vue_component__$1,
    Repeatable: __vue_component__
});

// Import vue components

const install = function installMtessVueformulate(Vue) {
  Object.entries(components).forEach(([componentName, component]) => {
    Vue.component(componentName, component);
  });
}; // Create module definition for Vue.use()

export default install;
export { __vue_component__$2 as ErrorList, __vue_component__$3 as Label, __vue_component__ as Repeatable, __vue_component__$1 as RepeatableRemove };
