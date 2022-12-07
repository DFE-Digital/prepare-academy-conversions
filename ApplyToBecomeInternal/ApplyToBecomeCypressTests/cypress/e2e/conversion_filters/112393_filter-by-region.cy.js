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
        cy.get('[data-cy="select-projectlist-filter-region-West Midlands"]').click();
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
        cy.get('[data-cy="select-projectlist-filter-region-West Midlands"]').click();
        cy.get('[data-cy=select-projectlist-filter-apply]').click();
        cy.get('[data-cy="select-projectlist-filter-banner"]').should('be.visible');
        cy.get('[data-cy="select-projectlist-filter-count"]').should('not.contain', '0 projects found');
      });

      //skip until #114627 or #114481 is fixed
      it.skip('TC03: should display results when all the Region filter options are selected', () => {
        cy.get('[data-cy="select-projectlist-filter-region-East Midlands"]').click();
        cy.get('[data-cy="select-projectlist-filter-region-East of England"]').click();
        cy.get('[data-cy="select-projectlist-filter-region-London"]').click();
        cy.get('[data-cy="select-projectlist-filter-region-North East"]').click();
        cy.get('[data-cy="select-projectlist-filter-region-North West"]').click();
        cy.get('[data-cy="select-projectlist-filter-region-South East"]').click();
        cy.get('[data-cy="select-projectlist-filter-region-South West"]').click();
        cy.get('[data-cy="select-projectlist-filter-region-West Midlands"]').click();
        cy.get('[data-cy="select-projectlist-filter-region-Yorkshire and the Humber"]').click();
        cy.get('[data-cy=select-projectlist-filter-apply]').click();
        cy.get('[data-cy="select-projectlist-filter-banner"]').should('be.visible');
        cy.get('[data-cy="select-projectlist-filter-row"]').should('be.visible');
        cy.get('[data-cy="select-projectlist-filter-count"]').should('not.contain', '0 projects found');
      });
    });
  });
  