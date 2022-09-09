/// <reference types ='Cypress'/>

Cypress._.each(['iphone-x'], (viewport) => {
	describe(`86316 Submit and view MP details ${viewport}`, () => {
		before(() => {
			cy.viewport(viewport)
		});

		beforeEach(() => {
			cy.login()
			cy.selectSchoolListing(2)
			cy.url().then(url =>{
				let modifiedUrl = url + '/confirm-general-information'
				cy.visit(modifiedUrl)
			});
			cy.viewport(viewport)
			cy.get('[data-test="change-member-of-parliament-party"]').click()
			cy.mpName().clear().type('An MP')
			cy.mpParty().clear().type('A Party')
		})

        it('TC01: Should navigate to MP details page', () => {
            cy.url().then(href => {
                expect(href.endsWith('/confirm-general-information/enter-MP-name-and-political-party')).to.be.true;
            });
        });

        it('TC02: Should change the MP details', () => {
            cy.mpName().should('have.value', 'An MP')
            cy.mpParty().should('have.value', 'A Party')
        });

		context("when form submitted", () => {
			beforeEach(() => {
				cy.saveAndContinueButton().click()
			})

			it('TC03: Should go back to general information page on confirm', () => {
				cy.url().then(href => {
					expect(href.endsWith('/confirm-general-information')).to.be.true});
			});

			it('TC04: Should display the MP details after it is submitted', () => {
				cy.mpName().should('have.text', 'An MP')
				cy.mpParty().should('have.text','A Party')
			});

			it('TC05: Should navigate to MP details page and remove details', () => {
				cy.get('[data-test="change-member-of-parliament-party"]').click()
				cy.mpName().clear()
				cy.mpParty().clear()
				cy.saveAndContinueButton().click()
				cy.mpName().should('have.text', 'Empty')
				cy.mpParty().should('have.text', 'Empty')
			});
		});
    });
});