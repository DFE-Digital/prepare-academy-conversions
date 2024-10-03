/// <reference types ='Cypress'/>

import BasePage from "./BasePage"

const currentYear = new Date()
const nextYear = new Date().setFullYear(new Date().getFullYear() + 1)

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
        completeCheckbox: '[id="school-budget-information-complete"]'
    }

    static path = 'budget'

    static updateBudgetInfomation({
        endOfFinanicalYear,
        forecastedRevenueCurrentYear,
        forecastedCapitalCurrentYear,
        endOfNextFinancialYear,
        forecastedRevenueNextYear,
        forecastedCapitalNextYear
    }) {
        if (!endOfFinanicalYear) {
            endOfFinanicalYear = currentYear
        }

        if (!endOfNextFinancialYear) {
            endOfNextFinancialYear = nextYear
        }
        cy.checkPath(this.path)

        cy.get(this.selectors.changeEndOfFinancialYearLink).click()

        cy.get(this.selectors.currentFinancialYearDayInput).clear()
        cy.get(this.selectors.currentFinancialYearDayInput).type(endOfFinanicalYear.getDate())
        cy.get(this.selectors.currentFinancialYearMonthInput).clear()
        cy.get(this.selectors.currentFinancialYearMonthInput).type(endOfFinanicalYear.getMonth() + 1) // Add 1 as months start at 0
        cy.get(this.selectors.currentFinancialYearYearInput).clear()
        cy.get(this.selectors.currentFinancialYearYearInput).type(endOfFinanicalYear.getFullYear())
        cy.get(this.selectors.currentFinancialYearRevenueInput).clear()
        cy.get(this.selectors.currentFinancialYearRevenueInput).type(forecastedRevenueCurrentYear)
        cy.get(this.selectors.currentFinancialYearCapitalInput).clear()
        cy.get(this.selectors.currentFinancialYearCapitalInput).type(forecastedCapitalCurrentYear)

        cy.get(this.selectors.nextFinancialYearDayInput).clear()
        cy.get(this.selectors.nextFinancialYearDayInput).type(endOfNextFinancialYear.getDate())
        cy.get(this.selectors.nextFinancialYearMonthInput).clear()
        cy.get(this.selectors.nextFinancialYearMonthInput).type(endOfNextFinancialYear.getMonth() + 1) // Add 1 as months start at 0
        cy.get(this.selectors.nextFinancialYearYearInput).clear()
        cy.get(this.selectors.nextFinancialYearYearInput).type(endOfNextFinancialYear.getFullYear())
        cy.get(this.selectors.nextFinancialYearRevenueInput).clear()
        cy.get(this.selectors.nextFinancialYearRevenueInput).type(forecastedRevenueNextYear)
        cy.get(this.selectors.nextFinancialYearCapitalInput).clear()
        cy.get(this.selectors.nextFinancialYearCapitalInput).type(forecastedCapitalNextYear)

        cy.get(this.selectors.saveButton).click()
    }

    static getCurrentFinancialYear() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.currentFinancialYearValue)
    }

    static getCurrentRevenue() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.currentFinancialYearRevenueValue)
    }

    static getCurrentCapital() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.currentFinancialYearCapitalValue)
    }

    static getNextFinancialYear() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.nextFinancialYearValue)
    }

    static getNextRevenue() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.nextFinancialYearRevenueValue)
    }

    static getNextCapital() {
        cy.checkPath(this.path)
        return cy.get(this.selectors.nextFinancialYearCapitalValue)
    }

    static markComplete() {
        cy.checkPath(this.path)
        cy.get(this.selectors.completeCheckbox).check()
    }

    static markIncomplete() {
        cy.checkPath(this.path)
        cy.get(this.selectors.completeCheckbox).uncheck()
    }
}
