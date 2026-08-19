/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeProjectAssignment extends BasePage {
    public path = 'significant-change/project-assignment';

    private readonly selectors = {
        schoolName: 'school-name',
        // accessibleAutocomplete.enhanceSelectElement() puts an <input> in the select's place
        // under the original id, and renames the select itself to '<id>-select'. The visible
        // control is therefore an input; the options stay on the renamed select.
        deliveryOfficerAutocomplete: '#delivery-officer',
        deliveryOfficerOptions: '#delivery-officer-select',
        deliveryOfficerInput: '#delivery-officer-input',
        unassignLink: 'unassign-link',
    };

    public verifyPageIsVisible(): this {
        cy.checkPath(this.path);
        cy.getByDataId(this.selectors.schoolName).should('be.visible');
        cy.get('h1').should('contain.text', 'Who will be on this project?');
        cy.get(this.selectors.deliveryOfficerAutocomplete).should('exist');
        return this;
    }

    public assignFirstAvailableDeliveryOfficer(): Cypress.Chainable<string> {
        cy.checkPath(this.path);

        return cy.get(`${this.selectors.deliveryOfficerOptions} option`).then(($options) => {
            const values = [...$options]
                .map((option) => option.getAttribute('value')?.trim() ?? '')
                .filter((value) => value.length > 0);

            expect(values.length, 'delivery officer options').to.be.greaterThan(0);

            const selectedOfficer = values[0];

            // Blur confirms the choice: accessible-autocomplete writes it back to the hidden
            // select the form posts as SelectedName, and the page's change handler fills
            // DeliveryOfficerInput, which OnPost requires to be non-empty.
            cy.get(this.selectors.deliveryOfficerAutocomplete).click();
            cy.get(this.selectors.deliveryOfficerAutocomplete).type(selectedOfficer);
            cy.get(this.selectors.deliveryOfficerAutocomplete).blur();

            cy.get(this.selectors.deliveryOfficerInput).should('have.value', selectedOfficer);
            cy.clickContinueBtn();

            // Return a chainable, not a bare string: cy commands were queued above, so a
            // synchronous return would race them.
            return cy.wrap(selectedOfficer);
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
