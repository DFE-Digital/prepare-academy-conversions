/// <reference types ='Cypress'/>

import BasePage from "./BasePage";

export default class Budget extends BasePage {
    static selectors = {
        changeEndOfFinancialYearLink: '[data-test="change-financial-year"]',
        currentFinancialYearDayInput: '[id="financial-year-day"]',
        currentFinancialYearMonthInput: '[id="financial-year-month"]',
        currentFinancialYearYearInput: '[id="financial-year-year"]',
        currentFinancialYearRevenue: '[id="finance-year-current"]',
        currentFinancialYearCapital: '[id="finance-current-capital"]',
        nextFinancialYearDayInput: '[id="next-financial-year-day"]',
        nextFinancialYearMonthInput: '[id="next-financial-year-month"]',
        nextFinancialYearYearInput: '[id="next-financial-year-year"]',
        nextFinancialYearRevenue: '[id="finance-year-following"]',
        nextFinancialYearCapital: '[id="finance-projected-capital"]',
        saveButton: '[class="govuk-button"]'
    };

    static path = 'budget';

    static updateBudgetInfomation({
        endOfFinanicalYear = new Date(),
        forecastedRevenueCurrentYear = 10,
        forecastedCapitalCurrentYear = 5,
        endOfNextFinancialYear = new Date().setFullYear(new Date().getFullYear() + 1),
        forecastedRevenueNextYear = 10,
        forecastedCapitalNextYear = 5
    }) {
        cy.checkPath(this.path);

        cy.get(this.selectors.changeEndOfFinancialYearLink).click()

        cy.get(this.selectors.currentFinancialYearDayInput).clear().type(endOfFinanicalYear.getDate());
        cy.get(this.selectors.currentFinancialYearMonthInput).clear().type(endOfFinanicalYear.getMonth() + 1); // Add 1 as months start at 0
        cy.get(this.selectors.currentFinancialYearYearInput).clear().type(endOfFinanicalYear.getFullYear());
        cy.get(this.selectors.currentFinancialYearRevenue).clear().type(forecastedRevenueCurrentYear);
        cy.get(this.selectors.currentFinancialYearCapital).clear().type(forecastedCapitalCurrentYear);

        cy.get(this.selectors.nextFinancialYearDayInput).clear().type(endOfNextFinancialYear.getDate());
        cy.get(this.selectors.nextFinancialYearMonthInput).clear().type(endOfNextFinancialYear.getMonth() + 1); // Add 1 as months start at 0
        cy.get(this.selectors.nextFinancialYearYearInput).clear().type(endOfNextFinancialYear.getFullYear());
        cy.get(this.selectors.nextFinancialYearRevenue).clear().type(forecastedRevenueNextYear);
        cy.get(this.selectors.nextFinancialYearCapital).clear().type(forecastedCapitalNextYear);

        cy.get(this.selectors.saveButton).click();
    };

    static unassignProject() {
        cy.checkPath(this.path)
        cy.get(this.selectors.unassignLink).click()
    };

};
