<formulate-form v-model="contenuform"
                @submit="submitHandler"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                @failed-validation="failedValidation"
                :errors="inputErrors">
    {{#Form.Sections}}
    <div class="section">
        <h2>{{section}}</h2>
        {{#MessageUrgent}}<div>{{.}}</div>{{/MessageUrgent}}
        {{#InputsHandled}}
        {{#attributes}}
        <formulate-input {{# type}} type="{{.}}" {{/ type}}
                         {{# name}} name="{{.}}" {{/ name}}
                         {{# validationName}} validation-name="{{.}}" {{/ validationName}}
                         {{# label}} label="{{.}}" {{/ label}}
                         {{# help}} help="{{.}}" {{/ help}}
                         {{# addLabel}} add-label="{{.}}" {{/ addLabel}}
                         {{# validations}} :validation="{{JsArray .}}" {{/ validations}}
                         {{# options}} :options="{{JsObject .}}" {{/ options}}
                         {{# repeatable}}
                         :repeatable="{{#.}}true{{/.}}{{^.}}false{{/.}}"
                         {{/ repeatable}}>
            {{#isGroup}}
            <div class="group">
                {{#inputs}}
                <formulate-input {{# type}} type="{{.}}" {{/ type}}
                                 {{# name}} name="{{.}}" {{/ name}}
                                 {{# validationName}} validation-name="{{.}}" {{/ validationName}}
                                 {{# label}} label="{{.}}" {{/ label}}
                                 {{# help}} help="{{.}}" {{/ help}}
                                 {{# validations}} :validation="{{JsArray .}}" {{/ validations}}
                                 {{# options}} :options="{{JsObject .}}" {{/ options}}></formulate-input>
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
</formulate-form>