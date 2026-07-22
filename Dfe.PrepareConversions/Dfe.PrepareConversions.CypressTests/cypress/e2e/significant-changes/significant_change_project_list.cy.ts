/// <reference types="cypress" />
import { Logger } from '../../support/logger';

describe('Significant change project list', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    it('Should navigate to significant change project list from service navigation', () => {
        cy.get('.govuk-service-navigation')
            .contains('a', 'Significant changes')
            .click();

        cy.checkPath('/significant-change/project-list');
        cy.getByDataCy('select-heading').should('contain.text', 'Significant change projects');
        cy.contains('.govuk-service-navigation__item--active', 'Significant changes').should('be.visible');
    });

    it('Should display project list rows or no-results state', () => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getByDataCy('select-projectlist-row').its('length').should('be.greaterThan', 0);
                    cy.getById('urn-0').should('contain.text', 'URN:');
                    cy.getById('incoming-trust-0').should('contain.text', 'Trust:');
                    cy.getById('tier-0').should('contain.text', 'Tier:');
                    cy.getById('type-and-route-0').should('contain.text', 'Route:');
                    cy.getById('assigned-to-0').should('contain.text', 'Unassigned');
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });
});
