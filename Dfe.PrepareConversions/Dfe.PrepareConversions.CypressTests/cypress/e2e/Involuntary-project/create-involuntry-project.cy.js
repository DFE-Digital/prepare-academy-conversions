/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
  describe(`119898 Create Involuntry project and generate it's template on ${viewport}`, { tags: ['@dao'] }, () => {
    beforeEach(() => {
      cy.login({ titleFilter: 'Gloucester school' })
      cy.viewport(viewport)
    })

    afterEach(() => {
      cy.clearCookies();
    });

    it('TC01: should display correct details of the project and generate template', () => {
      cy.createInvoluntaryProject();
      cy.url().should('include', 'project-list')
      cy.get('[aria-label="sort-by"]').should('contain.text', 'Sorted by: Project created date')
      cy.get('[data-cy="select-projectlist-filter-row"]').should('contain.text', 'Route: Involuntary conversion')
      cy.get('[id="application-received-date-0"]').should('contain.text', 'Project created date: ')
      cy.get('[id="school-name-0"]').click()
      cy.url().should('include', 'task-list')
      //check all the links for create project template
      cy.navigateToAllCreateProjectTemplateLinks()
      //preview project template
      cy.get('#preview-project-template-button').click()
      cy.get('h1').should('not.contain', 'Page not found')
      cy.get('[role="button"]').contains('Generate project template').click()
      cy.get('[data-cy="error-summary"]').click()
      cy.get('h1').should('contain', 'Set the advisory board date')
      cy.setAdvisoryBoardDate(12, 2, 2026)
      cy.get('h1').should('contain.text', 'Preview project template')
      //generate project template
      cy.get('[role="button"]').contains('Generate project template').click()
      cy.get('h1').should('not.contain', 'Page not found')
      cy.get('h1').should('contain.text', 'Download project template')
    });
  })
})
