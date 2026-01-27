class RationalePage {
    public slug = 'rationale';

    public completeRationale(): this {
        cy.getByDataTest('project-rationale').click();

        cy.get('h1').should('contain.text', 'Write the rationale for the project');

        cy.getByDataTest('project-rationale').clear();
        cy.getByDataTest('project-rationale').type('Cypress project rationale');

        cy.containsText('Save and continue').click();

        // Check the table has been updated
        cy.get('dd').eq(0).should('contain.text', 'Cypress project rationale');

        return this;
    }

    public completeChosenReason(): this {
        cy.getByDataTest('trust-rationale').click();

        cy.get('h1').should('contain.text', 'Write the rationale for the incoming trust or sponsor');

        cy.getByDataTest('trust-rationale').clear();
        cy.getByDataTest('trust-rationale').type('Cypress trust rationale');

        cy.containsText('Save and continue').click();

        // Check the table has been updated
        cy.get('dd').eq(2).should('contain.text', 'Cypress trust rationale');

        return this;
    }

    public markAsComplete(): this {
        cy.getByDataTest('mark-section-complete').click();
        return this;
    }

    public confirmRationale(): this {
        cy.containsText('Confirm and continue').click();

        return this;
    }
}

const rationalePage = new RationalePage();

export default rationalePage;
