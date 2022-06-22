/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
    describe(`86462: "Date you sent/return the template" are reflected in preview on ${viewport}`, () => {
		beforeEach(() => {
			cy.login()
			cy.viewport(viewport)
			cy.selectSchoolListing(2)
			cy.url().then(url => {
				//changes the current URL
				let modifiedUrl = url + '/confirm-local-authority-information-template-dates'
				cy.visit(modifiedUrl)
			});

			cy.get('[data-test="change-la-info-template-returned-date"]').click()
			cy.submitDateLaInfoReturn(20, 2, 2020)
			cy.submitDateLaInfoSent(20, 2, 2020)
			cy.saveContinueBtn().click()
		})
    
        it('TC01: Verifies "Date you sent the template" on LA information preview', () => {
            cy.sentDateSummLaInfo()
            .invoke('text')
            .should('contain', '20 February 2022' )
            .then((text) => {
                expect(text).to.match(/^([0-9]){2}\s[a-zA-Z]{1,}\s[0-9]{4}$/)
            });
        });
    
        it('TC02: Verifies "Date you want the template returned" on LA information preview', () => {
            cy.returnDateSummLaInfo()
            .invoke('text')
            .should('contain', '20 February 2020')
            .then((text) => {
                expect(text).to.match(/^([0-9]){2}\s[a-zA-Z]{1,}\s[0-9]{4}$/)
            });
        });
    });
});