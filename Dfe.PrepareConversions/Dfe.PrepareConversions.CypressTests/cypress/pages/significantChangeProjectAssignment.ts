/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeProjectAssignment extends BasePage {
    public path = 'significant-change/project-assignment';

    private readonly selectors = {
        schoolName: 'school-name',
        deliveryOfficerSelect: '#delivery-officer',
        deliveryOfficerInput: '#delivery-officer-input',
        unassignLink: 'unassign-link',
    };

    public verifyPageIsVisible(): this {
        cy.checkPath(this.path);
        cy.getByDataId(this.selectors.schoolName).should('be.visible');
        cy.get('h1').should('contain.text', 'Who will be on this project?');
        cy.get(this.selectors.deliveryOfficerSelect).should('exist');
        return this;
    }

    public assignFirstAvailableDeliveryOfficer(): Cypress.Chainable<string> {
        cy.checkPath(this.path);

        return cy.get(`${this.selectors.deliveryOfficerSelect} option`).then(($options) => {
            const values = [...$options]
                .map((option) => option.getAttribute('value')?.trim() ?? '')
                .filter((value) => value.length > 0);

            expect(values.length, 'delivery officer options').to.be.greaterThan(0);

            const selectedOfficer = values[0];

            cy.get(this.selectors.deliveryOfficerSelect).select(selectedOfficer, { force: true });
            cy.get(this.selectors.deliveryOfficerInput).should('have.value', selectedOfficer);
            cy.clickContinueBtn();

            return selectedOfficer;
        });
    }

    public unassignDeliveryOfficer(): this {
        cy.checkPath(this.path);
        cy.getById(this.selectors.unassignLink).click();
        return this;
    }
}

const significantChangeProjectAssignment = new SignificantChangeProjectAssignment();

export default significantChangeProjectAssignment;
