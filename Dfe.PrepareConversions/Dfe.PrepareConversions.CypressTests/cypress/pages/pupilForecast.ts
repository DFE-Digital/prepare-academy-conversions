/// <reference types="cypress" />
import BasePage from './basePage';

class PupilForecast extends BasePage {
    public path = 'pupil-forecasts';

    private selectors = {
        changeAdditionalInfoLink: '[data-test="change-school-pupil-forecasts-additional-information"]',
        additionalInfo: '[id="additional-information"]',
        saveButton: '[data-cy="select-common-submitbutton"]',
    };

    public enterAditionalInfomation(additionalInformation = 'Pupil Forecast additional infomation'): this {
        cy.checkPath(this.path);
        cy.get(this.selectors.changeAdditionalInfoLink).click();
        cy.get(this.selectors.additionalInfo).clear();
        cy.get(this.selectors.additionalInfo).type(additionalInformation);
        cy.get(this.selectors.saveButton).click();
        return this;
    }

    public getAdditionalInfomation(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.get(this.selectors.additionalInfo);
    }
}

const pupilForecast = new PupilForecast();

export default pupilForecast;
