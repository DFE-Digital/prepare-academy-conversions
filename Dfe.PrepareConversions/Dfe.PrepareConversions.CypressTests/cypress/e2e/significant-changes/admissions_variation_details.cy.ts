/// <reference types="cypress" />
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import admissionsVariationDetails from '../../pages/admissionsVariationDetails';
import { Logger } from '../../support/logger';

describe('Significant change - admissions variation details', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    const openTask = (then: (projectId: string) => void) => {
        cy.visit('/significant-change/project-list');
        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count === 0) {
                    Logger.log('No significant change projects available - skipping');
                    cy.contains('There are no matching results.').should('be.visible');
                    return;
                }

                cy.getById('school-name-0').click();
                cy.url().then((url) => {
                    const projectId = url.split('?')[0].split('/').pop() as string;
                    significantChangeTaskList.openAdmissionsVariationDetailsTask();
                    then(projectId);
                });
            });
    };

    it('shows the admissions variation details task in its section', () => {
        cy.visit('/significant-change/project-list');
        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getById('school-name-0').click();
                    cy.contains('h3.app-task-list__section', 'Admissions variation')
                        .next('.app-task-list')
                        .contains('a', 'Admissions variation details')
                        .should('be.visible');
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });

    it('saves No with optional evidence and does not show the second page', () => {
        openTask(() => {
            admissionsVariationDetails
                .verifyFirstPage()
                .chooseNo()
                .enterSupportingEvidence('Admissions policy reviewed')
                .continue();

            cy.url().should('match', /\/significant-change\/task-list\/\d+(\?.*)?$/);
        });
    });

    it('shows the second page for Yes and only saves after all three answers are supplied', () => {
        openTask(() => {
            admissionsVariationDetails.verifyFirstPage().chooseYes().continue();
            admissionsVariationDetails.verifySupportPage();
            admissionsVariationDetails.continue();

            cy.contains('Select an option').should('be.visible');

            admissionsVariationDetails.answerSupportQuestions().continue();
            cy.url().should('match', /\/significant-change\/task-list\/\d+(\?.*)?$/);
        });
    });
});
