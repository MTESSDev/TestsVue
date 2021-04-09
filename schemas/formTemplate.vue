<formulate-form name="form"
                v-model="contenuform"
                @submit="submitHandler"
                @submit-raw="submitRawHandler"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                :keep-model-data="config.keepData"
                @failed-validation="failedValidation"
                :errors="inputErrors">

    {{# Form.Sections}}
    <div v-if="contenuform.validAll === true || contenuform.idPageCourante == '{{Id}}'">
        <div class="section {{Classes}}"
             v-show="contenuform.idPageCourante == '{{Id}}' && contenuform.validAll === false">
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