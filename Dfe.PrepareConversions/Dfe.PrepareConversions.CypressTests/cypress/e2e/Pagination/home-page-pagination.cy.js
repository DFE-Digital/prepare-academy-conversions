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
                cy.log(text)
                const total_count = text.replace('projects found', '').trim()
                if (total_count > 10) {
                    cy.get('[test-id="nextPage"]').click()
                    cy.url().should('contain', 'currentPage=2');
                    cy.get('[test-id="previousPage"]').click()
                    cy.url().should('contain', 'currentPage=1');
                }
                else {
                    cy.log('Number of projects does not exceed 10')
                }
            })
    })
})