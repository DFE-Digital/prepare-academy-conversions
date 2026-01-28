/// <reference types="cypress" />
import BasePage from './basePage';

class PublicSectorEqualityDutyImpact extends BasePage {
    public path = 'public-sector-equality-duty';

    private selectors = {
        psedChangeLink: '[data-test="change-reduce-impact-reason-label"]',
        confirmButton: '[id="confirm-and-continue-button"]',
        completeCheckbox: 'public-sector-equality-duty-complete',
        unLikely: '[data-cy="select-radio-psed-impact-unlikely"]',
        someImpact: '[data-cy="select-radio-psed-impact-someimpact"]',
        likely: '[data-cy="select-radio-psed-impact-likely"]',
        psedErrorMessage: '[data-cy="error-message-0+="]',
        psedValue: '[id="reduce-impact-reason-label"]',
        psedReasonValue: '[id="reduce-impact-reason"]',
        psedReasonInput: '[data-cy="psed-impact-reason-input"]',
        saveButton: '[data-cy="select-common-submitbutton"]',
    };

    public changePsed(psedUnlikeyConsideration: string): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.psedChangeLink).click();
        if (psedUnlikeyConsideration == 'Unlikely') {
            cy.get(this.selectors.unLikely).check();
        } else if (psedUnlikeyConsideration == 'Some impact') {
            cy.get(this.selectors.someImpact).check();
        } else {
            cy.get(this.selectors.likely).check();
        }
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getPsed(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.psedValue);
    }

    public enterImpactReason(): this {
        cy.get(this.selectors.psedReasonInput).clear();
        cy.get(this.selectors.psedReasonInput).type('Test PSED reason');
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getImpactReason(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.psedReasonValue);
    }

    public markComplete(): this {
        cy.checkPath(this.path);
        cy.get(`#${this.selectors.completeCheckbox}`).check();
        return this;
    }

    public markIncomplete(): this {
        cy.checkPath(this.path);
        cy.get(`#${this.selectors.completeCheckbox}`).uncheck();
        return this;
    }

    public getErrorMessage(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.psedErrorMessage);
    }

    public clickSaveBtnWithoutReason(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        cy.get(this.selectors.psedReasonInput).clear();
        return cy.get(this.selectors.saveButton).click();
    }
}

const publicSectorEqualityDutyImpact = new PublicSectorEqualityDutyImpact();

export default publicSectorEqualityDutyImpact;
