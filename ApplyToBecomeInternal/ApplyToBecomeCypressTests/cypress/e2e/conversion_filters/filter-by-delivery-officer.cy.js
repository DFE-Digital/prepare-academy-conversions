/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`109473: Filter projects by deliver officer ${viewport}`, () => {
      beforeEach(() => {
        cy.viewport(viewport);
        cy.login();
        cy.navigateToFilterProjects();
      })
  
      afterEach(() => {
        cy.clearCookies();
      });
  
      it('TC01: should display Not assigned filter option at the top', () => {
        cy.get(':nth-child(4) > .govuk-form-group > .govuk-fieldset > .govuk-checkboxes > :nth-child(1)')
          .should('contain', 'Not assigned'); 
      });   
  
      it('TC02: should display all the Non assigned projects when Not Assigned option is selected', () => {
        cy.get('[data-cy="select-projectlist-filter-officer-not-assigned"]').click();
        cy.get('[data-cy=select-projectlist-filter-apply]').click();
        cy.get('[data-cy="select-projectlist-filter-count"]').should('contain', 'projects found');
        cy.get('[data-cy="select-projectlist-filter-count"]').should('not.contain', '0 projects found');
        cy.get('#delivery-officer-0 > .empty').should('have.text', 'Empty');     
      });
  
      it('TC03: should display all the projects assigned to the DO when a particular DO name option is selected', () => {
        cy.get('[name="selectedOfficers"]').eq(1).click();
        cy.get('[data-cy=select-projectlist-filter-apply]').click();
        cy.get('[data-cy="select-projectlist-filter-count"]').should('not.contain', '0 projects found');
        cy.get('#delivery-officer-0').should('not.have.text', 'Delivery officer: Empty');
      });
    });
  });
  