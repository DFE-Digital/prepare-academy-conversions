/// <reference types ='Cypress'/>

import BasePage from "./BasePage";

export default class PupilForecast extends BasePage {
    static selectors = {
        changeAdditionalInfoLink: '[data-test="change-school-pupil-forecasts-additional-information"]',
        additionalInfo: '[id="additional-information"]',
        saveButton: '[class="govuk-button"]'
    };

    static path = 'pupil-forecasts';

    static enterAditionalInfomation(additionalInformation = 'Pupil Forecast additional infomation') {
        cy.checkPath(this.path);
        cy.get(this.selectors.changeAdditionalInfoLink).click();
        cy.get(this.selectors.id = "additional-information").clear().type(`${deliveryOfficer}{enter}`);
        cy.get(this.selectors.saveButton).click();
    };

    static getAdditionalInfomation() {
        cy.checkPath(this.path);
        return cy.get(this.selectors.additionalInfo);
    };

};
