/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeMainstreamSchoolSatelliteConsiderations extends BasePage {
    public path = 'mainstream-school-satellite-considerations';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Mainstream school satellite considerations');
        cy.contains(
            'legend',
            'Does the proposal meet the requirements set out for special school provision on a satellite site in the operational guidance?'
        ).should('be.visible');
        return this;
    }

    public verifySupportingEvidencePresent(): this {
        cy.getByDataTest('supporting-evidence').should('exist');
        return this;
    }

    public enterAdditionalInformation(value: string): this {
        cy.getByDataTest('additional-information').clear();
        cy.getByDataTest('additional-information').type(value);
        return this;
    }

    public enterSupportingEvidence(value: string): this {
        cy.getByDataTest('supporting-evidence').clear();
        cy.getByDataTest('supporting-evidence').type(value);
        return this;
    }
}

const significantChangeMainstreamSchoolSatelliteConsiderations =
    new SignificantChangeMainstreamSchoolSatelliteConsiderations();

export default significantChangeMainstreamSchoolSatelliteConsiderations;
