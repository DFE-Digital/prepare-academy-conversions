/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangePlanningPermission extends BasePage {
    public path = 'planning-permission';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Planning Permission');
        cy.contains('legend', 'Has planning permission been secured?').should('be.visible');
        return this;
    }

    public verifyAllOptionsPresent(): this {
        cy.getByDataTest('planning-permission-yes').should('exist');
        cy.getByDataTest('planning-permission-no').should('exist');
        cy.getByDataTest('planning-permission-not-applicable').should('exist');
        cy.contains('label', 'Yes').should('be.visible');
        cy.contains('label', 'No').should('be.visible');
        cy.contains('label', 'Not applicable').should('be.visible');
        return this;
    }

    public selectYes(): this {
        cy.getByDataTest('planning-permission-yes').check();
        return this;
    }

    public selectNo(): this {
        cy.getByDataTest('planning-permission-no').check();
        return this;
    }

    public selectNotApplicable(): this {
        cy.getByDataTest('planning-permission-not-applicable').check();
        return this;
    }

    public verifyAdditionalInformationHidden(): this {
        cy.getByDataTest('planning-permission-additional-information').should('not.be.visible');
        return this;
    }

    public verifyAdditionalInformationVisible(): this {
        cy.getByDataTest('planning-permission-additional-information').should('be.visible');
        return this;
    }

    public enterAdditionalInformation(text: string): this {
        cy.getByDataTest('planning-permission-additional-information').clear().type(text);
        return this;
    }

    public enterSupportingEvidence(text: string): this {
        cy.getByDataTest('planning-permission-supporting-evidence').clear().type(text);
        return this;
    }

    public save(): this {
        cy.getById('save-and-continue-button').click();
        return this;
    }
}

const significantChangePlanningPermission = new SignificantChangePlanningPermission();

export default significantChangePlanningPermission;
