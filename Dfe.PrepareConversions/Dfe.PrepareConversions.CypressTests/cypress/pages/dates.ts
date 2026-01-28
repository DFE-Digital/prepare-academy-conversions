import BasePage from './basePage';

class DatesPage extends BasePage {
    public path = 'features';

    public completeAdvisoryBoardDate(date): this {
        cy.getByDataTest('ab-date').click();

        cy.get('h1').should('contain.text', 'Advisory board date');

        cy.getByDataTest('day').clear();
        cy.getByDataTest('day').type(date.date());
        cy.getByDataTest('month').clear();
        cy.getByDataTest('month').type(date.month() + 1);
        cy.getByDataTest('year').clear();
        cy.getByDataTest('year').type(date.year());

        this.saveAndContinue();

        // Check the table has been updated
        cy.get('dd').eq(0).should('contain.text', date.format('D MMMM YYYY'));

        return this;
    }

    public completeExpectedTransferDate(date): this {
        cy.getByDataTest('target-date').click();

        cy.getByDataTest('month').clear();
        cy.getByDataTest('month').type(date.month() + 1);
        cy.getByDataTest('year').clear();
        cy.getByDataTest('year').type(date.year());

        this.saveAndContinue();

        // Check the table has been updated
        cy.get('dd').eq(4).should('contain.text', date.format('1 MMMM YYYY'));

        return this;
    }

    public confirmDates(): this {
        this.confirmAndContinue();
        return this;
    }
}

const datesPage = new DatesPage();

export default datesPage;
