/* eslint-disable cypress/no-unnecessary-waiting */
/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
  describe(`119898 Change school for Involuntry project on ${viewport}`, { tags: ['@dao'] }, () => {
    beforeEach(() => {
      cy.login();
      cy.viewport(viewport);
      cy.get('[role="button"]').should('contain.text', "Start a new involuntary conversion project");
      cy.get('a[href="/start-new-project/school-name"]').click();
      cy.selectSchool();
      cy.selectTrust();
      cy.get('h1').should('contain.text', 'Check school and trust details');
    });

    it('TC01: should update the changed school', () => {
      cy.get('[data-cy="change-school"]').should('be.visible').click();
      cy.wait(500) //need to explicitly wait due to performance issue
      cy.get('.govuk-label').should('contain', 'Which school is involved?');
      cy.get('[id="SearchQuery"]').first().clear();
      cy.get('[id="SearchQuery"]').first().type('lon');
      cy.get('#SearchQuery__option--0').click();
      cy.get('[data-id="submit"]').click();
      cy.url().should('include', 'start-new-project/check-school-trust-details');
      cy.get('[data-cy="school-name"]').should('include.text', 'Lon');
    });

    it('TC02: should update the changed trust', () => {
      cy.get('[data-cy="change-trust"]').should('contain', 'Change').click()
      cy.wait(500) //need to explicitly wait due to performance issue
      cy.get('#query-hint').should('contain.text', 'Search by name, UKPRN or Companies House number.')
      cy.url().should('include', '/start-new-project/trust-name?urn')
      cy.get('.govuk-label').should('contain', 'Which trust is involved?');
      cy.get('[id="SearchQuery"]').first().clear();
      cy.get('[id="SearchQuery"]').first().type('bristol');
      cy.get('#SearchQuery__option--0').click();
      cy.get('[data-id="submit"]').click();
      cy.url().should('include', 'start-new-project/check-school-trust-details');
      cy.get('[data-cy="trust-name"]').should('include.text', 'BRISTOL');
    });
  });
});
