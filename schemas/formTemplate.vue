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
    <div class="section" v-show="contenuform.page === '{{Id}}'">
        <h2>{{i18n section}}</h2>
        {{#MessageUrgent}}<div>{{.}}</div>{{/MessageUrgent}}
        {{#InputsHandled}}
        {{#attributes}}
        <formulate-input {{# v-if}} v-if="{{{.}}}" {{/ v-if}}
                         {{# type}} type="{{.}}" {{/ type}}
                         {{# name}} name="{{.}}" {{/ name}}
                         {{# validation-name}} validation-name="{{i18n .}}" {{/ validation-name}}
                         {{# label}} label="{{i18n .}}" {{/ label}}
                         {{# help}} help="{{i18n .}}" {{/ help}}
                         {{# addLabel}} add-label="{{i18n .}}" {{/ addLabel}}
                         {{# validations}} :validation="{{JsArray .}}" {{/ validations}}
                         {{# min}} min="{{.}}" {{/ min}}
                         {{# max}} max="{{.}}" {{/ max}}
                         {{# limit}} limit="{{.}}" {{/ limit}}
                         {{# options}} :options="{{JsObject options}}" {{/ options}}
                         {{# repeatable}}
                         :repeatable="{{#.}}true{{/.}}{{^.}}false{{/.}}"
                         {{/ repeatable}}>
            {{#isGroup}}
            <div class="group">
                {{#inputs}}
                <formulate-input {{# v-if}} v-if="{{{.}}}" {{/ v-if}}
                                 {{# type}} type="{{.}}" {{/ type}}
                                 {{# name}} name="{{.}}" {{/ name}}
                                 {{# validation-name}} validation-name="{{i18n .}}" {{/ validation-name}}
                                 {{# label}} label="{{i18n .}}" {{/ label}}
                                 {{# help}} help="{{i18n .}}" {{/ help}}
                                 {{# validations}} :validation="{{JsArray .}}" {{/ validations}}
                                 {{# min}} min="{{.}}" {{/ min}}
                                 {{# max}} max="{{.}}" {{/ max}}
                                 {{# options}} :options="{{JsObject options}}" {{/ options}}></formulate-input>
                {{/inputs}}
            </div>
            {{/isGroup}}
        </formulate-input>
        {{/attributes}}
        {{/InputsHandled}}
    </div>
    {{/Form.Sections}}

    <formulate-input type="submit"
                     label="Register"></formulate-input>
    {{! ceci est un commentaire, pour dire que le }}
    {{! block ci-dessous sert à forcer stubble à skipper contenuform }}
    {{=<% %>=}}
    {{contenuform}}
    <%={{ }}=%>
</formulate-form>