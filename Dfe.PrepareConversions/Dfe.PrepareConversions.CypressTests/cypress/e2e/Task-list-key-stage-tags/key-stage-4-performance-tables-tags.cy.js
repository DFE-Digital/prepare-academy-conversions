/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`119883 Verify Key stage 4 data status on ${viewport}`, () => {
        beforeEach(() => {
            cy.login()
            cy.viewport(viewport)
            cy.navigateToFilterProjects();
            cy.get('[data-cy="select-projectlist-filter-title"]').type('Caludon Castle School')
            cy.get('[data-cy=select-projectlist-filter-apply]').click();
            cy.get('[data-cy="select-projectlist-filter-count"]')
              .invoke('text')
              .then((text) => {
                if( text == '0 projects found'){
                    cy.log('Expected result not found');
                    Cypress.runner.stop();
                }
            else {
                cy.get('[data-cy="select-projectlist-filter-count"]').should('not.equal', '0 projects found');
                cy.get('#school-name-0').click()
                cy.get('*[href*="/confirm-school-trust-information-project-dates"]').should('be.visible')
            }
          });
        })

        it('TC01: Navigates to Key stage 4 performance tables page', () => {
            cy.get('[aria-describedby="key-stage-4-performance-tables"]').click();
            cy.url().should('contain', '/key-stage-4-performance-tables')
            cy.get('.govuk-grid-row > :nth-child(2) > :nth-child(2)')
                .invoke('text')
                .then((text) => {
                    cy.log(text)
                    cy.get('[class="govuk-tag govuk-tag--orange"]').should('contain', 'Revised');
                    cy.get('[class="govuk-tag govuk-tag--green"]').should('contain', 'Final');
                });
        });

        it('TC02: Navigates to Key stage 4 performance tables on Preview page ', () => {
            cy.get('[id="preview-project-template-button"]').click();
            cy.url().should('include', '/preview-project-template');
            cy.get('h1').contains('Preview project template')
            cy.get('#key-stage-4-performance-tables').should('have.text', 'Key stage 4 performance tables')
            cy.get(':nth-child(19) > .app-summary-card__body')
                .invoke('text')
                .then((text) => {
                    cy.log(text)
                    cy.get('[class="govuk-tag govuk-tag--orange"]').should('contain', 'Revised');
                    cy.get('[class="govuk-tag govuk-tag--green"]').should('contain', 'Final');
                });
        });

    });
});
