/// <reference types="cypress" />
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import { Logger } from '../../support/logger';

describe('Significant change task list', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    it('Should navigate to task list from significant change list and display mapped header details', () => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getById('school-name-0').click();

                    cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
                    significantChangeTaskList.verifyHeaderAndSubNavigation();
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });
});
