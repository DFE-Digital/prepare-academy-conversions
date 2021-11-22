describe("Error messaging should be correct", () => {
	afterEach(() => {
		cy.storeSessionData();
	});

	before(function () {
		cy.login();
	});
	
	after(function () {
		cy.clearLocalStorage();
	});
	

	it("Should open first school in the list", () => {
		cy.get("#school-name-0").click();
		cy.get('*[href*="/confirm-school-trust-information-project-dates"]').should(
			"be.visible"
		);
		cy.saveLocalStorage();
	});

	it("Should display 'Advisory Board date must be in the future' when an elapsed date has been submitted", () => {
		cy.get(
			'*[href*="/confirm-school-trust-information-project-dates"]'
		).click();
		cy.get('*[data-test="change-head-teacher-board-date"]').click();
		cy.submitDate(11, 11, 1980);
		cy.get(".govuk-button").click();
		cy.get(".govuk-error-summary__list li a").should(
			"have.text",
			"Advisory Board date must be in the future"
		);
	});

	it("Should display 'Advisory Board date must be a real date' when submitting invalid month ", () => {
		cy.submitDate(11, 222, 1980);
		cy.get(".govuk-button").click();
		cy.get(".govuk-error-summary__list li a").should(
			"have.text",
			"Advisory Board date must be a real date"
		);
	});

	it("Should display 'Advisory Board date must be a real date' when submitting out-of-index month ", () => {
		cy.submitDate(11, 0, 1980);
		cy.get(".govuk-button").click();
		cy.get(".govuk-error-summary__list li a").should(
			"have.text",
			"Advisory Board date must be a real date"
		);
	});
});
