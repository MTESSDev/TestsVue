'use strict';function _slicedToArray(arr, i) {
  return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _unsupportedIterableToArray(arr, i) || _nonIterableRest();
}

function _arrayWithHoles(arr) {
  if (Array.isArray(arr)) return arr;
}

function _iterableToArrayLimit(arr, i) {
  if (typeof Symbol === "undefined" || !(Symbol.iterator in Object(arr))) return;
  var _arr = [];
  var _n = true;
  var _d = false;
  var _e = undefined;

  try {
    for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) {
      _arr.push(_s.value);

      if (i && _arr.length === i) break;
    }
  } catch (err) {
    _d = true;
    _e = err;
  } finally {
    try {
      if (!_n && _i["return"] != null) _i["return"]();
    } finally {
      if (_d) throw _e;
    }
  }

  return _arr;
}

function _unsupportedIterableToArray(o, minLen) {
  if (!o) return;
  if (typeof o === "string") return _arrayLikeToArray(o, minLen);
  var n = Object.prototype.toString.call(o).slice(8, -1);
  if (n === "Object" && o.constructor) n = o.constructor.name;
  if (n === "Map" || n === "Set") return Array.from(o);
  if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen);
}

function _arrayLikeToArray(arr, len) {
  if (len == null || len > arr.length) len = arr.length;

  for (var i = 0, arr2 = new Array(len); i < len; i++) arr2[i] = arr[i];

  return arr2;
}

function _nonIterableRest() {
  throw new TypeError("Invalid attempt to destructure non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
}//
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
    messagesErreur: function messagesErreur() {
      return this.context.visibleValidationErrors.join(' ');
    }
  },
  created: function created() {
    this.hasValidationRules = this.context.rules.length > 0;
    this.isRequired = this.context.rules.findIndex(function (element) {
      return element.ruleName === 'required';
    }) >= 0;
    this.requiredFieldIndicator = this.isRequired ? '*' : ''; // Ne pas lire la pr�cision au lecteur �cran car nous l'avons ajout�e dans le label.

    if (this.context.help) {
      this.$nextTick(function () {
        var help = this.$parent.$el.querySelector(".formulate-input-help");

        if (help) {
          help.setAttribute('aria-hidden', 'true');
        }
      });
    }
  }
};function normalizeComponent(template, style, script, scopeId, isFunctionalTemplate, moduleIdentifier /* server only */, shadowMode, createInjector, createInjectorSSR, createInjectorShadow) {
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
}/* script */
var __vue_script__$3 = script$3;
/* template */

var __vue_render__$3 = function __vue_render__() {
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
  }, [_vm._ssrNode("<span>" + _vm._ssrEscape(_vm._s(_vm.context.label)) + "</span> " + (_vm.isRequired ? "<span class=\"sr-only\">. Obligatoire.</span>" : "<!---->") + " " + (_vm.context.help ? "<span class=\"sr-only\">" + (!_vm.isRequired ? "<span>.</span>" : "<!---->") + _vm._ssrEscape(" " + _vm._s(_vm.context.help)) + "</span>" : "<!---->") + " " + (_vm.hasValidationRules && _vm.messagesErreur ? "<span aria-live=\"polite\" class=\"sr-only\">" + _vm._ssrEscape(" " + _vm._s(_vm.messagesErreur)) + "</span>" : "<!---->") + " " + (_vm.isRequired ? "<span aria-hidden=\"true\" class=\"icone-champ-requis\">" + _vm._ssrEscape(_vm._s(_vm.requiredFieldIndicator)) + "</span>" : "<!---->") + " <span class=\"conteneur-tooltip\">" + (_vm.tooltip ? "<button type=\"button\" data-toggle=\"tooltip\"" + _vm._ssrAttr("title", _vm.tooltip) + " class=\"tooltip-toggle\"><span class=\"conteneur-puce\"><span aria-hidden=\"true\" class=\"puce\"><span aria-hidden=\"true\" class=\"icone-svg question\"></span></span></span></button>" : "<!---->") + "</span>")]);
};

var __vue_staticRenderFns__$3 = [];
/* style */

var __vue_inject_styles__$3 = undefined;
/* scoped */

var __vue_scope_id__$3 = undefined;
/* module identifier */

var __vue_module_identifier__$3 = "data-v-39371b2f";
/* functional template */

var __vue_is_functional_template__$3 = false;
/* style inject */

/* style inject SSR */

/* style inject shadow dom */

