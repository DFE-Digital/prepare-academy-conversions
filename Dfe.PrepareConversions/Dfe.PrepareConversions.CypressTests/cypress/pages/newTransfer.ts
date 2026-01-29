import BasePage from './basePage';

class NewTransferPage extends BasePage {
    public path = 'transfers/newtransfersinformation';

    public clickCreateNewTransfer(): this {
        cy.get('.govuk-button').contains('Create a new transfer').click();
        return this;
    }
}

const newTransferPage = new NewTransferPage();

export default newTransferPage;
