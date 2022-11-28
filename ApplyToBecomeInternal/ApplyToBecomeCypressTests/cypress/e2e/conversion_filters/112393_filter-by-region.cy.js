/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`112393: Filter projects by region ${viewport}`, () => {
      beforeEach(() => {
        cy.viewport(viewport);
        cy.login();
        cy.navigateToFilterProjects();
      })
  
      afterEach(() => {
        cy.clearCookies();
      });
  
      it('TC01: should display results when region option is checked', () => {
        cy.get('[data-cy="select-projectlist-filter-region-South West"]').click();
        cy.get('[data-cy=select-projectlist-filter-apply]').click();
        cy.get('[data-cy="select-projectlist-filter-row"]').should('be.visible');
        cy.get('[data-cy="select-projectlist-filter-banner"]').should('be.visible');
        cy.get('[data-cy="select-projectlist-filter-count"]').should('not.have.text', '0 projects found'); 
      });   
  
      it('TC02: should display all the results when all filters are cleared', () => {
        cy.clearFilters();
        cy.get('[data-cy="select-projectlist-filter-banner"]').should('not.exist');
        cy.get('[data-cy="select-projectlist-filter-count"]').should('not.have.text', '0 projects found');    
      });
  
      it('TC03: should display results when multiple filter options are checked', () => {
        cy.get('[data-cy="select-projectlist-filter-title"]').type('School');
        cy.get('[data-cy="select-projectlist-filter-region-South West"]').click();
        cy.get('[data-cy=select-projectlist-filter-apply]').click();
        cy.get('[data-cy="select-projectlist-filter-banner"]').should('be.visible');
        cy.get('[data-cy="select-projectlist-filter-count"]').should('not.contain', '0 projects found');
      });
    });
  });
  