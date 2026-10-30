/// <reference types="cypress" />
import BasePage from './basePage';

class AdmissionVariationConsultation extends BasePage {
    public path = 'admission-variation-consultation';

    private readonly selectors = {
        yesRadio: 'consultation-include-admission-variation-yes',
        noRadio: 'consultation-include-admission-variation-no',
        reasonTextAreaDataTest: 'consultation-no-admission-variation-reason',
        reasonError: 'ConsultationNoAdmissionVariationReason-error-link',
        saveAndContinueBtn: 'select-common-submitbutton',
    };

    public verifyPageIsVisible(): this {
        cy.checkPath(this.path);
        cy.get('h1').should('contain.text', 'Admission variation consultation');
        cy.getByDataTest(this.selectors.yesRadio).should('exist');
        cy.getByDataTest(this.selectors.noRadio).should('exist');
        return this;
    }

    public selectYesAndContinue(): this {
        cy.getByDataTest(this.selectors.yesRadio).check();
        cy.getByDataCy(this.selectors.saveAndContinueBtn).click();
        return this;
    }

    public selectNoAndContinueWithoutReason(): this {
        cy.getByDataTest(this.selectors.noRadio).check();
        cy.getByDataTest(this.selectors.reasonTextAreaDataTest).clear();
        cy.getByDataCy(this.selectors.saveAndContinueBtn).click();
        return this;
    }

    public selectNoWithReasonAndContinue(reason: string): this {
        cy.getByDataTest(this.selectors.noRadio).check();
        cy.getByDataTest(this.selectors.reasonTextAreaDataTest).clear();
        cy.getByDataTest(this.selectors.reasonTextAreaDataTest).type(reason);
        cy.getByDataCy(this.selectors.saveAndContinueBtn).click();
        return this;
    }

    public continueWithoutSelectingOption(): this {
        cy.getByDataCy(this.selectors.saveAndContinueBtn).click();
        return this;
    }

    public verifyMissingReasonError(): this {
        cy.getById(this.selectors.reasonError).should('contain.text', 'Add a reason');
        return this;
    }
}

const admissionVariationConsultation = new AdmissionVariationConsultation();

export default admissionVariationConsultation;
