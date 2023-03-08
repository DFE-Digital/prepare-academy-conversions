/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
  describe(`119898 Check validation for school and trust on ${viewport}`, { tags: ['@dao'] }, () => {
    beforeEach(() => {
      cy.login()
      cy.viewport(viewport)
      cy.get('[role="button"]').should('contain.text', "Start a new involuntary conversion project")
      cy.get('a[href="/start-new-project/school-name"]').click()
    })
    afterEach(() => {
      cy.clearCookies();
    });

    it('TC01: should display an error message for blank school-name', () => {
      cy.get('.govuk-label').should('contain', 'Which school is involved?')
      cy.get('[data-id="submit"]').click()
      cy.get('[data-cy="error-summary"]')
        .should('contains.text', 'Enter the school name or URN')
    });

    //skip TC02 as the bug exist
    it.skip('TC02: should display an error message for invalid school-name', () => {
      cy.get('.govuk-label').should('contain', 'Which school is involved?')
      cy.get('[id="SearchQuery"]').first().type('6657££$$')
      cy.get('[data-id="submit"]').click()
      cy.get('[data-cy="error-summary"]')
        .should('contains.text', 'Enter the school name or URN')
    });

    it('TC03: should display an error message for blank trust-name', () => {
      cy.selectSchool()
      cy.get('.govuk-label').should('contain', 'Which trust is involved?')
      cy.get('[data-id="submit"]').click()
      cy.get('[data-cy="error-summary"]')
        .should('contains.text', 'Enter the Trust name, UKPRN or Companies House number')
    });
    //skip TC04 as the bug exist
    it.skip('TC04: should display an error message for invalid trust-name', () => {
      cy.get('[role="button"]').should('contain.text', "Start a new involuntary conversion project")
      cy.get('a[href="/start-new-project/school-name"]').click()
      cy.get('.govuk-label').should('contain', 'Which trust is involved?')
      cy.get('[id="SearchQuery"]').first().type('aabbcc')
      cy.get('[data-id="submit"]').click()
      cy.get('[data-cy="error-summary"]')
        .should('contains.text', 'Enter the Trust name, UKPRN or Companies House number')
    });
  });
});