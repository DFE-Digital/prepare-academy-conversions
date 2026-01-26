/// <reference types="cypress" />

import BasePage from './basePage';

export default class LocalAuthorityInfomation extends BasePage {
    static selectors = {
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
        completeCheckbox: '[id="la-info-template-complete"]',
    };

    static path = 'confirm-local-authority-information-template-dates';

    static changeTemplateDates(sentDate: Date, returnedDate: Date): void {
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
    }

    static getTemplateSentDate(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.templateSentValue);
    }

    static getTemplateReturnedDate(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.templateReturnedValue);
    }

    static changeComments(comment: string): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.commentsLink).click();
        cy.get(this.selectors.commentsInput).clear();
        cy.get(this.selectors.commentsInput).type(comment);
        cy.get(this.selectors.saveButton).click();
    }

    static getComments(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.commentsValue);
    }

    static changeSharepointLink(sharepointLink: string): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.sharepointLinkLink).click();
        cy.get(this.selectors.sharepointLinkInput).clear();
        cy.get(this.selectors.sharepointLinkInput).type(sharepointLink);
        cy.get(this.selectors.saveButton).click();
    }

    static getSharepointLink(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.sharepointLinkValue);
    }

    static markComplete(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.completeCheckbox).check();
    }

    static markIncomplete(): void {
        cy.checkPath(this.path);
        cy.get(this.selectors.completeCheckbox).uncheck();
    }
}
