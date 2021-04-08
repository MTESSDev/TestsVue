<formulate-form v-model="contenuform"
                @submit="submitHandler"
                @submit-raw="submitRawHandler"
                :invalid-message="invalidMessage"
                :form-errors="formErrors"
                @failed-validation="failedValidation"
                :errors="inputErrors">

    <div v-show="contenuform.noPageCourante === 1">
        <formulate-input type="radio"
                         name="ne_qc"
                         label="Êtes-vous né au Québec?"
                         validation="required"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>
        <formulate-input v-if="contenuform.ne_qc === 'oui'"
                         type="text"
                         key="nom_parent"
                         name="nom_parent"
                         validation="required"
                         label="Inscrivez le nom de famille et le prénom d'un de vos parents:">
        </formulate-input>
        <formulate-input v-if="contenuform.ne_qc === 'non'"
                         type="radio"
                         key="citoyen_canadien"
                         name="citoyen_canadien"
                         validation="required"
                         label="Avez-vous la citoyenneté canadienne?"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>
        <formulate-input v-if="contenuform.citoyen_canadien === 'non'"
                         type="radio"
                         key="resident_permanent"
                         name="resident_permanent"
                         validation="required"
                         label="Êtes-vous résident permanent?"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>
        <formulate-input v-if="contenuform.resident_permanent === 'non'"
                         type="radio"
                         key="demandeur_asile"
                         name="demandeur_asile"
                         validation="required"
                         label="Êtes-vous un demandeur d'asile?"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>


        <formulate-input v-if="contenuform.demandeur_asile === 'non'"
                         type="radio"
                         key="refugie_protege"
                         name="refugie_protege"
                         validation="required"
                         label="Avez-vous obtenu le statut de réfugié ou de personne protégée ou à protéger?"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>


        <formulate-input type="radio"
                         name="deja_travaille"
                         label="Avez-vous déjà travaillé?"
                         validation="required"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>


        <formulate-input type="group"
                         v-if="contenuform.deja_travaille === 'oui'"
                         name="emplois"
                         :repeatable="true"
                         label="Emplois"
                         validation="required"
                         add-label="+ Ajouter un emploi">
            <formulate-input name="nom_entreprise"
                             validation="required"
                             label="Nom de l'entreprise">
            </formulate-input>

            <formulate-input name="periode_du"
                             type="date"
                             validation="required"
                             label="Période du">
            </formulate-input>

            <formulate-input name="periode_au"
                             type="date"
                             validation="required"
                             label="Période au">
            </formulate-input>
            <formulate-input name="salaire_hebdo"
                             type="number"
                             validation="required"
                             label="Salaire par semaine">
            </formulate-input>
            <formulate-input name="heures_hebdo"
                             type="number"
                             validation="required"
                             label="Heures par semaine">
            </formulate-input>

            <formulate-input type="radio"
                             validation="required"
                             name="raison_fin_emploi"
                             label="Raison de la fin de l'emploi"
                             :options="{1: 'Manque de travail', 2: 'Problème de santé', 3: 'Changement d\'emploi', 4: 'Changement de l\'entreprise',
                                           5: 'Fermeture de l\'entreprise', 6: 'Naissance ou prise en charge d\'un enfant', 7: 'Congédiement', 8: 'Abandon de l\'emploi',
                                           9: 'Autre'}">
            </formulate-input>
            <formulate-input v-if="contenuform.raison_fin_emploi === '9'"
                             name="raison_fin_emploi_autre"
                             validation="required"
                             label="Précisez la raison de fin d'emploi">
            </formulate-input>

        </formulate-input>

        <formulate-input type="radio"
                         name="a_limitations_fonctionnelles"
                         label="Avez-vous des limitations fonctionnelles faisant suite à des lésions professionnelles (ex. accident du travail)?"
                         validation="required"
                         :options="{oui: 'Oui', non: 'Non'}">
        </formulate-input>
        <formulate-input v-if="contenuform.a_limitations_fonctionnelles === 'oui'"
                         name="limitations_fonctionnelles_precisions"
                         validation="required"
                         label="Précisez :">
        </formulate-input>

        <formulate-input name="langues_parlees"
                         :options="{francais: 'Français', anglais: 'Anglais', autres: 'Autre(s)'}"
                         type="checkbox"
                         validation="required"
                         label="Langues parlées">
        </formulate-input>
        <formulate-input v-if="contenuform.langues_parlees && contenuform.langues_parlees.includes('autres')"
                         name="autres_langues_parlees"
                         validation="required"
                         label="Précisez :">
        </formulate-input>

    </div>

    <div v-show="contenuform.noPageCourante === 2">
        Section 2 avec rien :(
    </div>

    <div v-show="contenuform.noPageCourante === 3">
        Section 7 avec rien :(
    </div>

    <formulate-input type="submit"
                     label="Soumettre"></formulate-input>


    <template>
        {{ contenuform  }}
    </template>
</formulate-form>