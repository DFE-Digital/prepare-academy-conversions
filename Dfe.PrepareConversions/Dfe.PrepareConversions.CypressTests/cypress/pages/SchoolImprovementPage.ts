import { decisionPage } from './decisionPage';

export class SchoolImprovementPage {
  searchSchool(schoolName: string): this {
    cy.get('#SearchQuery').type(schoolName);
    cy.get('.autocomplete__input--default').first().click();
    return this;
  }

  clickContinue(): this {
    cy.get('[data-id="submit"]').click();
    return this;
  }

  selectNoAndContinue(): this {
    cy.get('[data-cy="select-legal-input-no"]').click();
    cy.get('[data-cy="select-common-submitbutton"]').click();
    return this;
  }

  assertSchoolDetails(
    schoolName: string,
    urn: string,
    localAuthority: string,
    schoolType: string
  ): this {
    cy.get('[data-cy="school-name"]').should('contain', schoolName);
    cy.get('.govuk-summary-list__value').eq(1).should('contain', urn);
    cy.get('.govuk-summary-list__value').eq(3).should('contain', localAuthority);
    cy.get('.govuk-summary-list__value').eq(5).should('contain', schoolType);
    return this;
  }

  clickOnFirstProject(): this {
    cy.get('[data-cy="select-projectlist-filter-title"]').type(
      'Manchester Academy'
    );
    decisionPage.clickApplyFilters();
    cy.get('[data-cy="trust-name-Manchester Academy-0"]').click();
    return this;
  }

  navigateToSchoolImprovementSection(): this {
    cy.get('[data-cy="school_improvement_plans_menu"]').click();
    return this;
  }

  clickAddSchoolImprovementPlan(): this {
    cy.get('[data-cy="add_school_improvement_plan"]').click();
    return this;
  }

  selectImprovementDetails(): this {
    cy.get('#localauthority-checkbox').click();
    return this;
  }

  saveImprovementDetails(): this {
    cy.get('#submit-btn').click();
    return this;
  }

  enterImprovementDetails(details: string): this {
    cy.get('#PlanProvider').type(details);
    cy.get('#submit-btn').click();
    return this;
  }

  enterImprovementEndDate(day: string, month: string, year: string): this {
    cy.get('#plan-start-date-day').clear();
    cy.get('#plan-start-date-day').type(day);
    cy.get('#plan-start-date-month').clear();
    cy.get('#plan-start-date-month').type(month);
    cy.get('#plan-start-date-year').clear();
    cy.get('#plan-start-date-year').type(year);
    cy.get('#submit-btn').click();
    return this;
  }

  enterExpectedEndDate(day: string, month: string, year: string): this {
    cy.get('#Other-radio').click();
    cy.get('#plan-end-date-other-day').clear();
    cy.get('#plan-end-date-other-day').type(day);
    cy.get('#plan-end-date-other-month').clear();
    cy.get('#plan-end-date-other-month').type(month);
    cy.get('#plan-end-date-other-year').clear();
    cy.get('#plan-end-date-other-year').type(year);
    cy.get('#submit-btn').click();
    return this;
  }

  selectHighConfidenceLevel(): this {
    cy.get('#High-radio').click();
    cy.get('#submit-btn').click();
    return this;
  }

  enterComments(comments: string): this {
    cy.get('#PlanComments').clear();
    cy.get('#PlanComments').type(comments);
    cy.get('#submit-btn').click();
    return this;
  }

  verifyImprovementDetails(
    arrangedBy: string,
    providedBy: string,
    startDate: string,
    endDate: string,
    confidenceLevel: string,
    comments: string
  ): this {
    cy.get('#arranged-by').should('contain', arrangedBy);
    cy.get('#provided-by').eq(0).should('contain', providedBy);
    cy.get('#start-date').should('contain', startDate);
    cy.get('#end-date').should('contain', endDate);
    cy.get('#confidence-level').should('contain', confidenceLevel);
    cy.get('#comments').should('contain', comments);
    cy.get('#submit-btn').click();
    return this;
  }

  verifyTheFinalImprovementDetails(
    arrangedBy: string,
    providedBy: string,
    startDate: string,
    endDate: string,
    confidenceLevel: string,
    comments: string
  ): this {
    cy.get('[data-cy="arranged-by"]').should('contain', arrangedBy);
    cy.get('[data-cy="provided-by"]').eq(0).should('contain', providedBy);
    cy.get('[data-cy="start-date"]').should('contain', startDate);
    cy.get('[data-cy="end-date"]').should('contain', endDate);
    cy.get('[data-cy="confidence-leve"]').should('contain', confidenceLevel);
    cy.get('[data-cy="comments"]').should('contain', comments);
    return this;
  }

  changeImprovementDetails(providedBy: string, comments: string): this {
    cy.get('#change-arranger-btn').click();
    cy.get('#regionaldirector-checkbox').click();
    cy.get('#submit-btn').click();
    cy.get('#PlanProvider').clear();
    cy.get('#PlanProvider').type(providedBy);
    cy.get('#submit-btn').click();
    cy.get('#submit-btn').click();
    cy.get('#Unknown-radio').click();
    cy.get('#submit-btn').click();
    cy.get('#Medium-radio').click();
    cy.get('#submit-btn').click();
    cy.get('#PlanComments').clear();
    cy.get('#PlanComments').type(comments);
    cy.get('#submit-btn').click();
    return this;
  }

  verifyChangedImprovementDetails(
    arrangedBy: string,
    providedBy: string,
    startDate: string,
    endDate: string,
    confidenceLevel: string,
    comments: string
  ): this {
    cy.get('#arranged-by').should('contain', arrangedBy);
    cy.get('#provided-by').eq(0).should('contain', providedBy);
    cy.get('#start-date').should('contain', startDate);
    cy.get('#end-date').should('contain', endDate);
    cy.get('#confidence-level').should('contain', confidenceLevel);
    cy.get('#comments').should('contain', comments);
    cy.get('#submit-btn').click();
    return this;
  }

  deleteProject(projectId: string): void {
    cy.callAcademisationApi(
      'DELETE',
      `/conversion-project/${projectId}/Delete`
    ).then((response) => {
      expect(response.status).to.eq(200);
    });
  }
}

export const schoolImprovementPage = new SchoolImprovementPage();
