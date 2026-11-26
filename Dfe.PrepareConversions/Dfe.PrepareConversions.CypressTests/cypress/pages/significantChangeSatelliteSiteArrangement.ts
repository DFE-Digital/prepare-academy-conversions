/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeSatelliteSiteArrangement extends BasePage {
    public path = 'satellite-site-arrangement';

    public verifyPageLoaded(): this {
        return this.verifyPageLoadedWithLegend(
            'Satellite site arrangement',
            'Is this a genuine expansion of the special school that does not constitute a new school?'
        );
    }

    public selectAnswer(answer: 'yes' | 'no' | 'not-applicable'): this {
        cy.getById(`answer-${answer}`).check();
        return this;
    }

    public verifyAdditionalInformationVisible(visible: boolean): this {
        cy.getByDataTest('additional-information').should(visible ? 'be.visible' : 'not.be.visible');
        return this;
    }

    public save(): this {
        cy.getById('save-and-continue-button').click();
        return this;
    }
}

export default new SignificantChangeSatelliteSiteArrangement();
