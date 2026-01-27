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
        // data-test values
        changeEndOfFinancialYearLink: 'change-financial-year',
        // IDs (without #)
        currentFinancialYearDayInput: 'financial-year-day',
        currentFinancialYearMonthInput: 'financial-year-month',
        currentFinancialYearYearInput: 'financial-year-year',
        currentFinancialYearValue: 'financial-year',
        currentFinancialYearRevenueInput: 'finance-year-current',
        currentFinancialYearCapitalInput: 'finance-current-capital',
        nextFinancialYearDayInput: 'next-financial-year-day',
        nextFinancialYearMonthInput: 'next-financial-year-month',
        nextFinancialYearYearInput: 'next-financial-year-year',
        nextFinancialYearValue: 'next-financial-year',
        nextFinancialYearRevenueInput: 'finance-year-following',
        nextFinancialYearCapitalInput: 'finance-projected-capital',
        completeCheckbox: 'school-budget-information-complete',
        // data-cy values
        saveButton: 'select-common-submitbutton',
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

        cy.getByDataTest(this.selectors.changeEndOfFinancialYearLink).click();

        cy.getById(this.selectors.currentFinancialYearDayInput).clear();
        cy.getById(this.selectors.currentFinancialYearDayInput).type(String(financialYear.getDate()));
        cy.getById(this.selectors.currentFinancialYearMonthInput).clear();
        cy.getById(this.selectors.currentFinancialYearMonthInput).type(String(financialYear.getMonth() + 1)); // Add 1 as months start at 0
        cy.getById(this.selectors.currentFinancialYearYearInput).clear();
        cy.getById(this.selectors.currentFinancialYearYearInput).type(String(financialYear.getFullYear()));
        cy.getById(this.selectors.currentFinancialYearRevenueInput).clear();
        cy.getById(this.selectors.currentFinancialYearRevenueInput).type(String(forecastedRevenueCurrentYear));
        cy.getById(this.selectors.currentFinancialYearCapitalInput).clear();
        cy.getById(this.selectors.currentFinancialYearCapitalInput).type(String(forecastedCapitalCurrentYear));

        cy.getById(this.selectors.nextFinancialYearDayInput).clear();
        cy.getById(this.selectors.nextFinancialYearDayInput).type(String(nextFinancialYear.getDate()));
        cy.getById(this.selectors.nextFinancialYearMonthInput).clear();
        cy.getById(this.selectors.nextFinancialYearMonthInput).type(String(nextFinancialYear.getMonth() + 1)); // Add 1 as months start at 0
        cy.getById(this.selectors.nextFinancialYearYearInput).clear();
        cy.getById(this.selectors.nextFinancialYearYearInput).type(String(nextFinancialYear.getFullYear()));
        cy.getById(this.selectors.nextFinancialYearRevenueInput).clear();
        cy.getById(this.selectors.nextFinancialYearRevenueInput).type(String(forecastedRevenueNextYear));
        cy.getById(this.selectors.nextFinancialYearCapitalInput).clear();
        cy.getById(this.selectors.nextFinancialYearCapitalInput).type(String(forecastedCapitalNextYear));

        cy.getByDataCy(this.selectors.saveButton).click();
    }

    static getCurrentFinancialYear(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.currentFinancialYearValue);
    }

    static getCurrentRevenue(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.currentFinancialYearRevenueInput);
    }

    static getCurrentCapital(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.currentFinancialYearCapitalInput);
    }

    static getNextFinancialYear(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.nextFinancialYearValue);
    }

    static getNextRevenue(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.nextFinancialYearRevenueInput);
    }

    static getNextCapital(): Cypress.Chainable<JQuery<HTMLElement>> {
        cy.checkPath(this.path);
        return cy.getById(this.selectors.nextFinancialYearCapitalInput);
    }

    static markComplete(): void {
        cy.checkPath(this.path);
        cy.getById(this.selectors.completeCheckbox).check();
    }

    static markIncomplete(): void {
        cy.checkPath(this.path);
        cy.getById(this.selectors.completeCheckbox).uncheck();
    }
}
