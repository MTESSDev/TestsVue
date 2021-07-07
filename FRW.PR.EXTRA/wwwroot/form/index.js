Vue.use(VueFormulate)

const App = {
  el: '#form',

data: {
  values: {},
  schema: [
    {
      "component": "h3",
      "children": "Order pizza"
    },
    {
      "type": "select",
      "label": "Pizza size",
      "name": "size",
      "placeholder": "Select a size",
      "options": {
        "small": "Small",
        "large": "Large",
        "extra_large": "Extra Large"
      },
      "validation": "required"
    },
    {
      "component": "div",
      "class": "flex-wrapper",
      "children": [
        {
          "name": "cheese",
          "label": "Cheese options",
          "type": "checkbox",
          "options": {
            "mozzarella": "Mozzarella",
            "feta": "Feta",
            "parmesan": "Parmesan",
            "extra": "Extra cheese"
          }
        },
        {
          "name": "toppings",
          "label": "Toppings",
          "type": "checkbox",
          "validation": "required|max:2",
          "options": {
            "salami": "Salami",
            "prosciutto": "Prosciutto",
            "avocado": "Avocado",
            "onion": "Onion"
          }
        }
      ]
    },
    {
      "component": "div",
      "class": "flex-wrapper",
      "children": [
        {
          "type": "select",
          "name": "country_code",
          "label": "Code",
          "outer-class": ["flex-item-small"],
          "value": "1",
          "options": {
            "1": "+1",
            "49": "+49",
            "55": "+55"
          }
        },
        {
          "type": "text",
          "label": "Phone number",
          "name": "phone",
          "inputmode": "numeric",
          "pattern": "[0-9]*",
          "validation": "matches:/^[0-9-]+$/",
          "validation-messages": {
            "matches": "Phone number should only include numbers and dashes."
          }
        }
      ]
    },
  
    {
      "type": "submit",
      "label": "Order pizza"
    }
  ]
},

  mounted () {
    console.log('Application mounted.')
  }
}


window.addEventListener('load', () => {
  new Vue(App)
})
