/// <reference types ='Cypress'/>

import BasePage from "./basePage"

export default class PublicSectorEqualityDutyImpact extends BasePage {
    static selectors = {
        psedChangeLink: '[data-test="change-reduce-impact-reason-label"]',
        confirmButton: '[id="confirm-and-continue-button"]',
        completeCheckbox: '[id="public-sector-equality-duty-complete"]',
        unLikely: '[data-cy="select-radio-psed-impact-unlikely"]',
        someImpact: '[data-cy="select-radio-psed-impact-someimpact"]',
        likely: '[data-cy="select-radio-psed-impact-likely"]',
        psedErrorMessage: '[data-cy="error-message-0+="]',
        psedValue: '[id="reduce-impact-reason-label"]',
        psedReasonValue: '[id="reduce-impact-reason"]',
        psedReasonInput: '[data-cy="psed-impact-reason-input"]',
        saveButton: '[data-cy="select-common-submitbutton"]',
    }

    static path = 'public-sector-equality-duty'

    static changePsed(psedUnlikeyConsideration) {
        cy.checkPath(this.path)
        cy.get(this.selectors.psedChangeLink).click()
        if (psedUnlikeyConsideration == 'Unlikely') {
            cy.get(this.selectors.unLikely).check()
        }
        else if (psedUnlikeyConsideration == 'Some impact') {
            cy.get(this.selectors.someImpact).check()
        }
        else {
            cy.get(this.selectors.likely).check()
        }
        cy.get(this.selectors.saveButton).click()

    }

    static getPsed() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.psedValue)
    }

    static enterImpactReason() {
        cy.get(this.selectors.psedReasonInput).clear()
        cy.get(this.selectors.psedReasonInput).type('Test PSED reason')
        cy.get(this.selectors.saveButton).click()
    }

    static getImpactReason() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.psedReasonValue)
    }

    static markComplete() {
        cy.checkPath(this.path)
        cy.get(this.selectors.completeCheckbox).check()
    }

    static markIncomplete() {
        cy.checkPath(this.path)
        cy.get(this.selectors.completeCheckbox).uncheck()
    }

    static getErrorMessage() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.psedErrorMessage)
    }

    static clickSaveBtnWithoutReason() {
        cy.checkPath(this.path)
        cy.get(this.selectors.psedReasonInput).clear()
        return cy.get(this.selectors.saveButton).click()
    }

}
