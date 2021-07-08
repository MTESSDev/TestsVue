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

    <div v-if="form.EtatRevision === ''">
        <div class="traitement-en-cours md text-center" tabindex="0" role="status">
            <svg class="spinner" viewBox="0 0 50 50" aria-hidden="true"><circle class="path" cx="25" cy="25" r="20" fill="none"></circle></svg>
        </div>
    </div>

    {{# Form.sectionsGroup}}
    <div class="sectionGroup" {{# v-if }} v-if="{{.}}" {{/ v-if}}>
        {{# sections}}
        <div {{# v-if}} v-if="{{.}}" {{/ v-if}}>
            <div v-if="form.validAll === true || pageCourante.id == '{{prefixId}}{{id}}'">
                <div class="section {{classes}}" data-id-page="{{prefixId}}{{id}}"
                     v-show="pageCourante.id == '{{prefixId}}{{id}}'">
                    {{RecursiveComponents components}}
                </div>
            </div>
        </div>
        {{/ sections}}
    </div>
    {{/ Form.sectionsGroup}}

    <div v-if="pageCourante.id === 'revision'">
        <div v-if="form.EtatRevision === 'initial'" class="mt-32">
            <formulate-input type="submit"
                             label="Valider"
                             :input-class="['btn', 'btn-primaire', 'bouton-validation']"></formulate-input>
        </div>
        <div v-if="form.EtatRevision === 'sans-erreur'" class="mt-32">
            <formulate-input type="submit"
                             label="Soumettre"
                             :input-class="['btn', 'btn-primaire', 'bouton-validation']"></formulate-input>
        </div>

    </div>
    {{! ceci est un commentaire, pour dire que le }}
    {{! block ci-dessous sert à forcer stubble à skipper form }}
    {{=<% %>=}}
    <%={{ }}=%>
</formulate-form>