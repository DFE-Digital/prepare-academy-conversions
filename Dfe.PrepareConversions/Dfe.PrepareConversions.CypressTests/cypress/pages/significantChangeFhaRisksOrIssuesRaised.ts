/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeFhaRisksOrIssuesRaised extends BasePage {
    public path = 'fha-risks-or-issues-raised';
    public additionalInformationSelector = '[data-test="fha-risks-or-issues-raised-additional-information"]';
    public supportingEvidenceSelector = '[data-test="fha-risks-or-issues-raised-supporting-evidence"]';

    public verifyPageLoaded(): this {
        return this.verifyPageLoadedWithLegend(
            'Financial health assessment (FHA) risks or issues raised',
            'Have any risks or issues been raised in the Financial Health Assessment (FHA)?'
        );
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
