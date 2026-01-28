import FormBasePage from './formBasePage';

class ConversionDateChangePage extends FormBasePage {
    public path = 'conversion-date-change';

    public navigateToConversionDateChangeSection(): this {
        cy.getByDataCy('confirm_project_dates').click();
        return this;
    }

    public updateAdvisoryBoardDate(): this {
        cy.getByDataTest('change-advisory-board-date').click();
        cy.getById('advisory-board-date-day').clear().type('12');
        cy.getById('advisory-board-date-month').clear().type('12');
        cy.getById('advisory-board-date-year').clear().type('2023');
        cy.getByDataCy('select-common-submitbutton').click();

        return this;
    }

    public checkAdvisoryBoardDateChange(): this {
        cy.getById('advisory-board-date').should('contain', '12 December 2023');

        return this;
    }

    public updatePreviousAdvisoryBoardDate(): this {
        cy.getByDataTest('change-previous-advisory-board').click();
        cy.getById('previous-advisory-board-day').clear().type('12');
        cy.getById('previous-advisory-board-month').clear().type('12');
        cy.getById('previous-advisory-board-year').clear().type('2023');
        cy.getByDataCy('select-common-submitbutton').click();
        return this;
    }

    public checkPreviousAdvisoryBoardDateChange(): this {
        cy.getById('previous-advisory-board').should('contain', '12 December 2023');
        return this;
    }

    public updateProposedConversionDate(): this {
        cy.getByDataTest('change-proposed-conversion-date').click();
        cy.getById('proposed-conversion-month').clear().type('12');
        cy.getById('proposed-conversion-year').clear().type('2025');
        cy.getByDataCy('select-common-submitbutton').click();
        return this;
    }

    public checkProposedConversionDateChange(): this {
        cy.getById('proposed-conversion-date').should('contain', '1 December 2025');
        return this;
    }

    public confirmConversionDateChange(): this {
        cy.getById('confirm-and-continue-button').click();
        cy.getByDataCy('conversion_date_history_menu').click();
        cy.getByDataCy('current_proposed_date').should('contain', '1 December 2025');

        return this;
    }

    public enterConversionDate(day: string, month: string, year: string): this {
        cy.getById('conversion-date-day').clear().type(day);
        cy.getById('conversion-date-month').clear().type(month);
        cy.getById('conversion-date-year').clear().type(year);
        cy.getById('submit-btn').click();
        return this;
    }

    public enterComments(comments: string): this {
        cy.getById('Comments').clear().type(comments);
        cy.getById('submit-btn').click();
        return this;
    }

    public verifyConversionDateDetails(date: string, comments: string): this {
        cy.getById('conversion-date').should('contain', date);
        cy.getById('comments').should('contain', comments);
        cy.getById('submit-btn').click();
        return this;
    }

    public changeConversionDateDetails(newDate: string, comments: string): this {
        cy.getById('change-date-btn').click();
        cy.getById('conversion-date-day').clear().type(newDate.split(' ')[0]);
        cy.getById('conversion-date-month').clear().type(newDate.split(' ')[1]);
        cy.getById('conversion-date-year').clear().type(newDate.split(' ')[2]);
        cy.getById('submit-btn').click();
        cy.getById('Comments').clear().type(comments);
        cy.getById('submit-btn').click();
        return this;
    }

    public deleteProject(projectId: string): void {
        this.deleteConversionProject(projectId);
    }
}

const conversionDateChangePage = new ConversionDateChangePage();

export { conversionDateChangePage };
export default conversionDateChangePage;
