/// <reference types="cypress" />
import projectList from '../../pages/projectList';
import projectTaskList from '../../pages/projectTaskList';
import requestFinancialHealthAssessment from '../../pages/requestFinancialHealthAssessment';
import { Logger } from '../../support/logger';

describe('Request Financial Health Assessment (SFSO commissioning)', () => {
    // Shows for all conversion routes (Voluntary + Sponsored). Point at any seeded, editable conversion project.
    const projectName = 'Gloucester school';
    const overview = 'SFSO commissioning overview - case complexity notes for the funding delivery team.';

    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage and open a seeded conversion project');
        cy.visit('/');
        cy.acceptCookies();
        projectList.selectProject(projectName);
    });

    it('shows the Financials task on the project task list with a valid status', () => {
        Logger.log('Status is derived from the proposed decision date: Not started or Completed');
        projectTaskList
            .getRequestFinancialHealthAssessmentStatus()
            .should('be.visible')
            .invoke('text')
            .then((text) => {
                expect(text.trim()).to.match(/Not started|Completed/);
            });
    });

    it('shows the not-requested hint only when the task is Not started', () => {
        Logger.log('Hint appears under the row only when no proposed decision date is set');
        projectTaskList
            .getRequestFinancialHealthAssessmentStatus()
            .invoke('text')
            .then((status) => {
                if (status.trim() === 'Not started') {
                    projectTaskList.getRequestFinancialHealthAssessmentHint().should('be.visible');
                } else {
                    cy.get('[data-test="fha-not-requested-hint"]').should('not.exist');
                }
            });
    });

    it('opens the task page from the task list and shows the overview field', () => {
        projectTaskList.selectRequestFinancialHealthAssessment();
        requestFinancialHealthAssessment.verifyOnPage();
        requestFinancialHealthAssessment.getOverview().should('exist');
    });

    it('shows an FHA request message, or a prompt to set the decision date', () => {
        projectTaskList.selectRequestFinancialHealthAssessment();
        requestFinancialHealthAssessment.verifyOnPage();

        Logger.log('Scenario 5/6/past when a proposed decision date is set; Scenario 7 banner when it is not');
        cy.get('body').then(($body) => {
            if ($body.find('[data-test="fha-requested-date"]').length > 0) {
                requestFinancialHealthAssessment
                    .getRequestedDate()
                    .invoke('text')
                    .should('match', /has been sent|was requested on|proposed decision date is on/);
            } else {
                requestFinancialHealthAssessment.getNoDecisionDateBanner().should('be.visible');
            }
        });
    });

    it('saves an overview of 250 characters or fewer and returns to Project details', () => {
        projectTaskList.selectRequestFinancialHealthAssessment();
        requestFinancialHealthAssessment.enterOverview(overview);
        requestFinancialHealthAssessment.saveAndReturn();

        Logger.log('Back on Project details (task-list root URL, not the FHA sub-page)');
        cy.url().should('match', /\/task-list\/\d+$/);
        projectTaskList.getRequestFinancialHealthAssessmentStatus().should('be.visible');

        Logger.log('Re-open and confirm the overview persisted');
        projectTaskList.selectRequestFinancialHealthAssessment();
        requestFinancialHealthAssessment.getOverview().should('have.value', overview);
    });

    it('shows an error when the overview is more than 250 characters', () => {
        projectTaskList.selectRequestFinancialHealthAssessment();
        requestFinancialHealthAssessment.enterOverview('a'.repeat(251));
        requestFinancialHealthAssessment.saveAndReturn();

        requestFinancialHealthAssessment
            .getErrorSummary()
            .should('be.visible')
            .and('contain.text', 'Overview must be 250 characters or less');
        cy.checkPath('request-financial-health-assessment');
    });
});
