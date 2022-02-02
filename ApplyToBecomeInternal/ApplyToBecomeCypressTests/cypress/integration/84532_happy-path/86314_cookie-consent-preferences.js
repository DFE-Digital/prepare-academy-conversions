/// <reference types ="Cypress"/>

describe('Cookie consent details', () => {
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
    
    it('should show top cookie banner when no consent option set', () => {
        cy.get("[data-test='cookie-banner']").should("be.visible");
    });

    it('should consent to cookies from cookie header button', () => {
        cy.get('[data-test="cookie-banner-accept"]').click();
        let consentCookie = cy.getCookie('.ManageAnAcademyConversion.Consent');
        consentCookie.should('exist');
        consentCookie.should('have.property','value','True');
    })

    it('should hide the cookie banner when consent has been given', () => {
        cy.get("[data-test='cookie-banner']").should('not.exist');
    })

    it('should navigate to cookies page from footer link', () => {
        cy.get("[data-test='cookie-preferences']").click();
        cy.url().then(href => {
            expect(href).includes('cookie-preferences');
        });
    });

    it('should set cookie preferences', () => {
        cy.get('#cookie-consent-deny').click();
        cy.get("[data-qa='submit']").click();
        cy.getCookie('.ManageAnAcademyConversion.Consent').should('have.property','value','False');
    });

    it('should return show success banner', () => {
        cy.get("[data-test='success-banner-return-link']").click();
        cy.url().then(href => {
            expect(href).includes('/confirm-general-information')
        });
        cy.clearCookies();
    });
})
