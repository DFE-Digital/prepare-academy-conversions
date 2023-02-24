/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
  describe(`119898 Create Involuntry project on ${viewport}`, () => {
    beforeEach(() => {
      cy.login()
      cy.viewport(viewport)
    })

    afterEach(() => {
      cy.clearCookies();
    });

    it('TC01: should display involuntry project in the project list with the correct details', () => {
      cy.createInvoluntaryProject();
      cy.url().should('include', 'project-list')
      cy.get('[aria-label="sort-by"]').should('contain.text', 'Sorted by: Project created date')
      cy.get('[data-cy="route"]').should('contain.text', 'Route: Involuntary conversion')
      cy.get('[id="application-received-date-0"]').should('contain.text', 'Project created date: ')
      cy.get('[id="school-name-0"]').click()
      cy.url().should('include', 'task-list')
    });
  })
})
