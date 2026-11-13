/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeSatelliteSiteArrangement extends BasePage {
    public verifyPageLoaded(): this {
        cy.checkPath('satellite-site-arrangement');
        this.verifyHeading('Satellite site arrangement');
        cy.contains(
            'legend',
            'Is this a genuine expansion of the special school that does not constitute a new school?'
        ).should('be.visible');
        return this;
    }

    public selectAnswer(answer: 'yes' | 'no' | 'not-applicable'): this {
        cy.getById(`answer-${answer}`).check();
        return this;
    }

    public verifyAdditionalInformationVisible(visible: boolean): this {
        cy.getByDataTest('additional-information').should(visible ? 'be.visible' : 'not.be.visible');
        return this;
    }

    public enterAdditionalInformation(value: string): this {
        cy.getByDataTest('additional-information').clear().type(value);
        return this;
    }

    public enterSupportingEvidence(value: string): this {
        cy.getByDataTest('supporting-evidence').clear().type(value);
        return this;
    }

    public save(): this {
        cy.getById('save-and-continue-button').click();
        return this;
    }
}

export default new SignificantChangeSatelliteSiteArrangement();
