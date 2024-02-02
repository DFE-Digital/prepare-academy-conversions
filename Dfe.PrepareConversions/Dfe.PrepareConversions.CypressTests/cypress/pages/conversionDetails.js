/// <reference types ='Cypress'/>

import BasePage from "./BasePage"

const today = new Date()

export default class ConversionDetails extends BasePage {
    static selectors = {
        saveButton: '[class="govuk-button"]',
        form7ReceivedLink: '[data-test="change-form-7-received"]',
        form7ReveicedValue: '[id="form-7-received"]',
        form7DateLink: '[data-test="change-form-7-received-date"]',
        form7DateInput: 'form-7-received-date',
        form7DateValue: '[id="form-7-received-date"]',
        DAOLink: '[data-test="change-dao-pack-sent-date"]',
        DAODateInput: 'dao-pack-sent-date',
        DAOValue: '[id="dao-pack-sent-date"]',
        fundingTypeLink: '[data-test="change-grant-funding-type"]',
        fundingTypeValue: '[id="grant-funding-type"]',
        fundingAmountLink: '[data-test="change-grant-funding-amount"]',
        fundingAmountValue: '[id="grant-funding-amount"]',
        fundingAmountDefaultYesRadio: '[name="conversion-support-grant-amount-changed"][data-cy="select-radio-yes"]',
        fundingAmountDefaultNoRadio: '[name="conversion-support-grant-amount-changed"][data-cy="select-radio-no"]',
        fundingAmountInput: '[id="conversion-support-grant-amount"]',
        fundingAmountReasonInput: '[id="conversion-support-grant-change-reason"]',
        fundingAmountReasonValue: '[id="grant-funding-reason"]',
        fundingAmountReasonLink: '[data-test="change-grant-funding-reason"]',
        EIGLink: '[data-test="change-grant-funding-environmental-improvement-grant"]',
        EIGValue: '[id="grant-funding-environmental-improvement-grant"]',
        EIGYesRadio: '[name="conversion-support-grant-environmental-improvement-grant"][data-cy="select-radio-yes" i]',
        EIGNoRadio: '[name="conversion-support-grant-environmental-improvement-grant"][data-cy="select-radio-no" i]',
        advisoryBoardDateLink: '[data-test="change-advisory-board-date"]',
        advisoryBoardDateValue: '[id="advisory-board-date"]',
        advisoryBoardDateInput: 'head-teacher-board-date',
        proposedOpeningLink: '[data-test="change-proposed-academy-opening-date"]',
        proposedOpeningValue: '[id="proposed-academy-opening-date"]',
        proposedOpeningRadioButton: (month, year) => `input[data-cy="select-radio-${month}/01/${year} 00:00:00"]`,
        previousAdvisoryBoardLink: '[data-test="change-previous-advisory-board"]',
        previousAdvisoryBoardValue: '[id="previous-advisory-board"]',
        previousAdvisoryBoardDateInput: 'previous-head-teacher-board-date',
        authorLink: '[data-test="change-author"]',
        authorValue: '[id="author"]',
        authorInput: '[id="author"]',
        clearedByLink: '[data-test="change-cleared-by"]',
        clearedByValue: '[id="cleared-by"]',
        clearedByInput: '[id="cleared-by"]',
        completeCheckbox: '[id="school-and-trust-information-complete"]'
    }

    static path = 'conversion-details'

    static setForm7Receivied(status = 'Yes') {
        cy.checkPath(this.path)
        cy.get(this.selectors.form7ReceivedLink).click()
        cy.RadioBtn(status).check()
        cy.get(this.selectors.saveButton).click()
    }

