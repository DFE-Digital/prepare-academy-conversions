/// <reference types ='Cypress'/>
// Guide: To extend the viewport sizes check Cypress documentation.
// Step1: The Cypress._.each will run through each test within the imported smoke test file.
// Step2: To run tests unskip test.

import {smokeTest} from "./90893_SmokeTestSetUpExport"

Cypress._.each(['iphone-x', 'ipad-mini'], (viewport) => {
    it.skip(`Smoke Test Package works on ${viewport}`, () => {
        cy.viewport(viewport)
        smokeTest()
    });
});