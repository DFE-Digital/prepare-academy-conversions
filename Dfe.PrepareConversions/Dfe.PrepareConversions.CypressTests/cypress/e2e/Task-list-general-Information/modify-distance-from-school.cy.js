/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`92796 Modify Distance from School Info ${viewport}`, () => {
		beforeEach(() => {
			cy.login({titleFilter: 'Gloucester school'})
			cy.viewport(viewport)
			cy.selectSchoolListing(2)
			cy.urlPath().then(url => {
				let modifiedUrl = url + '/confirm-general-information'
				cy.visit(modifiedUrl)
			});
		})

        it('TC01: Precondition: Distance Info Summary is empty', () => {
            // Distrance Info
            cy.milesIsEmpty()
            // Additional Info box
            cy.addInfoIsEmpty()
        })

        it('TC02: Filling in the distance of school info for the first time', () => {
            cy.changeLink().click()
            .then(() => {
                cy.disMiles().click().type('10')
            })
            cy.get('[id="distance-to-trust-headquarters-additional-information"]').click().type('Testing')
            .then(() => {
                cy.saveContinue().click()
                })
            // Info saved on summary page
            cy.disMiles().should('contain.text', '10 miles')
            cy.get('[id="distance-to-trust-headquarters-additional-text"]').should('contain.text', 'Testing')
        })

        it('TC03: Error Message for distance info', () => {
            cy.changeLink().click()
            .then(() => {
                cy.disMiles().click().type('a')
                .then(() => {
                    cy.saveContinue().click()
                })
            cy.get('[id="error-summary-title"]').should('contain.text', 'There is a problem')
            cy.get('[id="distance-to-trust-headquarters-error-link"]').should('contain.text', "'Distance from the converting school to the trust or other schools in the trust' must be a valid format")
            })
        })
    })
})
