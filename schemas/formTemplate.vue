<formulate-form name="form"
                v-model="form"
                @submit="submitHandler"
                @submit-raw="submitRawHandler"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                :keep-model-data="config.keepData"
                @failed-validation="failedValidation"
                :errors="inputErrors">

    {{# Form.Sections}}
    <div v-if="form.validAll === true || form.idPageCourante == '{{Id}}'">
        <div class="section {{Classes}}"
             v-show="form.idPageCourante == '{{Id}}' && form.validAll === false">
            {{RecursiveComponents components}}
        </div>
    </div>
    {{/ Form.Sections}}

    <formulate-input type="button"
                     @click="sauvegarde"
                     label="Sauvegarder"
                     :input-class="['btn', 'btn-secondaire']"></formulate-input>

    <formulate-input type="submit"
                     v-if="form.idPageCourante === 'revision'"
                     label="Soumettre"
                     :input-class="['btn', 'btn-primaire']"></formulate-input>
    {{! ceci est un commentaire, pour dire que le }}
    {{! block ci-dessous sert à forcer stubble à skipper form }}
    {{=<% %>=}}
    {{form}}
    <%={{ }}=%>
</formulate-form>