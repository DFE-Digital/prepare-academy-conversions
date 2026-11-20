/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeTrustExperienceAndGovernance extends BasePage {
    public path = 'trust-experience-and-governance';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Trust experience and governance');
        cy.contains(
            'legend',
            'Does the trust currently have the experience and governance arrangements in place to support the proposed change?'
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

const significantChangeTrustExperienceAndGovernance = new SignificantChangeTrustExperienceAndGovernance();

export default significantChangeTrustExperienceAndGovernance;
