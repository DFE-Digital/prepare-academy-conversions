/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeClearance extends BasePage {
    public path = 'clearance-details';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Clearance Details');
        cy.contains('p', 'Clearance is given after a review.').should('be.visible');
        return this;
    }

    public verifyClearanceFieldsVisible(): this {
        cy.getByDataTest('regions-group-clearance').should('be.visible');
        cy.getByDataTest('trust-regulation-clearance').should('be.visible');
        return this;
    }

    public enterRegionsGroupClearance(value: string): this {
        cy.getByDataTest('regions-group-clearance').clear().type(value);
        return this;
    }

    public enterTrustRegulationClearance(value: string): this {
        cy.getByDataTest('trust-regulation-clearance').clear().type(value);
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

const significantChangeClearance = new SignificantChangeClearance();

export default significantChangeClearance;
