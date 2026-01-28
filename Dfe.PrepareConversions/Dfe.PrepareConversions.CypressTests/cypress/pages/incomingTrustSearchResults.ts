import BasePage from './basePage';

class IncomingTrustSearchResultsPage extends BasePage {
    public path = 'transfers/searchincomingtrust';

    public selectTrust(trustName): this {
        cy.get('label').contains(trustName).click();
        this.continue();
        return this;
    }
}

const incomingTrustSearchResultsPage = new IncomingTrustSearchResultsPage();

export default incomingTrustSearchResultsPage;