    static getForm7Receivied() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.form7ReveicedValue)
    }

    static setForm7Date(date = today) {
        cy.checkPath(this.path)
        cy.get(this.selectors.form7DateLink).click()
        cy.enterDate(this.selectors.form7DateInput, date.getDate(), date.getMonth() + 1, date.getFullYear())
        cy.get(this.selectors.saveButton).click()
    }

    static getForm7Date() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.form7DateValue)
    }

    // DAO

    static setDAODate(date = today) {
        cy.checkPath(this.path)
        cy.get(this.selectors.DAOLink).click()
        cy.enterDate(this.selectors.DAODateInput, date.getDate(), date.getMonth() + 1, date.getFullYear())
        cy.get(this.selectors.saveButton).click()
    }

    static getDAODate() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.DAOValue)
    }

    // Funding

    static setFundingType(type = 'Full') {
        cy.checkPath(this.path)
        cy.get(this.selectors.fundingTypeLink).click()
        cy.RadioBtn(type).check()
        //Save button twice to get back to the details page
        cy.get(this.selectors.saveButton).click()
        cy.get(this.selectors.saveButton).click()
    }

    static getFundingType() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.fundingTypeValue)
    }

    static setFundingAmount(useDefaultAmount = false, amount = 20000) {
        cy.checkPath(this.path)
        cy.get(this.selectors.fundingAmountLink).click()
        if (useDefaultAmount) {
            cy.get(this.selectors.fundingAmountDefaultYesRadio).click()
        } else {
            cy.get(this.selectors.fundingAmountDefaultNoRadio).click()
            cy.get(this.selectors.fundingAmountInput).clear()
            cy.get(this.selectors.fundingAmountInput).type(amount)
        }
        cy.get(this.selectors.saveButton).click()
    }

    static getFundingAmount() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.fundingAmountValue)
    }

    static setFundingReason(reason = 'Funding Reason') {
        cy.checkPath(this.path)
        cy.get(this.selectors.fundingAmountLink).click()
        cy.get(this.selectors.fundingAmountReasonInput).clear()
        cy.get(this.selectors.fundingAmountReasonInput).type(reason)
        cy.get(this.selectors.saveButton).click()
    }

    static getFundingReason() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.fundingAmountReasonValue)
    }

    static setEIG(value = true) {
        cy.checkPath(this.path)
        cy.get(this.selectors.EIGLink).click()
        if (value) {
            cy.get(this.selectors.EIGYesRadio).check()
        } else {
            cy.get(this.selectors.EIGNoRadio).check()
        }
        cy.get(this.selectors.saveButton).click()
    }

    static getEIG() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.EIGValue)
    }

    // Dates

    static setAdvisoryBoardDate(date = today) {
        cy.checkPath(this.path)
        cy.get(this.selectors.advisoryBoardDateLink).click()
        cy.enterDate(this.selectors.advisoryBoardDateInput, date.getDate(), date.getMonth() + 1, date.getFullYear())
        cy.get(this.selectors.saveButton).click()
    }

    static getAdvisoryBoardDate() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.advisoryBoardDateValue)
    }

    static setProposedAcademyOpening(month, year) {
        cy.checkPath(this.path)
        cy.get(this.selectors.proposedOpeningLink).click()
        cy.get(this.selectors.proposedOpeningRadioButton(month, year)).check()
        cy.get(this.selectors.saveButton).click()
    }

    static getProposedAcademyOpening() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.proposedOpeningValue)
    }

    static setPreviousAdvisoryBoardDate(previousBoard = true, date = today) {
        cy.checkPath(this.path)
        cy.get(this.selectors.previousAdvisoryBoardLink).click()
        if (previousBoard) {
            cy.YesRadioBtn().check()
            cy.get(this.selectors.saveButton).click()
            cy.enterDate(this.selectors.previousAdvisoryBoardDateInput, date.getDate(), date.getMonth() + 1, date.getFullYear())

        } else {
            cy.NoRadioBtn().check()
        }
        cy.get(this.selectors.saveButton).click()
    }

    static getPreviousAdvisoryBoardDate() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.previousAdvisoryBoardValue)
    }

    // People

    static setAuthor(author = 'Nicholas Warms') {
        cy.checkPath(this.path)
        cy.get(this.selectors.authorLink).click()
        cy.get(this.selectors.authorInput).clear()
        cy.get(this.selectors.authorInput).type(author)
        cy.get(this.selectors.saveButton).click()
    }

    static getAuthor() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.authorValue)
    }

    static setClearedBy(clearedBy = 'Nicholas Warms') {
        cy.checkPath(this.path)
        cy.get(this.selectors.clearedByLink).click()
        cy.get(this.selectors.clearedByInput).clear()
        cy.get(this.selectors.clearedByInput).type(clearedBy)
        cy.get(this.selectors.saveButton).click()
    }

    static getClearedBy() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.clearedByValue)
    }

    // Complete

    static markComplete() {
        cy.checkPath(this.path)
        cy.get(this.selectors.completeCheckbox).check()
    }

    static markIncomplete() {
        cy.checkPath(this.path)
        cy.get(this.selectors.completeCheckbox).uncheck()
    }

}
