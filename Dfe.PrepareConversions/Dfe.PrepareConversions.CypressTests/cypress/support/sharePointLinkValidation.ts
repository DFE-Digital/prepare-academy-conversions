export const invalidSharePointLinks = [
    {
        name: 'a non-HTTPS link',
        value: 'http://educationgovuk.sharepoint.com/sites/evidence',
        error: 'The Supporting Evidence must have the https scheme',
    },
    {
        name: 'an unapproved SharePoint domain',
        value: 'https://other.sharepoint.com/sites/evidence',
        error: "Enter a Supporting Evidence in the correct format. SharePoint links start with 'https://educationgovuk.sharepoint.com' or 'https://educationgovuk-my.sharepoint.com/'",
    },
];

export const verifySharePointLinkValidation = (field: string, value: string, error: string): void => {
    cy.get(`input[name="${field}"]`).should('be.visible');
    cy.location('pathname').then((path) => {
        cy.get(`input[name="${field}"]`).clear();
        cy.get(`input[name="${field}"]`).type(value);
        cy.getById('save-and-continue-button').click();

        cy.location('pathname').should('eq', path);
        cy.get(`input[name="${field}"]`)
            .should('have.value', value)
            .and('have.class', 'govuk-input--error')
            .invoke('attr', 'aria-describedby')
            .then((errorId) => {
                expect(errorId).to.be.a('string').and.not.be.empty;
                cy.getById(errorId!).should('be.visible').and('contain.text', error);
            });
    });
};
