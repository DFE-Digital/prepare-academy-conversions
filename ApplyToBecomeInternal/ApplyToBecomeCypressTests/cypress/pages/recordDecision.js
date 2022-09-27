class recordDecision {
    urlSliceID() {
        let url = Cypress.env('url')
        cy.visit(url)
        cy.get('[data-cy="select-projecttype-input-conversion"]').click()
        cy.get('[data-cy="select-common-submitbutton"]').click()
        cy.get('[id="school-name-0"]').click()
        let urlSplit = cy.url().toString().split("/");
        const id = urlSplit[urlSplit.length - 1];
        return id;
    }
}

export default new recordDecision()