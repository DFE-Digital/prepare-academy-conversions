before(function () {
	cy.login();
});

after(function () {
	cy.clearLocalStorage();
});

describe("Error handling should present correctly to the user", () => {
	afterEach(() => {
		cy.storeSessionData();
	});

    it("Should open first school in the list", () => {
		cy.get("#school-name-0").click();
		cy.get('*[href*="/confirm-school-trust-information-project-dates"]').should(
			"be.visible"
		);
	});

    it('Should display user-friendly error',()=>{
        let modifiedUrl
        cy.get('[aria-describedby*=la-info-template-status]').click()
        cy.url().then(url =>{
            //Changes the current URL to:
            //https://apply-to-become-an-academy-internal-dev.london.cloudapps.digital/task-list/<SOME_ID>/confirm-local-authority-information-template-dates?return=someInvalideParam/SomeInvalidPath
            modifiedUrl = url.replace('%2FTaskList%2FIndex&backText=Back%20to%20task%20list','someInvalideParam')
            cy.visit(modifiedUrl+'/SomeInvalidPath')
        })
       cy.get('.govuk-button').click()
       cy.get('h1').should('not.contain.text','An unhandled exception occurred while processing the request.')
    })
});
