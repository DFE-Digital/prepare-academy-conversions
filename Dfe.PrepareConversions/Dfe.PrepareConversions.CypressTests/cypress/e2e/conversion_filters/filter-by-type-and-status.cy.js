/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
  describe(`109473: Filter projects by type and status${viewport}`, () => {
    beforeEach(() => {
      cy.viewport(viewport);
      cy.login();
      cy.navigateToFilterProjects();
    })

    afterEach(() => {
      cy.clearCookies();
    });

    it('TC01: results should display correct format after applying filter', () => {
      cy.get('[class="govuk-checkboxes__input"]').first().click();
      cy.get('[data-cy=select-projectlist-filter-apply]').click();
      cy.get('[id="govuk-notification-banner-title"]')
        .should('contain', 'Success')
      cy.get('[data-cy="select-projectlist-filter-count"]')
        .invoke('text')
        .should('match', /^[0-9]\D*/);
    });

    it('TC02: should display 0 count when no match found for the projects', () => {
      cy.get('[data-cy="select-projectlist-filter-title"]').type('test')
      cy.get('[data-cy=select-projectlist-filter-apply]').click();
      cy.get('[data-cy="select-projectlist-filter-count"]').should('contain', '0 projects found');
    });

    it('TC03: should display result when  match found for the project title', () => {
      cy.get('[data-cy="select-projectlist-filter-title"]').type('school')
      cy.get('[data-cy=select-projectlist-filter-apply]').click();
      cy.get('[data-cy="select-projectlist-filter-count"]').should('not.equal', '0 projects found');
    });

    it('TC04: should display count when match found in the results on applying filter', () => {
      cy.get('[data-cy="select-projectlist-filter-status-Declined"]').click();
      cy.get('[data-cy=select-projectlist-filter-apply]').click();
      cy.get('[data-cy="select-projectlist-filter-row"]')
      cy.get('[data-cy="select-projectlist-filter-count"]')
        .then(($text) => {
          expect($text).not.to.have.text('0 projects found');
        });
    });

    it('TC05: should display count when match found in the results for project status', () => {
      cy.get('[data-cy="select-projectlist-filter-status-Approved"]').click();
      cy.get('[data-cy=select-projectlist-filter-apply]').click();
      cy.get('[test-id="projectCount"]')
        .invoke('text')
        .then((text) => {
          cy.log(text)
          const total_count = text.replace('projects found', '').trim()
          if (total_count > 0) {
            cy.get('[data-cy="select-projectlist-filter-row"]').should('be.visible')
          }
          else {
            cy.log('No match found')
          }
        });
    });
  });
});
