import { decisionPage } from './decisionPage';

export class ConversionDateChangePage {
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

  navigateToConversionDateChangeSection(): this {
    cy.get('[data-cy="confirm_project_dates"]').click();
    return this;
  }

  updateAdvisoryBoardDate(): this {
    cy.get('[data-test="change-advisory-board-date"]').click();
    cy.get('#advisory-board-date-day').clear().type('12');
    cy.get('#advisory-board-date-month').clear().type('12');
    cy.get('#advisory-board-date-year').clear().type('2023');
    cy.get('[data-cy="select-common-submitbutton"]').click();

    return this;
  }

  checkAdvisoryBoardDateChange(): this {
    cy.get('#advisory-board-date').should('contain', '12 December 2023');

    return this;
  }

  updatePreviousAdvisoryBoardDate(): this {
    cy.get('[data-test="change-previous-advisory-board"]').click();
    cy.get('#previous-advisory-board-day').clear().type('12');
    cy.get('#previous-advisory-board-month').clear().type('12');
    cy.get('#previous-advisory-board-year').clear().type('2023');
    cy.get('[data-cy="select-common-submitbutton"]').click();
    return this;
  }

  checkPreviousAdvisoryBoardDateChange(): this {
    cy.get('#previous-advisory-board').should('contain', '12 December 2023');
    return this;
  }

  updateProposedConversionDate(): this {
    cy.get('[data-test="change-proposed-conversion-date"]').click();
    cy.get('#proposed-conversion-month').clear().type('12');
    cy.get('#proposed-conversion-year').clear().type('2025');
    cy.get('[data-cy="select-common-submitbutton"]').click();
    return this;
  }

  checkProposedConversionDateChange(): this {
    cy.get('#proposed-conversion-date').should('contain', '1 December 2025');
    return this;
  }

  confirmConversionDateChange(): this {
    cy.get('#confirm-and-continue-button').click();
    cy.get('[data-cy="conversion_date_history_menu"]').click();
    cy.get('[data-cy="current_proposed_date"]').should(
      'contain',
      '1 December 2025'
    );

    return this;
  }

  enterConversionDate(day: string, month: string, year: string): this {
    cy.get('#conversion-date-day').clear().type(day);
    cy.get('#conversion-date-month').clear().type(month);
    cy.get('#conversion-date-year').clear().type(year);
    cy.get('#submit-btn').click();
    return this;
  }

  enterComments(comments: string): this {
    cy.get('#Comments').clear().type(comments);
    cy.get('#submit-btn').click();
    return this;
  }

  verifyConversionDateDetails(date: string, comments: string): this {
    cy.get('#conversion-date').should('contain', date);
    cy.get('#comments').should('contain', comments);
    cy.get('#submit-btn').click();
    return this;
  }

  changeConversionDateDetails(newDate: string, comments: string): this {
    cy.get('#change-date-btn').click();
    cy.get('#conversion-date-day').clear().type(newDate.split(' ')[0]);
    cy.get('#conversion-date-month').clear().type(newDate.split(' ')[1]);
    cy.get('#conversion-date-year').clear().type(newDate.split(' ')[2]);
    cy.get('#submit-btn').click();
    cy.get('#Comments').clear().type(comments);
    cy.get('#submit-btn').click();
    return this;
  }

  verifyChangedConversionDateDetails(date: string, comments: string): this {
    cy.get('#conversion-date').should('contain', date);
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

export const conversionDateChangePage = new ConversionDateChangePage();
