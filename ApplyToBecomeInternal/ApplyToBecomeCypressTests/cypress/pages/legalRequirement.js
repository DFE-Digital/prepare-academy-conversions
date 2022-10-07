class legalRequirements {
    selectProject() {
        let url = Cypress.env('url');
        cy.visit(url);
        cy.get('[data-cy="select-projecttype-input-conversion"]').click();
        cy.get('[data-cy="select-common-submitbutton"]').click();
        cy.get('[id="school-name-0"]').click();
        

        return cy.url()
    };
}

export default new legalRequirements();