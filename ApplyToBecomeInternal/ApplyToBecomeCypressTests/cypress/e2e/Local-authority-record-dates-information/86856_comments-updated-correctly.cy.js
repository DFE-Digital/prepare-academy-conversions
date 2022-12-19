/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`86856 Comments should accept alphanumeric inputs on ${viewport}`, () => {
		beforeEach(() => {
			cy.login()

			cy.viewport(viewport)
			cy.selectSchoolListing(2)
			cy.url().then(url => {
				//changes the current URL
				let modifiedUrl = url + '/confirm-local-authority-information-template-dates'
				cy.visit(modifiedUrl)
			});
		})

        it('TC: Precondition comment box', () => {
            cy.get('[id="la-info-template-comments"]').should('be.visible')
            .invoke('text')
            .then((text) => {
                if (text.includes('Empty')) {
                    return
                }
                else {
                    cy.commentBoxClearLaInfo()
                };
            });
        });

        it('TC01: Navigates to comment section & type alphanumerical characters', () => {
            let alphanumeric = 'abcdefghijklmnopqrstuvwxyz 1234567890 !"£$%^&*(){}[]:@,./<>?~|'
            cy.get('[data-test="change-la-info-template-comments"]').click()
            cy.commentBoxLaInfo().type(alphanumeric)
            cy.saveAndContinueButton().click()
            cy.commentBoxLaInfo().should('contain', alphanumeric)
        });

        it('TC02: Clears text input', () => {
            cy.commentBoxClearLaInfo()
            cy.commentBoxLaInfo().should('contain', 'Empty')
        });
    });
});