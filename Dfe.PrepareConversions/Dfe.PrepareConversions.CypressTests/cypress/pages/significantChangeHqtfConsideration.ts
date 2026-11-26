/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeHqtfConsideration extends BasePage {
    public path = 'hqtf-consideration';
    public additionalInformationSelector = '[data-test="hqtf-consideration-additional-information"]';
    public supportingEvidenceSelector = '[data-test="hqtf-consideration-supporting-evidence"]';

    public verifyPageLoaded(): this {
        return this.verifyPageLoadedWithLegend(
            'High Quality Trust Framework (HQTF) consideration',
            'Have any issues or risks been identified with regards to the High Quality Trust Framework (HQTF) pillars?'
        );
    }

    public verifyHqtfAllOptionsPresent(): this {
        cy.getByDataTest('hqtf-consideration-yes').should('exist');
        cy.getByDataTest('hqtf-consideration-no').should('exist');
        cy.getByDataTest('hqtf-consideration-not-applicable').should('exist');
        cy.contains('label', 'Yes').should('be.visible');
        cy.contains('label', 'No').should('be.visible');
        cy.contains('label', 'Not applicable').should('be.visible');
        return this;
    }

    public selectHqtfYes(): this {
        cy.getByDataTest('hqtf-consideration-yes').check();
        return this;
    }

    public selectHqtfNo(): this {
        cy.getByDataTest('hqtf-consideration-no').check();
        return this;
    }

    public selectHqtfNotApplicable(): this {
        cy.getByDataTest('hqtf-consideration-not-applicable').check();
        return this;
    }

    public verifyHqtfAdditionalInformationHidden(): this {
        cy.getByDataTest('hqtf-consideration-additional-information').should('not.be.visible');
        return this;
    }

    public verifyHqtfAdditionalInformationVisible(): this {
        cy.getByDataTest('hqtf-consideration-additional-information').should('be.visible');
        return this;
    }

    public clearHqtfAdditionalInformation(): this {
        cy.getByDataTest('hqtf-consideration-additional-information').clear();
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

const significantChangeHqtfConsideration = new SignificantChangeHqtfConsideration();

export default significantChangeHqtfConsideration;
