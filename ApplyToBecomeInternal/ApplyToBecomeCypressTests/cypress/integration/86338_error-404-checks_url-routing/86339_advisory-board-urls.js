/// <reference types ="Cypress"/>

describe('86339 validate advisory board urls', () => {
    let selectedSchool = '';

    afterEach(() => {
		cy.storeSessionData();
	});

    beforeEach(function () {
        cy.login();
        cy.selectSchoolListing(2)
        cy.url().then(url =>{
            selectedSchool = url
        })
     });
    
   after(function () {
        cy.clearLocalStorage();
    });  

    it('TC01: should correctly render a page when user visits /confirm-school-trust-information-project-dates/advisory-board-date', () => {
            //Changes the current URL
            cy.visit(selectedSchool + "/confirm-school-trust-information-project-dates/advisory-board-date"            )
            cy.get('h1').contains('Set the Advisory Board date')   
    });

    it('TC02: should correctly render a page when user visits /confirm-school-trust-information-project-dates/previous-advisory-board-date', () => {
            //Changes the current URL
            cy.visit(selectedSchool+"/confirm-school-trust-information-project-dates/previous-advisory-board-date")
            cy.get('h1').contains('Date of previous Advisory Board')
    });

    it('TC03: should correctly render a page when user visits /confirm-school-trust-information-project-dates/previous-advisory-board', () => {
        //Changes the current URL
        cy.visit(selectedSchool+"/confirm-school-trust-information-project-dates/previous-advisory-board")
        cy.get('h1').contains('Has this project been to a previous Advisory Board?')
    });

    it('TC04: should correctly render a page when user visits /preview-project-template', () => {
        //Changes the current URL
        cy.visit(selectedSchool+"/preview-project-template")
        cy.get('h1').contains('Preview project template')
    });

    it.skip('TC05: should correctly render a page when user visits /download-project-template', () => {
        //Changes the current URL
        cy.visit(selectedSchool+"/download-project-template")
        cy.get('h1').contains('Download project template')
    });

});