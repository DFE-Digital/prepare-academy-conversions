/// <reference types="cypress" />
import BasePage from './basePage';

class ProjectTaskList extends BasePage {
    public path = 'task-list';

    private selectors = {
        assignProjectButton: 'a[href*="project-assignment"]',
        assignedUser: 'assigned-user',
        notificationMessage: 'notification-message',
        schoolOverviewLink: 'a[href*="/school-overview"]',
        schoolOverviewStatus: 'school-overview-status',
        budgetLink: 'a[href*="/budget"]',
        budgetStatus: 'school-budget-information-status',
        pupilForecastLink: 'a[href*="/pupil-forecast"]',
        conversionDetailsLink: 'a[href*="/conversion-details"]',
        conversionDetailsStatus: 'school-and-trust-information-status',
        publicSectorEqualityDutyLink: 'a[href*="/public-sector-equality-duty"]',
        publicSectorEqualityDutyStatus: 'select-tasklist-psed-status',
        rationaleLink: 'a[href*="/confirm-project-trust-rationale"]',
        rationaleStatus: 'rationale-status',
        riskAndIssuesLink: 'a[href*="/confirm-risks-issues"]',
        riskAndIssuesStatus: 'risks-and-issues-status',
        LALink: 'a[href*="/confirm-local-authority-information-template-dates"]',
        LAStatus: 'la-info-template-status',
        ofstedLink: 'a[href*="/school-performance-ofsted-information"]',
        keyStageLink: (keyStageNumber: number) => `a[href*="/key-stage-${keyStageNumber}-performance-tables"]`,
        createNewConversionButton: 'create_new_conversion_btn',
        recordDecisionButton: 'record_decision_error_btn',
        schoolName: 'school-name',
        urn: 'urn',
        urnId: '.govuk-caption-xl',
        acceptCookieBtn: 'cookie-banner-accept',
        hideAcceptCookieBtn: '#acceptCookieBanner > .govuk-button-group',
        previewProjectDocument: 'preview-project-template-button',
        createProjectDocument: 'generate-htb',
        errorMessage: 'error-message-0+=',
    };

    public acceptCookieBtnClick(): this {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.acceptCookieBtn).click();
        return this;
    }

    public hideAcceptCookieBanner(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.hideAcceptCookieBtn).click();
        return this;
    }

    public selectAssignProject(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.assignProjectButton).click();
        return this;
    }

    public getAssignedUser(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getByDataId(this.selectors.assignedUser);
    }

    public getNotificationMessage(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.notificationMessage);
    }

    public selectSchoolOverview(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.schoolOverviewLink).click();
        return this;
    }

    public getSchoolOverviewStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.schoolOverviewStatus);
    }

    public selectBudget(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.budgetLink).click();
        return this;
    }

    public getBudgetStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.budgetStatus);
    }

    public selectPupilForecast(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.pupilForecastLink).click();
        return this;
    }

    public selectConversionDetails(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.conversionDetailsLink).click();
        return this;
    }

    public getConversionDetailsStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.conversionDetailsStatus);
    }

    public selectPublicSectorEqualityDuty(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.publicSectorEqualityDutyLink).click();
        return this;
    }

    public publicSectorEqualityDutyStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getByDataCy(this.selectors.publicSectorEqualityDutyStatus);
    }

    public selectRationale(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.rationaleLink).click();
        return this;
    }

    public getRationaleStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.rationaleStatus);
    }

    public selectRisksAndIssues(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.riskAndIssuesLink).click();
        return this;
    }

    public getRisksAndIssuesStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.riskAndIssuesStatus);
    }

    public selectLA(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.LALink).click();
        return this;
    }

    public getLAStatus(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.LAStatus);
    }

    public selectOfsted(): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.ofstedLink).click();
        return this;
    }

    public getProjectUrn(): Cypress.Chainable<string> {
        cy.checkPath(this.path);
        return cy
            .get(this.selectors.urnId)
            .invoke('text')
            .then((text) => {
                return text.replace('URN: ', '').trim();
            });
    }

    public clickOfStedINfoBackButton(): this {
        cy.getByDataCy('ofsted-info-back-btn').click();
        return this;
    }

    public selectKeyStage(keyStageNumber: number): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.keyStageLink(keyStageNumber)).click();
        return this;
    }

    public clickCreateNewConversionBtn(): this {
        cy.getByDataCy(this.selectors.createNewConversionButton).click();
        return this;
    }

    public selectRecordDecision(): this {
        cy.getByDataCy(this.selectors.recordDecisionButton).click();
        return this;
    }

    public verifyProjectDetails(urn: string, schoolName: string): this {
        cy.getByDataCy(this.selectors.schoolName).should('contain', schoolName);
        cy.getByDataCy(this.selectors.urn).should('contain', urn);
        return this;
    }

    public clickPreviewProjectDocumentButton(): this {
        cy.checkPath(this.path);
        cy.getById(this.selectors.previewProjectDocument).click();
        return this;
    }

    public clickCreateProjectDocumentButton(): this {
        cy.checkPath(this.path);
        cy.getByDataTest(this.selectors.createProjectDocument).click();
        return this;
    }

    public getErrorMessage(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.getByDataCy(this.selectors.errorMessage);
    }
}

const projectTaskList = new ProjectTaskList();

export default projectTaskList;
