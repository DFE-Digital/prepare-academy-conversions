import FormBasePage from './formBasePage';

class SchoolImprovementPage extends FormBasePage {
    public path = 'school-improvement';

    public navigateToSchoolImprovementSection(): this {
        cy.getByDataCy('school_improvement_plans_menu').click();
        return this;
    }

    public clickAddSchoolImprovementPlan(): this {
        cy.getByDataCy('add_school_improvement_plan').click();
        return this;
    }

    public selectImprovementDetails(): this {
        cy.getById('localauthority-checkbox').click();
        return this;
    }

    public saveImprovementDetails(): this {
        cy.getById('submit-btn').click();
        return this;
    }

    public enterImprovementDetails(details: string): this {
        cy.getById('PlanProvider').type(details);
        cy.getById('submit-btn').click();
        return this;
    }

    public enterImprovementEndDate(day: string, month: string, year: string): this {
        cy.getById('plan-start-date-day').clear();
        cy.getById('plan-start-date-day').type(day);
        cy.getById('plan-start-date-month').clear();
        cy.getById('plan-start-date-month').type(month);
        cy.getById('plan-start-date-year').clear();
        cy.getById('plan-start-date-year').type(year);
        cy.getById('submit-btn').click();
        return this;
    }

    public enterExpectedEndDate(day: string, month: string, year: string): this {
        cy.getById('Other-radio').click();
        cy.getById('plan-end-date-other-day').clear();
        cy.getById('plan-end-date-other-day').type(day);
        cy.getById('plan-end-date-other-month').clear();
        cy.getById('plan-end-date-other-month').type(month);
        cy.getById('plan-end-date-other-year').clear();
        cy.getById('plan-end-date-other-year').type(year);
        cy.getById('submit-btn').click();
        return this;
    }

    public selectHighConfidenceLevel(): this {
        cy.getById('High-radio').click();
        cy.getById('submit-btn').click();
        return this;
    }

    public enterComments(comments: string): this {
        cy.getById('PlanComments').clear();
        cy.getById('PlanComments').type(comments);
        cy.getById('submit-btn').click();
        return this;
    }

    public verifyImprovementDetails(
        arrangedBy: string,
        providedBy: string,
        startDate: string,
        endDate: string,
        confidenceLevel: string,
        comments: string
    ): this {
        cy.getById('arranged-by').should('contain', arrangedBy);
        cy.getById('provided-by').eq(0).should('contain', providedBy);
        cy.getById('start-date').should('contain', startDate);
        cy.getById('end-date').should('contain', endDate);
        cy.getById('confidence-level').should('contain', confidenceLevel);
        cy.getById('comments').should('contain', comments);
        cy.getById('submit-btn').click();
        return this;
    }

    public verifyTheFinalImprovementDetails(
        arrangedBy: string,
        providedBy: string,
        startDate: string,
        endDate: string,
        confidenceLevel: string,
        comments: string
    ): this {
        cy.getByDataCy('arranged-by').should('contain', arrangedBy);
        cy.getByDataCy('provided-by').eq(0).should('contain', providedBy);
        cy.getByDataCy('start-date').should('contain', startDate);
        cy.getByDataCy('end-date').should('contain', endDate);
        cy.getByDataCy('confidence-leve').should('contain', confidenceLevel);
        cy.getByDataCy('comments').should('contain', comments);
        return this;
    }

    public changeImprovementDetails(providedBy: string, comments: string): this {
        cy.getById('change-arranger-btn').click();
        cy.getById('regionaldirector-checkbox').click();
        cy.getById('submit-btn').click();
        cy.getById('PlanProvider').clear();
        cy.getById('PlanProvider').type(providedBy);
        cy.getById('submit-btn').click();
        cy.getById('submit-btn').click();
        cy.getById('Unknown-radio').click();
        cy.getById('submit-btn').click();
        cy.getById('Medium-radio').click();
        cy.getById('submit-btn').click();
        cy.getById('PlanComments').clear();
        cy.getById('PlanComments').type(comments);
        cy.getById('submit-btn').click();
        return this;
    }

    public verifyChangedImprovementDetails(
        arrangedBy: string,
        providedBy: string,
        startDate: string,
        endDate: string,
        confidenceLevel: string,
        comments: string
    ): this {
        cy.getById('arranged-by').should('contain', arrangedBy);
        cy.getById('provided-by').eq(0).should('contain', providedBy);
        cy.getById('start-date').should('contain', startDate);
        cy.getById('end-date').should('contain', endDate);
        cy.getById('confidence-level').should('contain', confidenceLevel);
        cy.getById('comments').should('contain', comments);
        cy.getById('submit-btn').click();
        return this;
    }

    public deleteProject(projectId: string): void {
        this.deleteConversionProject(projectId);
    }
}

const schoolImprovementPage = new SchoolImprovementPage();

export { schoolImprovementPage };
export default schoolImprovementPage;
