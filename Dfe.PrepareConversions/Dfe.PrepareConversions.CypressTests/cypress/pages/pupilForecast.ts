/// <reference types="cypress" />

import BasePage from './basePage';

export default class PupilForecast extends BasePage {
  static selectors = {
    changeAdditionalInfoLink:
      '[data-test="change-school-pupil-forecasts-additional-information"]',
    additionalInfo: '[id="additional-information"]',
    saveButton: '[data-cy="select-common-submitbutton"]',
  };

  static path = 'pupil-forecasts';

  static enterAditionalInfomation(
    additionalInformation = 'Pupil Forecast additional infomation'
  ): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.changeAdditionalInfoLink).click();
    cy.get(this.selectors.additionalInfo).clear();
    cy.get(this.selectors.additionalInfo).type(additionalInformation);
    cy.get(this.selectors.saveButton).click();
  }

  static getAdditionalInfomation(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.additionalInfo);
  }
}
