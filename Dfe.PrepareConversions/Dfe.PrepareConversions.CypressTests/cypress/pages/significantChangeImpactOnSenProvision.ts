/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeImpactOnSenProvision extends BasePage {
    public path = 'impact-on-sen-provision';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Impact on SEN provision');
        cy.contains(
            'legend',
            'Is the proposal likely to improve the standard, quality and range of SEN provision?'
        ).should('be.visible');
        return this;
    }

    public verifySupportingEvidencePresent(): this {
        cy.getByDataTest('supporting-evidence').should('exist');
        return this;
    }

    public verifyAdditionalInformationVisible(visible: boolean): this {
        cy.getByDataTest('additional-information').should(visible ? 'be.visible' : 'not.be.visible');
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

const significantChangeImpactOnSenProvision = new SignificantChangeImpactOnSenProvision();

export default significantChangeImpactOnSenProvision;
