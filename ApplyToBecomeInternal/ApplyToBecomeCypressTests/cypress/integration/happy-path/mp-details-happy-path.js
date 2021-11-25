describe('Submit and view MP details', () => {
    afterEach(() => {
		cy.storeSessionData();
	});

    before(function () {
        cy.login();
        cy.selectSchoolListing(2)
    });
    
    after(function () {
        cy.clearLocalStorage();
    });

    it('Should display MP details in general information page', () => {
        cy.url().then(url =>{
            //Changes the current URL
            let modifiedUrl = url + "/confirm-general-information"
            cy.visit(modifiedUrl)
        });
        cy.debug();        
        cy.get('.govuk-link').contains('mp-details').click();
    });

    it('Should navigate to MP details page and change details', () => {
        
    });

    it('Should display the MP details after it is submitted', () => {
        cy.get('#mp-name').should('have.text', 'THE_MP_NAME')
    });
    
    it('Should navigate to MP details page and remove details', () => {
        
    });

    it('Should display the MP details are empty after it is submitted', () => {
        
    });
})
