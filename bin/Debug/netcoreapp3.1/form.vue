<formulate-form v-model="contenuform" 
                @submit="submitHandler" 
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                @failed-validation="failedValidation"
                :errors="inputErrors">

    <formulate-input   type="group"
                        name="flavors"
                        validation-name="La saveur"
                        label="Nom de famille et prénom selon le certificat de naissance ou le document d’immigration"
                        help="Entrez votre nom de famille et prénom selon le certificat de naissance ou le document d’immigration.
                              Si vous utilisez habituellement un autre nom que celui qui figure sur votre 
                              certificat de naissance, ou si vous vous êtes marié avant le 2 avril 1981 et que 
                              vous portez le nom de votre conjoint ou les deux noms combinés, ajoutez-le en utilisant le bouton «Ajouter un autre nom»."
                        add-label="+ Ajouter un autre nom usuel"
                        validation="required|max:2"
                        limit="2"
                        :repeatable="true" >
 
        <div class="order">
            <formulate-input type="text"
                             name="nom"
                             label="Nom de famille"
                             validation="required"
                             validation-name="autre nom"></formulate-input>

            <formulate-input type="text"
                             name="prenom"
                             label="Prénom"
                             validation="required"></formulate-input>


        </div>
    

        </formulate-input>

            <formulate-input type="date"
                         name="dateNaissance"
                         label="Date de naissance"
                         validation="required"
                         min="1930-01-01"
                         max="2021-02-13"></formulate-input>

    <formulate-input v-if="contenuform.planet === 'earth'"
                        key="earth"
                        name="earth_moon"
                        dotnetcustom="yeah"
                        validation="^required|min:4,length|matches:/[0-9]/"
                        :validation-messages="{
                            required: 'Le nom de la lune est requis.'
                        }"
                        label="What is the name of earth’s moon?"></formulate-input>

    <formulate-input  v-if="contenuform.planet === 'mars'"
                        key="mars"
                        name="mars_sunset"
                        label="What color is the Martian sunset?"></formulate-input>

    <formulate-input   type="group"
                        name="flavors"
                        validation-name="La saveur"
                        label="Create your custom order"
                        help="Choose your hand-packed pints whipped up by our expert servers"
                        add-label="+ Add Flavor"
                        validation="required"
                        :repeatable="true" >
        <div class="order">
            <formulate-input name="flavor" validation-name="La saveur" type="select" label="Saveur" validation="required" :options="{ vanilla: 'Vanilla', chocolate: 'Chocolate', strawberry: 'Strawberry', pineapple: 'Pineapple'}" ></formulate-input>
            <formulate-input name="quantity" validation-name="La quantité" label="Quantité" type="number" min="1" validation="required|min:1" ></formulate-input>
        </div>
    </formulate-input>

       <template>
       {{ inputErrors  }}
    </template>

    <template  v-if="contenuform.planet !== 'earth'">
        <div>page 3</div>
    </template>

    <formulate-input type="submit"
                     label="Register"></formulate-input>
</formulate-form>