/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeMainstreamSchoolSatelliteConsiderations extends BasePage {
    public path = 'mainstream-school-satellite-considerations';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Mainstream school satellite considerations');
        cy.contains(
            'legend',
            'Does the proposal meet the requirements set out for special school provision on a satellite site in the operational guidance?'
        ).should('be.visible');
        return this;
    }

    public openTask(): this {
        cy.contains('a', 'Mainstream school satellite considerations').click();
        return this;
    }

    public verifySupportingEvidencePresent(): this {
        cy.getByDataTest('supporting-evidence').should('exist');
        return this;
    }

    public verifyAllOptionsPresent(): this {
        cy.getByDataTest('answer-yes').should('exist');
        cy.getByDataTest('additional-info').should('exist');
        cy.getByDataTest('not-applicable').should('exist');
        return this;
    }

    public selectNo(): this {
        cy.getByDataTest('additional-info').check();
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

    public save(): this {
        cy.getById('save-and-continue-button').click();
        return this;
    }

    public verifyErrorSummaryContains(message: string): this {
        cy.get('.govuk-error-summary').should('be.visible').and('contain.text', message);
        return this;
    }

    public verifyInlineError(field: string, message: string): this {
        cy.getById(`${field}-error`).should('contain.text', message);
        return this;
    }
}

const significantChangeMainstreamSchoolSatelliteConsiderations =
    new SignificantChangeMainstreamSchoolSatelliteConsiderations();

export default significantChangeMainstreamSchoolSatelliteConsiderations;
