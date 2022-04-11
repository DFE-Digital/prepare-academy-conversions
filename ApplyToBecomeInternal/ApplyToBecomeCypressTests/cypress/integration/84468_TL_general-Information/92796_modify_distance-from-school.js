/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
    describe(`92796 Modify Distance from School Info ${viewport}`, () => {
        afterEach(() => {
            cy.storeSessionData()
        });
    
        before(() => {
            cy.viewport(viewport)
            cy.login()
            cy.selectSchoolListing(2)
            cy.url().then(url => {
                let modifiedUrl = url + '/confirm-general-information'
                cy.visit(modifiedUrl)
            });
        });
    
        after(() => {
            cy.clearLocalStorage()
        });

        it('Precondition: Distance Info Summary is empty', () => {
            cy.viewport(viewport)
            // Distrance Info 
            cy.milesIsEmpty()
            // Additional Info box
            cy.addInfoIsEmpty()
        })

        it('TC01: Filling in the distance of school info for the first time', () => {
            cy.viewport(viewport)
            cy.changeLink().click()
            .then(() => {
                cy.disMiles().click().type('10')
            })
            cy.get('[id="distance-to-trust-headquarters-additional-information"').click().type('Testing')
            .then(() => {
                cy.saveContinue().click()
                })
            // Info saved on summary page
            cy.disMiles().should('contain.text', '10 miles')
            cy.get('[id="distance-to-trust-headquarters-additional-text"]').should('contain.text', 'Testing')
        })

        // raised under 92838
        it.skip('TC02: Error Message', () => {
            cy.viewport(viewport)
            cy.changeLink().click()
            .then(() => {
                cy.disMiles().click().type('a')
                .then(() => {
                    cy.saveContinue().click()
                })
            cy.get('[id="error-summary-title"]').should('contain.text', 'There is a problem')
            })
        })
    })
})