/// <reference types="cypress" />
import BasePage from './basePage';

class LocalAuthorityInformation extends BasePage {
    public path = 'confirm-local-authority-information-template-dates';

    private selectors = {
        templateSentLink: '[data-test="change-la-info-template-sent-date"]',
        templateSentInput: 'la-info-template-sent-date',
        templateSentValue: '[id="la-info-template-sent-date"]',
        templateReturnedLink: '[data-test="change-la-info-template-returned-date"]',
        templateReturnedInput: 'la-info-template-returned-date',
        templateReturnedValue: '[id="la-info-template-returned-date"]',
        commentsLink: '[data-test="change-la-info-template-comments"]',
        commentsInput: '[id="la-info-template-comments"]',
        commentsValue: '[id="la-info-template-comments"]',
        sharepointLinkLink: '[data-test="change-la-info-template-sharepoint-link"]',
        sharepointLinkInput: '[id="la-info-template-sharepoint-link"]',
        sharepointLinkValue: '[id="la-info-template-sharepoint-link"]',
        saveButton: '[data-cy="select-common-submitbutton"]',
        completeCheckbox: 'la-info-template-complete',
    };

    public changeTemplateDates(sentDate: Date, returnedDate: Date): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.templateSentLink).click();
        cy.enterDate(
            this.selectors.templateSentInput,
            sentDate.getDate(),
            sentDate.getMonth() + 1,
            sentDate.getFullYear()
        );
        cy.enterDate(
            this.selectors.templateReturnedInput,
            returnedDate.getDate(),
            returnedDate.getMonth() + 1,
            returnedDate.getFullYear()
        );
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getTemplateSentDate(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.templateSentValue);
    }

    public getTemplateReturnedDate(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.templateReturnedValue);
    }

    public changeComments(comment: string): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.commentsLink).click();
        cy.get(this.selectors.commentsInput).clear();
        cy.get(this.selectors.commentsInput).type(comment);
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getComments(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.commentsValue);
    }

    public changeSharepointLink(sharepointLink: string): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.sharepointLinkLink).click();
        cy.get(this.selectors.sharepointLinkInput).clear();
        cy.get(this.selectors.sharepointLinkInput).type(sharepointLink);
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getSharepointLink(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.sharepointLinkValue);
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
}

const localAuthorityInformation = new LocalAuthorityInformation();

export default localAuthorityInformation;
