<formulate-form v-model="contenuform"
                @submit="submitHandler"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                @failed-validation="failedValidation"
                :errors="inputErrors">

    <!--<formulate-input type="group"
                     name="noms"
                     validation-name="La saveur"
                     label="Nom de famille et prénom selon le certificat de naissance ou le document d’immigration"
                     help="Entrez votre nom de famille et prénom selon le certificat de naissance ou le document d’immigration.
                              Si vous utilisez habituellement un autre nom que celui qui figure sur votre
                              certificat de naissance, ou si vous vous êtes marié avant le 2 avril 1981 et que
                              vous portez le nom de votre conjoint ou les deux noms combinés, ajoutez-le en utilisant le bouton «Ajouter un autre nom»."
                     add-label="+ Ajouter un autre nom usuel"
                     validation="required|max:2"
                     limit="2"
                     :repeatable="true">

        <div class="order">
            <formulate-input type="text"
                             name="nom"
                             label="Nom de famille"
                             validation="required"
                             error-behavior="value"
                             validation-name="autre nom"></formulate-input>

            <formulate-input type="text"
                             name="prenom"
                             label="Prénom"
                             validation="required"></formulate-input>


        </div>
    </formulate-input>-->
    <formulate-input type="select"
                     name="page"
                     label="Page"
                     :options="{un: 'un', deux: 'deux'}">
    </formulate-input>

    <div v-show="contenuform.page === 'un'">
        <formulate-input type="radio"
                         name="ne_qc"
                         label="Êtes-vous né au Québec?"
                         validation="required"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>
        <formulate-input v-if="contenuform.ne_qc === 'oui'"
                         type="text"
                         key="nom_parent"
                         name="nom_parent"
                         validation="required"
                         label="Inscrivez le nom de famille et le prénom d'un de vos parents:">
        </formulate-input>
        <formulate-input v-if="contenuform.ne_qc === 'non'"
                         type="radio"
                         key="citoyen_canadien"
                         name="citoyen_canadien"
                         validation="required"
                         label="Avez-vous la citoyenneté canadienne?"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>
        <formulate-input v-if="contenuform.citoyen_canadien === 'non'"
                         type="radio"
                         key="resident_permanent"
                         name="resident_permanent"
                         validation="required"
                         label="Êtes-vous résident permanent?"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>
        <formulate-input v-if="contenuform.resident_permanent === 'non'"
                         type="radio"
                         key="demandeur_asile"
                         name="demandeur_asile"
                         validation="required"
                         label="Êtes-vous un demandeur d'asile?"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>



        <formulate-input v-if="contenuform.demandeur_asile === 'non'"
                         type="radio"
                         key="refugie_protege"
                         name="refugie_protege"
                         validation="required"
                         label="Avez-vous obtenu le statut de réfugié ou de personne protégée ou à protéger?"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>
    </div>

    <div v-show="contenuform.page === 'deux'">
        Rien encore :(
    </div>
    <!--<template v-if="contenuform.planet !== 'earth'">
        <div>page 3</div>
    </template>-->

    <formulate-input type="submit"
                     label="Register"></formulate-input>


    <template>
        {{ contenuform  }}
    </template>
</formulate-form>