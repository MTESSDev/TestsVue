# TestsVue

Un projet de formulaires 100% dynamiques, YAML first.

## Technologies
- Vue.js
- [Vue Formulate](https://github.com/wearebraid/vue-formulate)
- .net Core 3.1 
- [Stubble](https://github.com/StubbleOrg/Stubble)
- YAML

## Comment ça marche?
1- Côté serveur la lecture du fichier de config YAML s'effectue
2- Le fichier YAML est convertis en objet, et traité par Stubble
3- Stubble génère alors le code "vue-formulate" (tous les formulate-input) et les différentes sections HTML
4- Le code est chargé côté client et interprété par vue/vue-formulate
5- Les validations s'effectues selon les différentes modalitées de vue-formulate
6- À venir - Traitement côté serveur des validations
6- À venir - binding des valeurs vers un PDF
