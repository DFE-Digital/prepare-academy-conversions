/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeOfsted extends BasePage {
    public path = 'ofsted';

    public enterInspectionDate(day: string, month: string, year: string): this {
        this.enterDate('inspection-date', day, month, year);
        return this;
    }

    public continueToQuestion(): this {
        cy.getById('save-and-continue-button').click();
        return this;
    }

    public verifyQuestionPage(date: string, expectedQuestion: string): this {
        cy.checkPath('ofsted-inspection-question');
        this.verifyHeading(expectedQuestion);
        cy.contains('.govuk-hint', `The most recent Ofsted inspection was ${date}`).should('be.visible');
        return this;
    }

    public selectMeetsStandards(meetsStandards: boolean): this {
        cy.getById(meetsStandards ? 'ofsted-yes' : 'ofsted-no').check();
        return this;
    }

    public verifyAdditionalInformationVisible(): this {
        cy.getById('AdditionalInformation').should('be.visible');
        return this;
    }

    public verifyAdditionalInformationHidden(): this {
        cy.getById('AdditionalInformation').should('not.be.visible');
        return this;
    }

    public enterAdditionalInformation(text: string): this {
        cy.getById('AdditionalInformation').clear();
        cy.getById('AdditionalInformation').type(text);
        return this;
    }

    public enterSupportingEvidence(text: string): this {
        cy.getByDataTest('ofsted-supporting-evidence').clear();
        cy.getByDataTest('ofsted-supporting-evidence').type(text);
        return this;
    }

    public saveQuestion(): this {
        cy.getById('save-and-continue-button').click();
        return this;
    }

    public verifyErrorSummaryContains(text: string): this {
        cy.get('.govuk-error-summary').should('be.visible').and('contain.text', text);
        return this;
    }
}

const significantChangeOfsted = new SignificantChangeOfsted();

export default significantChangeOfsted;
