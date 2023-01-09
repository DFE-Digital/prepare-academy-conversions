/// <reference types ='Cypress'/>

describe('101092: Pagination', () => {

	beforeEach(() => {
		cy.login()
	})

    after(function () {
        cy.clearLocalStorage()
    })

    it('TC01: Next/Previous button if project number is greater than 10', () => {
        cy.get('[test-id="projectCount"]')
        .invoke('text')
        .then((text) => {
            if (text.includes('Projects (10)')) {
                cy.get('[test-id="nextPage"]').click()
                cy.get('[test-id="previousPage"]').click()
            }
            else {
                cy.log('Number of projects does not exceed 10')
            }
        })
    })
})