var __vue_component__$3 = /*#__PURE__*/normalizeComponent({
  render: __vue_render__$3,
  staticRenderFns: __vue_staticRenderFns__$3
}, __vue_inject_styles__$3, __vue_script__$3, __vue_scope_id__$3, __vue_is_functional_template__$3, __vue_module_identifier__$3, false, undefined, undefined, undefined);//
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
  updated: function updated() {
    this.$nextTick(function () {
      if (this.type === 'form' && this.$root.$el.getAttribute('data-submit')) {
        this.$root.$el.removeAttribute('data-submit');
        var errorSummary = document.getElementById('errorSummary');

        if (errorSummary) {
          errorSummary.focus();
        }
      }
    });
  },
  computed: {
    errorSummary: function errorSummary() {
      return this.visibleErrors.length > 0 ? JSON.parse(this.visibleErrors) : [];
    }
  },
  methods: {
    setInputFocus: function setInputFocus(name) {
      var _this = this;

      // setTimeout requis pour que le focus et le scroll se fasse.
      setTimeout(function (controlName) {
        var errorControl = document.getElementsByName(controlName);

        if (errorControl) {
          _this.$root.effectuerNavigationParId(_this.obtenirIdPage(errorControl[0]), controlName);
        }
      }, 10, name);
      return false;
    },
    obtenirIdPage: function obtenirIdPage(controle) {
      return $(controle).parents('.section:first').attr('data-id-page');
    }
  }
};/* script */
var __vue_script__$2 = script$2;
/* template */

var __vue_render__$2 = function __vue_render__() {
  var _vm = this;

  var _h = _vm.$createElement;

  var _c = _vm._self._c || _h;

  return _vm.visibleErrors.length > 0 ? _c('div', [_vm._ssrNode(_vm.type === 'form' ? "<div>" + (_vm.errorSummary.length > 0 ? "<div id=\"errorSummary\" data-valmsg-summary=\"true\" tabindex=\"-1\" class=\"validation-summary-errors\"><div class=\"message erreur text-sm\"><div class=\"entete d-flex\"><div aria-hidden=\"true\" class=\"icone-svg\"></div></div> <div class=\"contenu zone-html\"><h2 class=\"text-sm\">Des erreurs sont présentes dans le formulaire</h2> <ul" + _vm._ssrClass(null, _vm.outerClass) + ">" + _vm._ssrList(_vm.errorSummary, function (error, index) {
    return "<li" + _vm._ssrAttr("role", _vm.role) + _vm._ssrAttr("aria-live", _vm.ariaLive) + _vm._ssrClass(null, _vm.itemClass) + "><a href=\"#\">" + _vm._ssrEscape(_vm._s(error.message)) + "</a></li>";
  }) + "</ul></div></div></div>" : "<!---->") + "</div>" : "<div><ul aria-hidden=\"true\"" + _vm._ssrClass(null, _vm.outerClass) + ">" + _vm._ssrList(_vm.visibleErrors, function (error) {
    return "<li" + _vm._ssrAttr("role", _vm.role) + _vm._ssrAttr("aria-live", _vm.ariaLive) + _vm._ssrClass(null, _vm.itemClass) + ">" + _vm._ssrEscape(_vm._s(error)) + "</li>";
  }) + "</ul></div>")]) : _vm._e();
};

var __vue_staticRenderFns__$2 = [];
/* style */

var __vue_inject_styles__$2 = undefined;
/* scoped */

var __vue_scope_id__$2 = undefined;
/* module identifier */

var __vue_module_identifier__$2 = "data-v-7ada0d93";
/* functional template */

var __vue_is_functional_template__$2 = false;
/* style inject */

/* style inject SSR */

/* style inject shadow dom */

var __vue_component__$2 = /*#__PURE__*/normalizeComponent({
  render: __vue_render__$2,
  staticRenderFns: __vue_staticRenderFns__$2
}, __vue_inject_styles__$2, __vue_script__$2, __vue_scope_id__$2, __vue_is_functional_template__$2, __vue_module_identifier__$2, false, undefined, undefined, undefined);//
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
};/* script */
var __vue_script__$1 = script$1;
/* template */

var __vue_render__$1 = function __vue_render__() {
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
      "click": function click($event) {
        $event.preventDefault();
        return _vm.removeItem($event);
      },
      "keypress": function keypress($event) {
        if (!$event.type.indexOf('key') && _vm._k($event.keyCode, "enter", 13, $event.key, "Enter")) {
          return null;
        }

        return _vm.removeItem($event);
      }
    }
  }, []) : _vm._e();
};

var __vue_staticRenderFns__$1 = [];
/* style */

