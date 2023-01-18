/// <reference types ='Cypress'/>
import conversionLinks from '../../fixtures/conversionLinks.json'


const wcagStandards = ["wcag22aa"];
const impactLevel = ["critical", "minor", "moderate", "serious"];
const continueOnFail = false;

Cypress._.each(['ipad-mini'], (viewport) => {
    describe('Check accessibility of the different pages', function () {
        conversionLinks.forEach((link) => {

            it('Validate accessibility on different pages', function () {
                let url = Cypress.env('url')
                cy.visit(url)
                cy.visit(url + link)
                cy.excuteAccessibilityTests(wcagStandards, continueOnFail, impactLevel)
            })
        })
    })
})
