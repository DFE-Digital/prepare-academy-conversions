/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
  describe(`119898 Change school for Involuntry project on ${viewport}`, () => {
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
      cy.contains('Change').should('be.visible').click();
      cy.get('.govuk-label').should('contain', 'What is the school name?');
      cy.get('[id="SearchQuery"]').first().clear();
      cy.get('[id="SearchQuery"]').first().type('sch');
      cy.get('#SearchQuery__option--0').click();
      cy.get('[data-id="submit"]').click();
      cy.url().should('include', 'start-new-project/check-school-trust-details');
      cy.get('[data-cy="school-name"]').should('include.text', 'Sch');
    });

    it('TC02: should update the changed trust', () => {
      cy.get('a[class="govuk-link"]').eq(4).should('contain', 'Change').click()
      cy.url().should('include', '/start-new-project/trust-name?urn')
      cy.get('.govuk-label').should('contain', 'What is the trust name?');
      cy.get('[id="SearchQuery"]').first().clear();
      cy.get('[id="SearchQuery"]').first().type('tri');
      cy.get('#SearchQuery__option--0').click();
      cy.get('[data-id="submit"]').click();
      cy.url().should('include', 'start-new-project/check-school-trust-details');
      cy.get('[data-cy="trust-name"]').should('include.text', 'Tri');
    });
  });
});
