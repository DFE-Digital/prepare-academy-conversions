/// <reference types ='Cypress'/>
// Guide: The exported smoke test will run within the screeSizeImport.js File.
// Step1: To use this effectively include your test commands into this file without the 'Before' & 'After' Hooks.
// Step2: Ensure to include notes for each test so that changes can be updated easily as needed for each smoke run. Example is provided below.

export const smokeTest = () => {
    // -- start
    // 84468_TL_general-Information
    // Describe('86858 Modify viability fields)
    // Before hook
    cy.login()
    cy.selectSchoolListing(2)
    cy.url().then(url =>{
        //Changes the current URL
        let modifiedUrl = url + '/confirm-general-information'
        cy.visit(modifiedUrl)
    });
    cy.storeSessionData();
    cy.clearLocalStorage();

    // it('TC01: Navigates to Viability fields and modifies fields "Yes"')
    cy.get('[data-test="change-viability-issues"]').click()
    cy.get('[id="viability-issues"]').click()
    cy.saveAndContinueButton().click()
    cy.get('[id="viability-issues"]').should('contain', 'Yes')
    .should('not.contain', 'No')
    .should('not.contain', 'Empty');

    // it('TC02: Navigates to Viability fields and modifies fields "No"')
    cy.get('[data-test="change-viability-issues"]').click()
    cy.get('[id="viability-issues-2"]').click()
    cy.saveAndContinueButton().click()
    cy.get('[id="viability-issues"]').should('contain', 'No')
    .should('not.contain', 'Yes')
    .should('not.contain', 'Empty');
    // --end

}