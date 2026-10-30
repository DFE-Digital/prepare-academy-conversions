/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeRecommendation extends BasePage {
    public path = 'significant-change-recommendation';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Significant change recommendation');
        cy.contains('legend', 'Recommendation').should('be.visible');
        return this;
    }

    public verifyAllOptionsPresent(): this {
        cy.getByDataTest('approve-radio').should('exist');
        cy.getByDataTest('decline-radio').should('exist');
        cy.getByDataTest('defer-radio').should('exist');
        return this;
    }

    public selectApprove(): this {
        cy.getByDataTest('approve-radio').check();
        return this;
    }

    public selectDecline(): this {
        cy.getByDataTest('decline-radio').check();
        return this;
    }

    public selectDefer(): this {
        cy.getByDataTest('defer-radio').check();
        return this;
    }

    public verifyRecommendationMoreInformationVisible(): this {
        cy.getByDataTest('recommendation-more-information').should('be.visible');
        return this;
    }

    public enterMoreInformation(moreInformation: string): this {
        cy.getByDataTest('recommendation-more-information').clear().type(moreInformation);
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

const significantChangeRecommendation = new SignificantChangeRecommendation();

export default significantChangeRecommendation;
