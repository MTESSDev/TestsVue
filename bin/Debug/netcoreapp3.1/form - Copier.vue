<formulate-form v-model="contenuform" 
                @submit="submitHandler" 
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                @failed-validation="failedValidation"
                :errors="inputErrors">

    <formulate-input type="select"
                     name="planet"
                     label="What is your favorite rocky planet?"
                     validation="required"
                     pdf-field="planet_page1"
                     validation-name="Planète"
                     :options="{ mercury: 'Mercury', venus: 'Venus', earth: 'Earth', mars: 'Mars' }"></formulate-input>

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