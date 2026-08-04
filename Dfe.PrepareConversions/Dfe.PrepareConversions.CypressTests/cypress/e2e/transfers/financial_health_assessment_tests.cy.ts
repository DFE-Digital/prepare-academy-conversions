/// <reference types="cypress" />
import { NewTransferProjectWithDecisions } from 'cypress/pages/newTransferProjectWithDecisions';
import transferFinancialHealthAssessment from '../../pages/transferFinancialHealthAssessment';
import { Logger } from '../../support/logger';

describe('Transfers - Financial Health Assessment (SFSO commissioning)', () => {
    const transferProject = new NewTransferProjectWithDecisions();
    const overview = 'SFSO commissioning overview - case complexity notes for the funding delivery team.';
    let projectId: string;

    beforeEach(() => {
        cy.login();
        Logger.log('Create a fresh transfer project and land on its task list');
        cy.visit('/transfers/home');
        cy.acceptCookies();

        transferProject
            .createNewTransferProject()
            .searchOutgoingTrust('Greater Manchester Academies Trust')
            .selectOutgoingTrust('10058252')
            .confirmOutgoingTrust()
            .selectOptionById('10030221')
            .submitForm()
            .selectOptionById('false')
            .submitForm()
            .selectOptionById('false')
            .submitForm()
            .createProjectButton();

        cy.url().then((url) => {
            const match = url.match(/project\/(\d+)/);
            projectId = match && match[1] ? match[1] : '';
        });
    });

    afterEach(() => {
        if (projectId) {
            transferProject.deleteProject(projectId);
        }
    });

    it('shows the Financial Health Assessment row with a valid status', () => {
        cy.getByDataTest('transfer-financial-health-assessment').should('be.visible');
        cy.getById('financial-health-assessment')
            .invoke('text')
            .then((text) => {
                expect(text.trim()).to.match(/Not started|Completed/);
            });
    });

    it('opens the FHA page and shows the request message or the no-decision-date banner', () => {
        cy.getByDataTest('transfer-financial-health-assessment').click();
        transferFinancialHealthAssessment.verifyOnPage();

        Logger.log('Scenario 5/6 message when a proposed decision date is set; Scenario 7 banner when it is not');
        cy.get('body').then(($body) => {
            if ($body.find('[data-test="fha-requested-date"]').length > 0) {
                transferFinancialHealthAssessment
                    .getRequestedDate()
                    .invoke('text')
                    .should('match', /has been sent|was requested on|proposed decision date is on/);
            } else {
                transferFinancialHealthAssessment.getNoDecisionDateBanner().should('be.visible');
            }
        });
    });

    it('saves an overview of 250 characters or fewer and returns to the task list', () => {
        cy.getByDataTest('transfer-financial-health-assessment').click();
        transferFinancialHealthAssessment.enterOverview(overview);
        transferFinancialHealthAssessment.saveAndReturn();

        Logger.log('Back on the transfer task list (not the FHA sub-page)');
        cy.url().should('include', '/transfers/project/');
        cy.url().should('not.include', 'financial-health-assessment');

        Logger.log('Re-open and confirm the overview persisted');
        cy.getByDataTest('transfer-financial-health-assessment').click();
        transferFinancialHealthAssessment.getOverview().should('have.value', overview);
    });

    it('caps the overview at 250 characters', () => {
        cy.getByDataTest('transfer-financial-health-assessment').click();
        transferFinancialHealthAssessment.enterOverview('a'.repeat(251));
        transferFinancialHealthAssessment.getOverview().invoke('val').its('length').should('eq', 250);
        transferFinancialHealthAssessment.saveAndReturn();
        cy.url().should('include', '/transfers/project/');
        cy.url().should('not.include', 'financial-health-assessment');
    });
});
