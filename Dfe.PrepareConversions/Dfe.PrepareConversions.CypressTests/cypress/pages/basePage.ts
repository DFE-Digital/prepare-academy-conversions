export default class BasePage {
    public path: string = '';

    public checkPath(): this {
        if (this.path) {
            cy.checkPath(this.path);
        }
        return this;
    }

    public continue(): this {
        cy.get('button').contains('Continue').click();
        return this;
    }

    public confirmAndContinue(): this {
        cy.containsText('Confirm and continue').click();
        return this;
    }

    public saveAndContinue(): this {
        cy.containsText('Save and continue').click();
        return this;
    }

    public clickSaveButton(): this {
        cy.getByDataCy('select-common-submitbutton').click();
        return this;
    }

    public clickBackLink(): this {
        cy.getByDataCy('select-backlink').click();
        return this;
    }

    public markComplete(checkboxSelector: string): this {
        if (this.path) {
            cy.checkPath(this.path);
        }
        cy.get(`#${checkboxSelector}`).check();
        return this;
    }

    public markIncomplete(checkboxSelector: string): this {
        if (this.path) {
            cy.checkPath(this.path);
        }
        cy.get(`#${checkboxSelector}`).uncheck();
        return this;
    }

    public markSectionComplete(): this {
        cy.getByDataTest('mark-section-complete').click();
        return this;
    }

    public getById(id: string): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.getById(id);
    }

    public getByDataTest(value: string): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.getByDataTest(value);
    }

    public getByDataCy(value: string): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.getByDataCy(value);
    }

    public enterDate(inputPrefix: string, day: string, month: string, year: string): this {
        cy.getById(`${inputPrefix}-day`).clear().type(day);
        cy.getById(`${inputPrefix}-month`).clear().type(month);
        cy.getById(`${inputPrefix}-year`).clear().type(year);
        return this;
    }

    public selectRadioByValue(value: string): this {
        cy.get(`[value="${value}"]`).click();
        return this;
    }

    public checkById(id: string): this {
        cy.getById(id).check();
        return this;
    }

    public typeIntoField(id: string, text: string): this {
        cy.getById(id).clear().type(text);
        return this;
    }

    public verifyContainsText(selector: string, expectedText: string): this {
        cy.get(selector).should('contain', expectedText);
        return this;
    }

    public verifyHeading(expectedText: string): this {
        cy.get('h1').should('contain.text', expectedText);
        return this;
    }
}
