/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeFhaRisksOrIssuesRaised extends BasePage {
    public path = 'fha-risks-or-issues-raised';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Financial health assessment (FHA) risks or issues raised');
        cy.contains('legend', 'Have any risks or issues been raised in the Financial Health Assessment (FHA)?').should(
            'be.visible'
        );
        return this;
    }

    public selectYes(): this {
        cy.getByDataTest('fha-risks-or-issues-raised-yes').check();
        return this;
    }

    public selectNo(): this {
        cy.getByDataTest('fha-risks-or-issues-raised-no').check();
        return this;
    }

    public verifyAdditionalInformationHidden(): this {
        cy.getByDataTest('fha-risks-or-issues-raised-additional-information').should('not.be.visible');
        return this;
    }

    public verifyAdditionalInformationVisible(): this {
        cy.getByDataTest('fha-risks-or-issues-raised-additional-information').should('be.visible');
        return this;
    }

    public enterAdditionalInformation(text: string): this {
        cy.getByDataTest('fha-risks-or-issues-raised-additional-information').clear();
        cy.getByDataTest('fha-risks-or-issues-raised-additional-information').type(text);
        return this;
    }

    public enterSupportingEvidence(text: string): this {
        cy.getByDataTest('fha-risks-or-issues-raised-supporting-evidence').clear();
        cy.getByDataTest('fha-risks-or-issues-raised-supporting-evidence').type(text);
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

const significantChangeFhaRisksOrIssuesRaised = new SignificantChangeFhaRisksOrIssuesRaised();

export default significantChangeFhaRisksOrIssuesRaised;
