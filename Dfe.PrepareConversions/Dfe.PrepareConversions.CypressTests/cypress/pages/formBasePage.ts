import BasePage from './basePage';

export default class FormBasePage extends BasePage {
    public searchSchool(schoolName: string): this {
        cy.getById('SearchQuery').type(schoolName);
        cy.get('.autocomplete__input--default').first().click();
        return this;
    }

    public clickContinue(): this {
        cy.getByDataId('submit').click();
        return this;
    }

    public selectNoAndContinue(): this {
        cy.getByDataCy('select-legal-input-no').click();
        cy.getByDataCy('select-common-submitbutton').click();
        return this;
    }

    public assertSchoolDetails(schoolName: string, urn: string, localAuthority: string, schoolType: string): this {
        cy.getByDataCy('school-name').should('contain', schoolName);
        cy.get('.govuk-summary-list__value').eq(1).should('contain', urn);
        cy.get('.govuk-summary-list__value').eq(3).should('contain', localAuthority);
        cy.get('.govuk-summary-list__value').eq(5).should('contain', schoolType);
        return this;
    }

    public clickOnFirstProject(projectName: string = 'Manchester Academy'): this {
        cy.getByDataCy('select-projectlist-filter-title').type(projectName);
        this.clickApplyFilters();
        cy.getByDataCy(`trust-name-${projectName}-0`).click();
        return this;
    }

    public clickApplyFilters(): this {
        cy.getByDataCy('select-projectlist-filter-apply').first().click();
        return this;
    }

    public clickSubmitButton(): this {
        cy.clickSubmitBtn();
        return this;
    }

    public deleteConversionProject(projectId: string): void {
        cy.callAcademisationApi('DELETE', `/conversion-project/${projectId}/Delete`).then((response) => {
            expect(response.status).to.eq(200);
        });
    }

    public makeDecision(decision: string): this {
        cy.getById(`${decision}-radio`).click();
        cy.clickSubmitBtn();
        return this;
    }

    public selectDecisionMaker(grade: string): this {
        cy.getById(`${grade}-radio`).click();
        cy.clickSubmitBtn();
        return this;
    }

    public enterDecisionMakerName(name: string): this {
        cy.getById('decision-maker-name').type(name);
        cy.clickSubmitBtn();
        return this;
    }

    public enterDecisionDate(day: string, month: string, year: string): this {
        cy.getById('decision-date-day').type(day);
        cy.getById('decision-date-month').type(month);
        cy.getById('decision-date-year').type(year);
        cy.clickSubmitBtn();
        return this;
    }
}
