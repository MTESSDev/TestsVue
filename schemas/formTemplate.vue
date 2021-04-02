<formulate-form v-model="contenuform"
                @submit="submitHandler"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                :debounce="10"
                @failed-validation="failedValidation"
                :errors="inputErrors">

    <formulate-input name="page"
                     :options="{ {{# Form.Sections}}{{Id}}:'{{i18n Section}}', {{/ Form.Sections}} }"
                     type="select"></formulate-input>

    {{# Form.Sections}}
    <div class="section" v-if="contenuform.page === '{{Id}}'">
        <h2>{{i18n section}}</h2>

        {{RecursiveComponents components}}

    </div>
    {{/ Form.Sections}}

    <formulate-input type="submit"
                     label="Register"></formulate-input>
    {{! ceci est un commentaire, pour dire que le }}
    {{! block ci-dessous sert à forcer stubble à skipper contenuform }}

    {{& contenuform}}
</formulate-form>