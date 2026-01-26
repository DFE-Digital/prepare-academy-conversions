/// <reference types="cypress" />

import BasePage from './basePage';

const currentYear = new Date();
const nextYear = new Date();
nextYear.setFullYear(new Date().getFullYear() + 1);

interface BudgetInfo {
  endOfFinanicalYear?: Date;
  forecastedRevenueCurrentYear: number;
  forecastedCapitalCurrentYear: number;
  endOfNextFinancialYear?: Date;
  forecastedRevenueNextYear: number;
  forecastedCapitalNextYear: number;
}

export default class Budget extends BasePage {
  static selectors = {
    changeEndOfFinancialYearLink: '[data-test="change-financial-year"]',
    currentFinancialYearDayInput: '[id="financial-year-day"]',
    currentFinancialYearMonthInput: '[id="financial-year-month"]',
    currentFinancialYearYearInput: '[id="financial-year-year"]',
    currentFinancialYearValue: '[id="financial-year"]',
    currentFinancialYearRevenueInput: '[id="finance-year-current"]',
    currentFinancialYearRevenueValue: '[id="finance-year-current"]',
    currentFinancialYearCapitalInput: '[id="finance-current-capital"]',
    currentFinancialYearCapitalValue: '[id="finance-current-capital"]',
    nextFinancialYearDayInput: '[id="next-financial-year-day"]',
    nextFinancialYearMonthInput: '[id="next-financial-year-month"]',
    nextFinancialYearYearInput: '[id="next-financial-year-year"]',
    nextFinancialYearValue: '[id="next-financial-year"]',
    nextFinancialYearRevenueInput: '[id="finance-year-following"]',
    nextFinancialYearRevenueValue: '[id="finance-year-following"]',
    nextFinancialYearCapitalInput: '[id="finance-projected-capital"]',
    nextFinancialYearCapitalValue: '[id="finance-projected-capital"]',
    saveButton: '[data-cy="select-common-submitbutton"]',
    completeCheckbox: '[id="school-budget-information-complete"]',
  };

  static path = 'budget';

  static updateBudgetInfomation({
    endOfFinanicalYear,
    forecastedRevenueCurrentYear,
    forecastedCapitalCurrentYear,
    endOfNextFinancialYear,
    forecastedRevenueNextYear,
    forecastedCapitalNextYear,
  }: BudgetInfo): void {
    let financialYear = endOfFinanicalYear;
    let nextFinancialYear = endOfNextFinancialYear;

    if (!financialYear) {
      financialYear = currentYear;
    }

    if (!nextFinancialYear) {
      nextFinancialYear = nextYear;
    }
    cy.checkPath(this.path);

    cy.get(this.selectors.changeEndOfFinancialYearLink).click();

    cy.get(this.selectors.currentFinancialYearDayInput).clear();
    cy.get(this.selectors.currentFinancialYearDayInput).type(
      String(financialYear.getDate())
    );
    cy.get(this.selectors.currentFinancialYearMonthInput).clear();
    cy.get(this.selectors.currentFinancialYearMonthInput).type(
      String(financialYear.getMonth() + 1)
    ); // Add 1 as months start at 0
    cy.get(this.selectors.currentFinancialYearYearInput).clear();
    cy.get(this.selectors.currentFinancialYearYearInput).type(
      String(financialYear.getFullYear())
    );
    cy.get(this.selectors.currentFinancialYearRevenueInput).clear();
    cy.get(this.selectors.currentFinancialYearRevenueInput).type(
      String(forecastedRevenueCurrentYear)
    );
    cy.get(this.selectors.currentFinancialYearCapitalInput).clear();
    cy.get(this.selectors.currentFinancialYearCapitalInput).type(
      String(forecastedCapitalCurrentYear)
    );

    cy.get(this.selectors.nextFinancialYearDayInput).clear();
    cy.get(this.selectors.nextFinancialYearDayInput).type(
      String(nextFinancialYear.getDate())
    );
    cy.get(this.selectors.nextFinancialYearMonthInput).clear();
    cy.get(this.selectors.nextFinancialYearMonthInput).type(
      String(nextFinancialYear.getMonth() + 1)
    ); // Add 1 as months start at 0
    cy.get(this.selectors.nextFinancialYearYearInput).clear();
    cy.get(this.selectors.nextFinancialYearYearInput).type(
      String(nextFinancialYear.getFullYear())
    );
    cy.get(this.selectors.nextFinancialYearRevenueInput).clear();
    cy.get(this.selectors.nextFinancialYearRevenueInput).type(
      String(forecastedRevenueNextYear)
    );
    cy.get(this.selectors.nextFinancialYearCapitalInput).clear();
    cy.get(this.selectors.nextFinancialYearCapitalInput).type(
      String(forecastedCapitalNextYear)
    );

    cy.get(this.selectors.saveButton).click();
  }

  static getCurrentFinancialYear(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.currentFinancialYearValue);
  }

  static getCurrentRevenue(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.currentFinancialYearRevenueValue);
  }

  static getCurrentCapital(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.currentFinancialYearCapitalValue);
  }

  static getNextFinancialYear(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.nextFinancialYearValue);
  }

  static getNextRevenue(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.nextFinancialYearRevenueValue);
  }

  static getNextCapital(): Cypress.Chainable<JQuery<HTMLElement>> {
    cy.checkPath(this.path);
    return cy.get(this.selectors.nextFinancialYearCapitalValue);
  }

  static markComplete(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.completeCheckbox).check();
  }

  static markIncomplete(): void {
    cy.checkPath(this.path);
    cy.get(this.selectors.completeCheckbox).uncheck();
  }
}
