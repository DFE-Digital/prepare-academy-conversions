/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeHqtfConsideration extends BasePage {
    public path = 'hqtf-consideration';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('High Quality Trust Framework (HQTF) consideration');
        cy.contains(
            'legend',
            'Have any issues or risks been identified with regards to the High Quality Trust Framework (HQTF) pillars?'
        ).should('be.visible');
        return this;
    }

    public verifyAllOptionsPresent(): this {
        cy.getByDataTest('hqtf-consideration-yes').should('exist');
        cy.getByDataTest('hqtf-consideration-no').should('exist');
        cy.getByDataTest('hqtf-consideration-not-applicable').should('exist');
        cy.contains('label', 'Yes').should('be.visible');
        cy.contains('label', 'No').should('be.visible');
        cy.contains('label', 'Not applicable').should('be.visible');
        return this;
    }

    public selectYes(): this {
        cy.getByDataTest('hqtf-consideration-yes').check();
        return this;
    }

    public selectNo(): this {
        cy.getByDataTest('hqtf-consideration-no').check();
        return this;
    }

    public selectNotApplicable(): this {
        cy.getByDataTest('hqtf-consideration-not-applicable').check();
        return this;
    }

    public verifyAdditionalInformationHidden(): this {
        cy.getByDataTest('hqtf-consideration-additional-information').should('not.be.visible');
        return this;
    }

    public verifyAdditionalInformationVisible(): this {
        cy.getByDataTest('hqtf-consideration-additional-information').should('be.visible');
        return this;
    }

    public enterAdditionalInformation(text: string): this {
        cy.getByDataTest('hqtf-consideration-additional-information').clear().type(text);
        return this;
    }

    public clearAdditionalInformation(): this {
        cy.getByDataTest('hqtf-consideration-additional-information').clear();
        return this;
    }

    public enterSupportingEvidence(text: string): this {
        cy.getByDataTest('hqtf-consideration-supporting-evidence').clear().type(text);
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
