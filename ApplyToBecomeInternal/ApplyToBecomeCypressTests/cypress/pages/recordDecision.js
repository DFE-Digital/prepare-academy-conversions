class RecordDecision {

    selectProject() {
        let url = Cypress.env('url');
        cy.visit(url);
        cy.get('[data-cy="select-projecttype-input-conversion"]').click();
        cy.get('[data-cy="select-common-submitbutton"]').click();
        cy.get('[id="school-name-0"]').click();
        
        return cy.url().then(url => this.getIdFromUrl(url));
    };

    getIdFromUrl(url) {
        const urlSplit = url.toString().split('/');        
        return urlSplit[urlSplit.length - 1];        
    };
};

export default new RecordDecision();