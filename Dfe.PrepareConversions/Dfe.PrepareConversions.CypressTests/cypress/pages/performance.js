/// <reference types ='Cypress'/>

import BasePage from "./BasePage"

export default class Performance extends BasePage {
    static selectors = {
        ofstedInfoLink: '[data-test="change-school-performance-additional-information"]',
        ofstedInfoInput: '[id="additional-information"]',
        ofstedInfoValue: '[id="additional-information"]',
        keyStageLink: (keyStageNumber) => `[data-test="change-key-stage-${keyStageNumber}-additional-information"]`,
        keyStageInput: '[id="additional-information"]',
        keyStageValue: '[id="additional-information"]',
        saveButton: '[class="govuk-button"]'
    }

    static ofstedPath = 'school-performance-ofsted-information'
    static keyStagePath = (keyStageNumber) => `key-stage-${keyStageNumber}-performance-tables`

    static changeOfstedInfo(additionalInfo) {
        cy.checkPath(this.ofstedPath)
        cy.get(this.selectors.ofstedInfoLink).click()
        cy.get(this.selectors.ofstedInfoInput).clear()
        cy.get(this.selectors.ofstedInfoInput).type(additionalInfo)
        cy.get(this.selectors.saveButton).click()
    }

    static getOfstedInfo() {
        cy.checkPath(this.ofstedPath)
        return cy.get(this.selectors.ofstedInfoValue)
    }

    static changeKeyStageInfo(keyStageNumber, additionalInfo) {
        cy.checkPath(this.keyStagePath(keyStageNumber))
        cy.get(this.selectors.keyStageLink(keyStageNumber)).click()
        cy.get(this.selectors.keyStageInput).clear()
        cy.get(this.selectors.keyStageInput).type(additionalInfo)
        cy.get(this.selectors.saveButton).click()
    }

    static getKeyStageInfo(keyStageNumber) {
        cy.checkPath(this.keyStagePath(keyStageNumber))
        return cy.get(this.selectors.keyStageValue)
    }
    
}
