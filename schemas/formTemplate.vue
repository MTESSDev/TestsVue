<formulate-form v-model="contenuform"
                @submit="submitHandler"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                :keep-model-data="config.keepData"
                @failed-validation="failedValidation"
                :errors="inputErrors">

    <formulate-input name="page"
                     {{GenerateInputClasses "select" .}}
                     :options="{ {{# Form.Sections}}{{Id}}:'{{i18n Section}}', {{/ Form.Sections}} }"
                     type="select"></formulate-input>


    <formulate-input name="validAll"
                     type="checkbox"
                     label="Valider tout"></formulate-input>

    {{# Form.Sections}}
    <div v-if="contenuform.validAll === true || contenuform.page == '{{Id}}'">
        <div class="section {{Classes}}"
             v-show="contenuform.page == '{{Id}}' && contenuform.validAll === false">
            <h2>{{i18n section}}</h2>

            {{RecursiveComponents components}}

        </div>
    </div>
    {{/ Form.Sections}}

    <formulate-input type="submit"
                     label="Soumettre"
                     :input-class="['btn', 'btn-primaire']"></formulate-input>
    {{! ceci est un commentaire, pour dire que le }}
    {{! block ci-dessous sert à forcer stubble à skipper contenuform }}
    {{=<% %>=}}
    {{contenuform}}
    <%={{ }}=%>
</formulate-form>