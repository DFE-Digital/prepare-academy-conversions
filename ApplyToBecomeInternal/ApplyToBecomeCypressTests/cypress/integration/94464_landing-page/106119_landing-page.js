/// <reference types ='Cypress'/>

const url = Cypress.env('url') + '/project-type'

describe('Landing Page',() => {
    beforeEach(() => {
        cy.visit(url)

    })

    afterEach(() => {
        cy.storeSessionData()
    })

    after(function () {
        cy.clearLocalStorage()
    })

    it('TC01: Navigates to Conversion site from landing page, and clicks back to landing page', () => {
        cy.get('[id="conversion-radio"]').click()
        cy.continueBtn().click()
        cy.get('[class="govuk-heading-xl"]').should('contain.text', 'Manage an academy conversion')
        cy.get('[class="govuk-back-link"]').click()
        cy.get('[class="govuk-heading-l"]').should('contain.text', 'What do you want to do?')
    })

    it('TC02: Checks error message on unselected project' ,() => {
        cy.continueBtn().click()
        cy.get('[id="ProjectType-error-link "]').should('contain.text', 'Select a project type')
    })
})