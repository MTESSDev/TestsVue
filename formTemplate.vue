<formulate-form v-model="contenuform"
                @submit="submitHandler"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                @failed-validation="failedValidation"
                :errors="inputErrors">
    {{#Form.Sections}}
    <div class="section">
        <h2>{{i18n section}}</h2>
        {{#MessageUrgent}}<div>{{.}}</div>{{/MessageUrgent}}
        {{#InputsHandled}}
        {{#attributes}}
        <formulate-input {{# v-if}} v-if="{{{.}}}" {{/ v-if}}
                         {{# type}} type="{{.}}" {{/ type}}
                         {{# name}} name="{{.}}" {{/ name}}
                         {{# validationName}} validation-name="{{i18n .}}" {{/ validationName}}
                         {{# label}} label="{{i18n .}}" {{/ label}}
                         {{# help}} help="{{i18n .}}" {{/ help}}
                         {{# addLabel}} add-label="{{i18n .}}" {{/ addLabel}}
                         {{# validations}} :validation="{{JsArray .}}" {{/ validations}}
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
                                 {{# validationName}} validation-name="{{i18n .}}" {{/ validationName}}
                                 {{# label}} label="{{i18n .}}" {{/ label}}
                                 {{# help}} help="{{i18n .}}" {{/ help}}
                                 {{# validations}} :validation="{{JsArray .}}" {{/ validations}}
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
    {{contenuform}}
</formulate-form>