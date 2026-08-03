/// <reference types="cypress" />
import significantChangeProjectAssignment from '../../pages/significantChangeProjectAssignment';
import { Logger } from '../../support/logger';

describe('Significant change project assignment', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    it('Should open the significant change project assignment page', () => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getById('school-name-0').click();

                    cy.url().then((taskListUrl) => {
                        const projectId = taskListUrl.split('/').pop();
                        cy.visit(`/significant-change/project-assignment/${projectId}`);

                        significantChangeProjectAssignment.verifyPageIsVisible();
                        cy.getById('unassign-link').should('be.visible');
                    });
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });

    it('Should assign and unassign a significant change project', () => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getById('school-name-0').click();

                    cy.url().then((taskListUrl) => {
                        const projectId = taskListUrl.split('/').pop() as string;

                        cy.visit(`/significant-change/project-assignment/${projectId}`);

                        significantChangeProjectAssignment
                            .assignFirstAvailableDeliveryOfficer()
                            .then((deliveryOfficer) => {
                                cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
                                cy.getById('notification-message').should('contain.text', 'Project is assigned');
                                cy.getByDataId('assigned-user').should('contain.text', deliveryOfficer);

                                cy.visit(`/significant-change/project-assignment/${projectId}`);
                                significantChangeProjectAssignment.unassignDeliveryOfficer();

                                cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
                                cy.getById('notification-message').should('contain.text', 'Project is unassigned');
                                cy.getByDataId('assigned-user').should('contain.text', 'Empty');
                            });
                    });
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });
});
