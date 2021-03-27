<formulate-form v-model="contenuform"
                @submit="submitHandler"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                @failed-validation="failedValidation"
                :errors="inputErrors">
    {{#Form.Sections}}
    <div class="section">
        <h2>{{Section}}</h2>
        {{#MessageUrgent}}<div>{{.}}</div>{{/MessageUrgent}}
        {{#Inputs}}
        <formulate-input {{# Type}} type="{{.}}" {{/ Type}}
                         {{# Name}} name="{{.}}" {{/ Name}}
                         {{# ValidationName}} validation-name="{{.}}" {{/ ValidationName}}
                         {{# Label}} label="{{.}}" {{/ Label}}
                         {{# Help}} help="{{.}}" {{/ Help}}
                         {{# AddLabel}} add-label="{{.}}" {{/ AddLabel}}
                         {{# Validations}} :validation="{{JsArray .}}" {{/ Validations}}
                         {{# Options}} :options="{{JsObject .}}" {{/ Options}}
                         {{# Repeatable}}
                         :repeatable="{{#.}}true{{/.}}{{^.}}false{{/.}}"
                         {{/ Repeatable}}>
            <div class="group">
                {{#Inputs}}
                <formulate-input {{# Type}} type="{{.}}" {{/ Type}}
                                 {{# Name}} name="{{.}}" {{/ Name}}
                                 {{# ValidationName}} validation-name="{{.}}" {{/ ValidationName}}
                                 {{# Label}} label="{{.}}" {{/ Label}}
                                 {{# Help}} help="{{.}}" {{/ Help}}
                                 {{# AddLabel}} add-label="{{.}}" {{/ AddLabel}}
                                 {{# Validations}} :validation="{{JsArray .}}" {{/ Validations}}
                                 {{# Options}} :options="{{JsObject .}}" {{/ Options}}
                                 {{# Repeatable}}
                                 :repeatable="{{#.}}true{{/.}}{{^.}}false{{/.}}"
                                 {{/ Repeatable}}></formulate-input>
                {{/Inputs}}
            </div>
        </formulate-input>
        {{/Inputs}}
    </div>
    {{/Form.Sections}}

    <formulate-input type="submit"
                     label="Register"></formulate-input>
</formulate-form>