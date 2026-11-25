/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeLocalSenPlans extends BasePage {
    public path = 'local-sen-plans';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Local SEN plans');
        cy.contains(
            'legend',
            'Does the proposal conflict with a Local SEN Reform Plan (or equivalent) for SEN, AP and high needs provision?'
        ).should('be.visible');
        return this;
    }

    public verifySupportingEvidencePresent(): this {
        cy.getByDataTest('supporting-evidence').should('exist');
        return this;
    }

    public verifyAdditionalInformationVisible(visible: boolean): this {
        cy.getByDataTest('additional-information').should(visible ? 'be.visible' : 'not.be.visible');
        return this;
    }

    public enterAdditionalInformation(value: string): this {
        cy.getByDataTest('additional-information').clear();
        cy.getByDataTest('additional-information').type(value);
        return this;
    }

    public enterSupportingEvidence(value: string): this {
        cy.getByDataTest('supporting-evidence').clear();
        cy.getByDataTest('supporting-evidence').type(value);
        return this;
    }
}

const significantChangeLocalSenPlans = new SignificantChangeLocalSenPlans();

export default significantChangeLocalSenPlans;
