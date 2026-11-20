/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeSpecialSchoolSatelliteConsiderations extends BasePage {
    public path = 'special-school-satellite-considerations';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Special school satellite considerations');
        cy.contains(
            'legend',
            'Does the proposal meet the requirements set out for satellite sites in the operational guidance?'
        ).should('be.visible');
        return this;
    }

    public verifySupportingEvidencePresent(): this {
        cy.getByDataTest('supporting-evidence').should('exist');
        return this;
    }

    public enterSupportingEvidence(value: string): this {
        cy.getByDataTest('supporting-evidence').clear();
        cy.getByDataTest('supporting-evidence').type(value);
        return this;
    }

    public enterAdditionalInformation(value: string): this {
        cy.getByDataTest('additional-information').clear();
        cy.getByDataTest('additional-information').type(value);
        return this;
    }
}

const significantChangeSpecialSchoolSatelliteConsiderations =
    new SignificantChangeSpecialSchoolSatelliteConsiderations();

export default significantChangeSpecialSchoolSatelliteConsiderations;
