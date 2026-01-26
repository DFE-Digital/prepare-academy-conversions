/// <reference types="cypress" />

import BasePage from './basePage';
import { EnvUrl } from '../constants/cypressConstants';

export default class ProjectTaskList extends BasePage {
  static selectors = {
    assignProjectButton: 'a[href*="project-assignment"]',
    assignedUser: '[data-id="assigned-user"]',
    notificationMessage: '[id="notification-message"]',
    schoolOverviewLink: 'a[href*="/school-overview"]',
    schoolOverviewStatus: '[id="school-overview-status"]',
    budgetLink: 'a[href*="/budget"]',
    budgetStatus: '[id="school-budget-information-status"]',
    pupilForecastLink: 'a[href*="/pupil-forecast"]',
    conversionDetailsLink: 'a[href*="/conversion-details"]',
    conversionDetailsStatus: '[id="school-and-trust-information-status"]',
    publicSectorEqualityDutyLink: 'a[href*="/public-sector-equality-duty"]',
    publicSectorEqualityDutyStatus: '[data-cy="select-tasklist-psed-status"]',
    rationaleLink: 'a[href*="/confirm-project-trust-rationale"]',
    rationaleStatus: '[id="rationale-status"]',
    riskAndIssuesLink: 'a[href*="/confirm-risks-issues"]',
    riskAndIssuesStatus: '[id="risks-and-issues-status"]',
    LALink: 'a[href*="/confirm-local-authority-information-template-dates"]',
    LAStatus: '[id="la-info-template-status"]',
    ofstedLink: 'a[href*="/school-performance-ofsted-information"]',
    keyStageLink: (keyStageNumber: number) =>
      `a[href*="/key-stage-${keyStageNumber}-performance-tables"]`,
    createNewConversionButton: '[data-cy="create_new_conversion_btn"]',
    recordDecisionButton: '[data-cy="record_decision_error_btn"]',
    schoolName: '[data-cy="school-name"]',
    urn: '[data-cy="urn"]',
    urnId: '.govuk-caption-xl',
    acceptCookieBtn: '[data-test="cookie-banner-accept"]',
    hideAcceptCookieBtn: '#acceptCookieBanner > .govuk-button-group',
    previewProjectDocument: '#preview-project-template-button',
    createProjectDocument: '[data-test="generate-htb"]',
    errorMessage: '[data-cy="error-message-0+="]',
  };

  static path = 'task-list';

  static acceptCookieBtnClick(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.acceptCookieBtn).click();
  }

  static hideAcceptCookieBanner(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.hideAcceptCookieBtn).click();
  }

  static selectAssignProject(): void {
    cy.checkPath(this.path);
    this.acceptCookieBtnClick();
    this.hideAcceptCookieBanner();
    cy.get(this.selectors.assignProjectButton).click();
  }

  static getAssignedUser(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.assignedUser);
  }

  static getNotificationMessage(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.notificationMessage);
  }

  static selectSchoolOverview(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.schoolOverviewLink).click();
  }

  static getSchoolOverviewStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.schoolOverviewStatus);
  }

  static selectBudget(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.budgetLink).click();
  }

  static getBudgetStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.budgetStatus);
  }

  static selectPupilForecast(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.pupilForecastLink).click();
  }

  static selectConversionDetails(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.conversionDetailsLink).click();
  }

  static getHomePage(): void {
    cy.visit(`${Cypress.env(EnvUrl)}/`);
  }

  static getConversionDetailsStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.conversionDetailsStatus);
  }

  static selectPublicSectorEqualityDuty(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.publicSectorEqualityDutyLink).click();
  }

  static publicSectorEqualityDutyStatus(): Cypress.Chainable<
    JQuery<HTMLElement>
  > {
    cy.checkPath(this.path);
    return cy.get(this.selectors.publicSectorEqualityDutyStatus);
  }

  static selectRationale(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.rationaleLink).click();
  }

  static getRationaleStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.rationaleStatus);
  }

  static selectRisksAndIssues(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.riskAndIssuesLink).click();
  }

  static getRisksAndIssuesStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.riskAndIssuesStatus);
  }

  static selectLA(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.LALink).click();
  }

  static getLAStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.LAStatus);
  }

  static selectOfsted(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.ofstedLink).click();
  }

  static getProjectUrn(): Cypress.Chainable<string> {
    cy.checkPath(this.path);
    return cy
      .get(this.selectors.urnId)
      .invoke('text')
      .then((text) => {
        return text.replace('URN: ', '').trim();
      });
  }

  static clickOfStedINfoBackButton(): void {
    cy.get('[data-cy="ofsted-info-back-btn"]').click();
  }

  static selectKeyStage(keyStageNumber: number): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.keyStageLink(keyStageNumber)).click();
  }

  static clickCreateNewConversionBtn(): typeof ProjectTaskList {
    cy.get(this.selectors.createNewConversionButton).click();
    return this;
  }

  static selectRecordDecision(): typeof ProjectTaskList {
    cy.get(this.selectors.recordDecisionButton).click();
    return this;
  }

  static verifyProjectDetails(
    urn: string,
    schoolName: string
  ): typeof ProjectTaskList {
    cy.get(this.selectors.schoolName).should('contain', schoolName);
    cy.get(this.selectors.urn).should('contain', urn);
    return this;
  }

  static clickPreviewProjectDocumentButton(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.previewProjectDocument).click();
  }

  static clickCreateProjectDocumentButton(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.createProjectDocument).click();
  }

  static getErrorMessage(): Cypress.Chainable<JQuery<HTMLElement>> {
    return cy.get(this.selectors.errorMessage);
  }
}
