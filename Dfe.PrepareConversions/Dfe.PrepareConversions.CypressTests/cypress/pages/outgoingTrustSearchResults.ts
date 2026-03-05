import BasePage from './basePage';

class OutgoingTrustSearchResultsPage extends BasePage {
    public path = 'transfers/trustsearch';

    public selectTrust(trustName): this {
        cy.get('label').contains(trustName).click();
        this.continue();
        return this;
    }
}

const outgoingTrustSearchResultsPage = new OutgoingTrustSearchResultsPage();

export default outgoingTrustSearchResultsPage;
