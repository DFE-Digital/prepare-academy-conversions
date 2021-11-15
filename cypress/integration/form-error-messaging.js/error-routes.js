before(function () {
	cy.login();
});

describe("Error message link should redirect correctly", () => {
	afterEach(() => {
		cy.storeSessionData();
	});

	it("Should open first school in the list", () => {
		cy.get("#school-name-0").click();
		cy.get('*[href*="/confirm-school-trust-information-project-dates"]').should(
			"be.visible"
		);
	});

	it("Should click on error link and allow user to re-enter date", () => {
		cy.get(
			'*[href*="/confirm-school-trust-information-project-dates"]'
		).click();
		cy.get('*[data-test="change-head-teacher-board-date"]').click();
		cy.submitDate(11, 11, 1980);

		cy.get(".govuk-button").click();
		cy.get(".govuk-error-summary__list li a")
			.should("have.text", "Advisory Board date must be in the future")
			.click();
		cy.submitDate(1, 2, 2025);
		cy.get(".govuk-button").click();
		cy.get(".govuk-button").click();
		cy.get(".govuk-button.govuk-button--secondary").click();
	});

	it("Should display report link for school when Generate Report link clicked", () => {
		cy.get(".app-c-attachment__link").should("be.visible");
	});
});
