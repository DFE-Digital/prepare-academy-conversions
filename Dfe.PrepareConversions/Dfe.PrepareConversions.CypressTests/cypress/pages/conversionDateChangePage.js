export class ConversionDateChangePage {

    searchSchool(schoolName) {
        cy.get('#SearchQuery').type(schoolName);
        cy.get('.autocomplete__input--default').first().click();
        return this;
    }

    clickContinue() {
        cy.get('[data-id="submit"]').click();
        return this;
    }

    selectNoAndContinue() {
        cy.get('[data-cy="select-legal-input-no"]').click();
        cy.get('[data-cy="select-common-submitbutton"]').click();
        return this;
    }

    assertSchoolDetails(schoolName, urn, localAuthority, schoolType) {
        cy.get('[data-cy="school-name"]').should('contain', schoolName);
        cy.get('.govuk-summary-list__value').eq(1).should('contain', urn);
        cy.get('.govuk-summary-list__value').eq(3).should('contain', localAuthority);
        cy.get('.govuk-summary-list__value').eq(5).should('contain', schoolType);
        return this;
    }

    clickOnFirstProject() {
        cy.get('[data-cy="select-projectlist-filter-title"]').type('Manchester Academy');
        cy.get('[data-cy="select-projectlist-filter-apply"]').click();
        cy.get('[data-cy="trust-name-Manchester Academy-0"]').click();
        return this;
    }

    navigateToConversionDateChangeSection() {
        cy.get('[data-cy="confirm_project_dates"]').click();
        return this;
    }

    updateAdvisoryBoardDate() {
        cy.get('[data-test="change-advisory-board-date"]').click();
        cy.get('#advisory-board-date-day').clear().type('12');
        cy.get('#advisory-board-date-month').clear().type('12');
        cy.get('#advisory-board-date-year').clear().type('2023');
        cy.get('[data-cy="select-common-submitbutton"]').click();

        return this;
    }

    checkAdvisoryBoardDateChange() {
        cy.get('#advisory-board-date').should('contain', '12 December 2023');

        return this;
    }
    updatePreviousAdvisoryBoardDate() {
        cy.get('[data-test="change-previous-advisory-board"]').click();
        cy.get('#previous-advisory-board-day').clear().type('12');
        cy.get('#previous-advisory-board-month').clear().type('12');
        cy.get('#previous-advisory-board-year').clear().type('2023');
        cy.get('[data-cy="select-common-submitbutton"]').click();
        return this;
    }


    checkPreviousAdvisoryBoardDateChange() {
        cy.get('#previous-advisory-board').should('contain', '12 December 2023');
        return this;
    }
    updateProposedConversionDate() {
        cy.get('[data-test="change-proposed-conversion-date"]').click();
        cy.get('#proposed-conversion-month').clear().type('12');
        cy.get('#proposed-conversion-year').clear().type('2025');
        cy.get('[data-cy="select-common-submitbutton"]').click();
        return this;
    }

    checkProposedConversionDateChange() {
        cy.get('#proposed-conversion-date').should('contain', '1 December 2025');
        return this;
    }
    confirmConversionDateChange() {
        cy.get('#confirm-and-continue-button').click();
        cy.get('[data-cy="conversion_date_history_menu"]').click();
        cy.get('[data-cy="current_proposed_date"]').should('contain', '1 December 2025');


        return this;
    }




    enterConversionDate(day, month, year) {
        cy.get('#conversion-date-day').clear().type(day);
        cy.get('#conversion-date-month').clear().type(month);
        cy.get('#conversion-date-year').clear().type(year);
        cy.get('#submit-btn').click();
        return this;
    }

    enterComments(comments) {
        cy.get('#Comments').clear().type(comments);
        cy.get('#submit-btn').click();
        return this;
    }

    verifyConversionDateDetails(date, comments) {
        cy.get('#conversion-date').should('contain', date);
        cy.get('#comments').should('contain', comments);
        cy.get('#submit-btn').click();
        return this;
    }

    changeConversionDateDetails(newDate, comments) {
        cy.get('#change-date-btn').click();
        cy.get('#conversion-date-day').clear().type(newDate.split(' ')[0]);
        cy.get('#conversion-date-month').clear().type(newDate.split(' ')[1]);
        cy.get('#conversion-date-year').clear().type(newDate.split(' ')[2]);
        cy.get('#submit-btn').click();
        cy.get('#Comments').clear().type(comments);
        cy.get('#submit-btn').click();
        return this;
    }

    verifyChangedConversionDateDetails(date, comments) {
        cy.get('#conversion-date').should('contain', date);
        cy.get('#comments').should('contain', comments);
        cy.get('#submit-btn').click();
        return this;
    }

    deleteProject(projectId) {
        cy.callAcademisationApi("DELETE", `/conversion-project/${projectId}/Delete`).then((response) => {
            expect(response.status).to.eq(200);
        });
    }
}

export const conversionDateChangePage = new ConversionDateChangePage();