var __vue_inject_styles__$1 = undefined;
/* scoped */

var __vue_scope_id__$1 = undefined;
/* module identifier */

var __vue_module_identifier__$1 = "data-v-5255d467";
/* functional template */

var __vue_is_functional_template__$1 = false;
/* style inject */

/* style inject SSR */

/* style inject shadow dom */

var __vue_component__$1 = /*#__PURE__*/normalizeComponent({
  render: __vue_render__$1,
  staticRenderFns: __vue_staticRenderFns__$1
}, __vue_inject_styles__$1, __vue_script__$1, __vue_scope_id__$1, __vue_is_functional_template__$1, __vue_module_identifier__$1, false, undefined, undefined, undefined);//
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
};/* script */
var __vue_script__ = script;
/* template */

var __vue_render__ = function __vue_render__() {
  var _vm = this;

  var _h = _vm.$createElement;

  var _c = _vm._self._c || _h;

  return _c('div', {
    class: [_vm.context.classes.groupRepeatable]
  }, [_vm._ssrNode("<div class=\"contenu-extensible instance-groupe card section-secondaire sans-bordure\">", "</div>", [_vm._ssrNode("<div class=\"card-title \"><a role=\"button\" data-toggle=\"collapse\" aria-expanded=\"true\"" + _vm._ssrAttr("href", '#' + _vm.context.name + '-' + _vm.index) + _vm._ssrAttr("aria-controls", _vm.context.name + '-' + _vm.index) + "><span" + _vm._ssrAttr("id", 'label' + _vm.context.name + '-' + _vm.index) + " class=\"libelle\"><span class=\"libelle-instance\">" + _vm._ssrEscape(_vm._s(_vm.instanceLabel) + " ") + "</span> <span class=\"numero\">" + _vm._ssrEscape(_vm._s(_vm.index + 1)) + "</span></span> <span aria-hidden=\"true\" class=\"icone-svg md chevron-haut-texte\"></span></a></div> "), _vm._ssrNode("<div role=\"group\"" + _vm._ssrAttr("aria-labelledby", 'label' + _vm.context.name + '-' + _vm.index) + _vm._ssrAttr("id", _vm.context.name + '-' + _vm.index) + " class=\"collapse show\">", "</div>", [_vm._ssrNode("<div class=\"card-body\">", "</div>", [_vm.context.removePosition === 'after' ? _vm._t("default") : _vm._e(), _vm._ssrNode(" "), _c('FormulateSlot', {
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
  }, 'component', _vm.context.slotProps.remove, false))], 1), _vm._ssrNode(" "), _vm.context.removePosition === 'before' ? _vm._t("default") : _vm._e()], 2)])], 2)]);
};

var __vue_staticRenderFns__ = [];
/* style */

var __vue_inject_styles__ = undefined;
/* scoped */

var __vue_scope_id__ = undefined;
/* module identifier */

var __vue_module_identifier__ = "data-v-23c3adb0";
/* functional template */

var __vue_is_functional_template__ = false;
/* style inject */

/* style inject SSR */

/* style inject shadow dom */

var __vue_component__ = /*#__PURE__*/normalizeComponent({
  render: __vue_render__,
  staticRenderFns: __vue_staticRenderFns__
}, __vue_inject_styles__, __vue_script__, __vue_scope_id__, __vue_is_functional_template__, __vue_module_identifier__, false, undefined, undefined, undefined);/* eslint-disable import/prefer-default-export */var components$1=/*#__PURE__*/Object.freeze({__proto__:null,Label: __vue_component__$3,ErrorList: __vue_component__$2,RepeatableRemove: __vue_component__$1,Repeatable: __vue_component__});var install = function installMtessVueformulate(Vue) {
  Object.entries(components$1).forEach(function (_ref) {
    var _ref2 = _slicedToArray(_ref, 2),
        componentName = _ref2[0],
        component = _ref2[1];

    Vue.component(componentName, component);
  });
}; // Create module definition for Vue.use()
var components=/*#__PURE__*/Object.freeze({__proto__:null,'default': install,Label: __vue_component__$3,ErrorList: __vue_component__$2,RepeatableRemove: __vue_component__$1,Repeatable: __vue_component__});// only expose one global var, with component exports exposed as properties of
// that global var (eg. plugin.component)

Object.entries(components).forEach(function (_ref) {
  var _ref2 = _slicedToArray(_ref, 2),
      componentName = _ref2[0],
      component = _ref2[1];

  if (componentName !== 'default') {
    install[componentName] = component;
  }
});module.exports=install;