<formulate-form name="form"
                v-model="form"
                @submit="submitHandler"
                @submit-raw="submitRawHandler"
                @created="created"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                :keep-model-data="config.keepData"
                @failed-validation="failedValidation"
                :errors="inputErrors">

    {{# Form.sections}}
    <div {{# v-if}}v-if="{{.}}"{{/ v-if}}>
        <div v-if="form.validAll === true || pageCourante.id == '{{id}}'">
            <div class="section {{classes}}" data-id-page="{{id}}"
                 v-show="pageCourante.id == '{{id}}' && form.validAll === false">
                {{RecursiveComponents components}}
            </div>
        </div>
    </div>
    {{/ Form.sections}}

    <formulate-input type="submit"
                     v-if="pageCourante.id === 'revision'"
                     label="Soumettre"
                     :input-class="['btn', 'btn-primaire']"></formulate-input>
    {{! ceci est un commentaire, pour dire que le }}
    {{! block ci-dessous sert à forcer stubble à skipper form }}
    {{=<% %>=}}
    <%={{ }}=%>
</formulate-form>