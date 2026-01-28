/// <reference types="cypress" />
import BasePage from './basePage';

const today = new Date();

class ConversionDetails extends BasePage {
    public path = 'conversion-details';

    private readonly selectors = {
        saveButton: '[data-cy="select-common-submitbutton"]',
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
        proposedOpeningRadioButton: (month: number, year: number) =>
            `input[data-cy="select-radio-${month}/01/${year} 00:00:00"]`,
        previousAdvisoryBoardLink: '[data-test="change-previous-advisory-board"]',
        previousAdvisoryBoardValue: '[id="previous-advisory-board"]',
        previousAdvisoryBoardDateInput: 'previous-head-teacher-board-date',
        authorLink: '[data-test="change-author"]',
        authorValue: '[id="author"]',
        authorInput: '[id="author"]',
        clearedByLink: '[data-test="change-cleared-by"]',
        clearedByValue: '[id="cleared-by"]',
        clearedByInput: '[id="cleared-by"]',
        completeCheckbox: 'school-and-trust-information-complete',
    };

    public setForm7Receivied(status = 'Yes'): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.form7ReceivedLink).click();
        cy.RadioBtn(status).check();
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getForm7Receivied(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.form7ReveicedValue);
    }

    public setForm7Date(date: Date = today): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.form7DateLink).click();
        cy.enterDate(this.selectors.form7DateInput, date.getDate(), date.getMonth() + 1, date.getFullYear());
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getForm7Date(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.form7DateValue);
    }

    // DAO

    public setDAODate(date: Date = today): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.DAOLink).click();
        cy.enterDate(this.selectors.DAODateInput, date.getDate(), date.getMonth() + 1, date.getFullYear());
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getDAODate(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.DAOValue);
    }

    // Funding

    public setFundingType(type = 'Full'): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.fundingTypeLink).click();
        cy.RadioBtn(type).check();
        //Save button twice to get back to the details page
        cy.get(this.selectors.saveButton).click();
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getFundingType(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.fundingTypeValue);
    }

    public setFundingAmount(useDefaultAmount = false, amount = 20000): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.fundingAmountLink).click();
        if (useDefaultAmount) {
            cy.get(this.selectors.fundingAmountDefaultYesRadio).click();
        } else {
            cy.get(this.selectors.fundingAmountDefaultNoRadio).click();
            cy.get(this.selectors.fundingAmountInput).clear();
            cy.get(this.selectors.fundingAmountInput).type(String(amount));
        }
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getFundingAmount(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.fundingAmountValue);
    }

    public setFundingReason(reason = 'Funding Reason'): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.fundingAmountLink).click();
        cy.get(this.selectors.fundingAmountReasonInput).clear();
        cy.get(this.selectors.fundingAmountReasonInput).type(reason);
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getFundingReason(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.fundingAmountReasonValue);
    }

    public setEIG(value = true): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.EIGLink).click();
        if (value) {
            cy.get(this.selectors.EIGYesRadio).check();
        } else {
            cy.get(this.selectors.EIGNoRadio).check();
        }
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getEIG(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.EIGValue);
    }

    public setPreviousAdvisoryBoardDate(previousBoard = true, date: Date = today): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.previousAdvisoryBoardLink).click();
        if (previousBoard) {
            cy.YesRadioBtn().check();
            cy.get(this.selectors.saveButton).click();
            cy.enterDate(
                this.selectors.previousAdvisoryBoardDateInput,
                date.getDate(),
                date.getMonth() + 1,
                date.getFullYear()
            );
        } else {
            cy.NoRadioBtn().check();
        }
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getPreviousAdvisoryBoardDate(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.previousAdvisoryBoardValue);
    }

    // People

    public setAuthor(author = 'Nicholas Warms'): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.authorLink).click();
        cy.get(this.selectors.authorInput).clear();
        cy.get(this.selectors.authorInput).type(author);
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getAuthor(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.authorValue);
    }

    public setClearedBy(clearedBy = 'Nicholas Warms'): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.clearedByLink).click();
        cy.get(this.selectors.clearedByInput).clear();
        cy.get(this.selectors.clearedByInput).type(clearedBy);
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getClearedBy(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.clearedByValue);
    }

    // Complete

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
}

const conversionDetails = new ConversionDetails();

export default conversionDetails;
