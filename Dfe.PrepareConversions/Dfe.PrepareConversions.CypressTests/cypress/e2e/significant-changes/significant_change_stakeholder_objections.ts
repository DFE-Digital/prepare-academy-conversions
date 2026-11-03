/// <reference types="cypress" />
const TASK_LIST_URL = /^.*\/significant-change\/task-list\/\d+(\?.*)?$/;

const navigateToTaskPage = () => {
    cy.login();
    cy.acceptCookies();
    cy.visit('/significant-change/project-list');
    cy.getById('school-name-0').click();
    cy.contains('a', 'Other stakeholder engagement').click();
};

describe('Other stakeholder engagement form', () => {
    beforeEach(() => {
        navigateToTaskPage();
    });

    it('shows details only for the answers that require them', () => {
        cy.getByDataTest('trust-consulted-stakeholders-not-consulted-reason').should('not.be.visible');
        cy.getByDataTest('stakeholder-objections-details').should('not.be.visible');
        cy.getByDataTest('stakeholders-notified-reason').should('not.be.visible');

        cy.getByDataTest('trust-consulted-stakeholders-no').check();
        cy.getByDataTest('trust-consulted-stakeholders-not-consulted-reason').should('be.visible');
        cy.getByDataTest('trust-consulted-stakeholders-yes').check();
        cy.getByDataTest('trust-consulted-stakeholders-not-consulted-reason').should('not.be.visible');

        cy.getByDataTest('objections-raised-yes').check();
        cy.getByDataTest('stakeholder-objections-details').should('be.visible');
        cy.getByDataTest('objections-raised-no').check();
        cy.getByDataTest('stakeholder-objections-details').should('not.be.visible');

        cy.getByDataTest('stakeholders-notified-No').check();
        cy.getByDataTest('stakeholders-notified-reason').should('be.visible');
        cy.getByDataTest('stakeholders-notified-Yes').check();
        cy.getByDataTest('stakeholders-notified-reason').should('not.be.visible');
    });

    it('requires conditional details and saves all three answers together', () => {
        cy.contains('button', 'Save and return').click();
        cy.get('.govuk-error-summary').should('contain.text', 'Select an option');

        cy.getByDataTest('trust-consulted-stakeholders-yes').check();
        cy.getByDataTest('objections-raised-yes').check();
        cy.getByDataTest('stakeholders-notified-Yes').check();
        cy.contains('button', 'Save and return').click();
        cy.getById('StakeholderObjectionsDetails-error').should('contain.text', 'Add details');

        cy.getByDataTest('stakeholder-objections-details').type('Objections were addressed.');
        cy.getByDataTest('other-stakeholder-supporting-evidence').type('https://example.test/evidence');
        cy.contains('button', 'Save and return').click();
        cy.urlPath().should('match', TASK_LIST_URL);
    });
});
