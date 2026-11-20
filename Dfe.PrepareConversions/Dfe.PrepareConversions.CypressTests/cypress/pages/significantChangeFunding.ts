/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeFunding extends BasePage {
    public path = 'funding';
    public additionalInformationSelector = '[data-test="funding-additional-information"]';
    public supportingEvidenceSelector = '[data-test="funding-supporting-evidence"]';

    public verifyPageLoaded(): this {
        return this.verifyPageLoadedWithLegend('Funding', 'Has funding been secured?');
    }

    public verifyAllOptionsPresent(): this {
        cy.getByDataTest('funding-yes').should('exist');
        cy.getByDataTest('funding-no').should('exist');
        cy.getByDataTest('funding-not-applicable').should('exist');
        return this;
    }

    public selectYes(): this {
        cy.getByDataTest('funding-yes').check();
        return this;
    }

    public selectNo(): this {
        cy.getByDataTest('funding-no').check();
        return this;
    }

    public selectNotApplicable(): this {
        cy.getByDataTest('funding-not-applicable').check();
        return this;
    }

    public verifyFundingAdditionalInformationHidden(): this {
        cy.getByDataTest('funding-additional-information').should('not.be.visible');
        return this;
    }

    public verifyFundingAdditionalInformationVisible(): this {
        cy.getByDataTest('funding-additional-information').should('be.visible');
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

const significantChangeFunding = new SignificantChangeFunding();

export default significantChangeFunding;
