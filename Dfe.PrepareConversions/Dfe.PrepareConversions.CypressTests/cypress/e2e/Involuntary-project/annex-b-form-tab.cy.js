/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`126074: Verify Annex B form tab for involuntary project ${viewport}`, { tags: ['@dao'] }, () => {
        beforeEach(() => {
            cy.login({ titleFilter: 'Gloucester school' })
            cy.viewport(viewport)
            cy.get('[data-cy="select-projectlist-filter-row"]').first().should('be.visible')
                .invoke('text')
                .then((text) => {
                    if (text.includes('Route: Involuntary conversion')) {
                        cy.get('[id="school-name-0"]').click()
                        cy.url().should('include', '/task-list')
                        cy.get(':nth-child(1) > .moj-sub-navigation__link').should('contain', 'Task list')
                        cy.get(':nth-child(3) > .moj-sub-navigation__link').should('contain', 'Project notes')
                        cy.get(':nth-child(2) > .moj-sub-navigation__link').should('contain', 'Annex B form')
                            .click()
                        cy.title('Confirm Annex B received - Manage an academy conversion')
                        cy.get('[data-cy="select-annex-b-form-returned-change"] > .govuk-link').should('contain', 'Change')
                            .click()
                        cy.title('Have you saved the Annex B form - Manage an academy conversion')
                        cy.get('.govuk-heading-l').should('have.text', `Have you saved the school's completed Annex B form in SharePoint?`)
                        cy.get('.govuk-details__summary-text').should('contain', 'What is the Annex B form?')
                    }
                    else {
                        cy.log('this is not involuntary project')
                    }
                });
        });

        it('TC01: Changes should show in summary page when Yes option is selected', () => {
            cy.get('[data-cy="select-radio-yes"]').click()
            cy.get('[data-cy="select-sharepoint-url"]').type('test link')
            cy.get('[data-cy="select-save-and-continue"]').click()
            cy.get('[data-cy="select-annex-b-form-returned-answer"]').should('contain', 'Yes')
            cy.get('[data-cy="select-annex-b-sharepoint-link"] > .govuk-summary-list__key').should('be.visible')
            cy.get('[data-cy="select-annex-b-sharepoint-change"] > .govuk-link').should('be.visible')
        })

        it('TC02: Changes should show in summary page when No option is selected', () => {
            cy.get('[data-cy="select-radio-no"]').click()
            cy.get('[data-cy="select-save-and-continue"]').click()
            cy.get('[data-cy="select-annex-b-form-returned-answer"]').should('contain', 'No')
        })

        it('TC03: Error message should show when user does not enter link for option Yes', () => {
            cy.get('[data-cy="select-radio-yes"]').click()
            cy.get('[data-cy="select-sharepoint-url"]')
            cy.get('[data-cy="select-save-and-continue"]').click()
            cy.get('#AnnexFormUrl-error').should('contain', 'You must enter valid link for the Annex B form')
            cy.get('.govuk-error-summary')
            cy.get('[data-cy="error-summary"] > li').should('contain', 'You must enter valid link for the Annex B form')
        })
    });
});