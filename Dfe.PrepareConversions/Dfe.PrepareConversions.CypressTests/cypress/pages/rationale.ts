import BasePage from './basePage';

class RationalePage extends BasePage {
    public path = 'rationale';

    public completeRationale(): this {
        cy.getByDataTest('project-rationale').click();

        cy.get('h1').should('contain.text', 'Write the rationale for the project');

        cy.getByDataTest('project-rationale').clear();
        cy.getByDataTest('project-rationale').type('Cypress project rationale');

        this.saveAndContinue();

        // Check the table has been updated
        cy.get('dd').eq(0).should('contain.text', 'Cypress project rationale');

        return this;
    }

    public completeChosenReason(): this {
        cy.getByDataTest('trust-rationale').click();

        cy.get('h1').should('contain.text', 'Write the rationale for the incoming trust or sponsor');

        cy.getByDataTest('trust-rationale').clear();
        cy.getByDataTest('trust-rationale').type('Cypress trust rationale');

        this.saveAndContinue();

        // Check the table has been updated
        cy.get('dd').eq(2).should('contain.text', 'Cypress trust rationale');

        return this;
    }

    public markAsComplete(): this {
        this.markSectionComplete();
        return this;
    }

    public confirmRationale(): this {
        this.confirmAndContinue();
        return this;
    }
}

const rationalePage = new RationalePage();

export default rationalePage;
