/// <reference types ='Cypress'/>

Cypress._.each(['ipad-2'], (viewport) => {
  describe(`109473: Filter projects by deliver officer ${viewport}`, () => {
    beforeEach(() => {
      cy.viewport(viewport)
      cy.login()
      cy.navigateToFilterProjects()
    })

    afterEach(() => {
      cy.clearCookies();
    });

    it('TC01: should display all the Non assigned projects when Not Assigned option is selected', () => {
      cy.get('[data-cy=asyncmethodbuildercore-start-not-assigned]').click();
      cy.get('[data-cy=select-projectlist-filter-apply]').click();
      cy.get('[data-cy="select-projectlist-filter-count"]').should('contain', 'projects found');
      cy.get('[data-cy="select-projectlist-filter-count"]').should('not.contain', '0 projects found');
      cy.get('#delivery-officer-0 > .empty').should('have.text', 'Empty');     
    });

    it('TC02: should display all the projects assigned to the DO when a particular DO name option is selected', () => {
      cy.get('[data-cy=asyncmethodbuildercore-start-adrian-horan]').click();
      cy.get('[data-cy=select-projectlist-filter-apply]').click();
      cy.get('[data-cy="select-projectlist-filter-count"]').should('not.contain', '0 projects found');
      cy.get('#delivery-officer-0').should('have.text', 'Delivery officer: Adrian Horan');
    });
  })
});

  