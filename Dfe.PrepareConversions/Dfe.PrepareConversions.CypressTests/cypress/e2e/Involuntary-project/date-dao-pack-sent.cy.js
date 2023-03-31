/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`121477: Add DAO pack sent page on Involuntry project on ${viewport}`, { tags: ['@dao'] }, () => {
        beforeEach(() => {
            cy.login({ titleFilter: 'Gloucester school' })
            cy.viewport(viewport)
            cy.get('[data-cy="select-projectlist-filter-row"]').first().should('be.visible')
                .invoke('text')
                .then((text) => {
                    if (text.includes('Route: Involuntary conversion')) {
                        cy.get('[id="school-name-0"]').click();
                        cy.url().should('include', '/task-list')
                        cy.get('[aria-describedby="school-and-trust-information-status"]').click()
                        cy.url().should('include', '/confirm-school-trust-information-project-dates')
                        cy.get('[class="govuk-summary-list"]').should('be.visible')
                            .invoke('text')
                            .then((text) => {
                                text.includes('Date directive academy order (DAO) pack sent')
                                cy.get('[data-test="change-dao-pack-sent-date"]').should('contain', 'Change')
                                    .click()
                                cy.get('h1').should('contain', 'Date you sent the directive academy order (DAO) pack to the school')
                            })
                    }
                    else {
                        cy.log('this is not involuntary project')
                        Cypress.runner.stop()
                    }
                });
        });

        it('TC01: Date DAO should display when user submitted the date', () => {
            cy.submitDAODate(11, 1, 2023)
            cy.get('#save-and-continue-button').click()
            cy.get('#dao-pack-sent-date').should('contain', '11 January 2023')
        })

        it('TC02: Validation should work on DAO page when user enter future date', () => {
            //future date
            cy.submitDAODate(11, 1, 2025)
            cy.get('#save-and-continue-button').click()
            cy.get('#dao-pack-sent-date-error-link').should('contain.text', 'DAO pack sent date must be in the past')
            //incorrect day
            cy.submitDAODate(111, 1, 2023)
            cy.get('#save-and-continue-button').click()
            cy.get('#dao-pack-sent-date-error-link').should('contain.text', 'Day must be between 1 and 31')
            //incorrect month
            cy.submitDAODate(11, 13, 2025)
            cy.get('#save-and-continue-button').click()
            cy.get('#dao-pack-sent-date-error-link').should('contain.text', 'Month must be between 1 and 12')
        });
    });
});