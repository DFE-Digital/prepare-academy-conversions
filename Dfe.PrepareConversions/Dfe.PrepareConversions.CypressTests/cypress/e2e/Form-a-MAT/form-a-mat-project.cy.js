/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`122608 Verify the details of Form-a-MAT project and generate it's template on ${viewport}`, () => {
      beforeEach(() => {
        cy.login()
        cy.viewport(viewport)
        cy.get('[data-cy="select-projectlist-filter-row"]').first().should('be.visible')
        .invoke('text')
        .then((text) => {
          if (text.includes('Route: Form a MAT')) {
            cy.not.contains('Delivery officer')
            cy.get('[id="school-name-0"]').click();
            cy.url().should('include', '/task-list')
            cy.get('[aria-describedby="school-and-trust-information-status"]').click()
            cy.url().should('include', '/confirm-school-trust-information-project-dates')
          }
          else {
            cy.log('this is not involuntary project')
          }
        });
      })

      afterEach(() => {
        cy.clearCookies();
      });

      it('TC01: should display correct details of the project and generate template', () => {
        cy.url().should('include', 'project-list')
        cy.get('[aria-label="sort-by"]').should('contain.text', 'Sorted by: Project created date')
        cy.get('[data-cy="select-projectlist-filter-row"]').should('contain.text', 'Route: Form a MAT')
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
