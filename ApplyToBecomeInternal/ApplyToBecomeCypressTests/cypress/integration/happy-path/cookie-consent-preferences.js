describe('Submit and view MP details', () => {
    afterEach(() => {
		cy.storeSessionData();
	});

    before(function () {
        cy.login();
        cy.selectSchoolListing(2)
        cy.url().then(url =>{
            //Changes the current URL
            let modifiedUrl = url + "/confirm-general-information"
            cy.visit(modifiedUrl)
        });
     });
    
    after(function () {
        cy.clearLocalStorage();
    });

    it('should navigate to cookies page from footer link', () => {
        cy.get("[data-test='cookie-preferences']").click();
        cy.url().then(href => {
            expect(href).includes('cookie-preferences');
        });
    });

    it('should set cookie preferences', () => {
        cy.get('#cookie-consent-deny').click();
        cy.get("[data-qa='submit']").click();
    });

    it('should return show success banner', () => {
        cy.get("[data-test='success-banner-return-link']").click();
        cy.url().then(href => {
            expect(href).includes('/confirm-general-information')
        });
    });
})
