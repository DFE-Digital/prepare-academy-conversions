import BasePage from './basePage';

class IncomingTrustSearchPage extends BasePage {
    public path = 'transfers/searchincomingtrust';

    public searchTrustsByName(trustName): this {
        cy.getByDataCy('ProposedTrustNameID').type(trustName);
        cy.clickContinueBtn();
        return this;
    }

    public searchTrustsByUkprn(ukprn): this {
        cy.getById('SearchQuery').type(ukprn);
        cy.containsText('Search').click();
        return this;
    }

    public searchTrustsByCompaniesHouseNo(companiesHouseNo): this {
        cy.getById('SearchQuery').type(companiesHouseNo);
        cy.containsText('Search').click();
        return this;
    }
}

const incomingTrustSearchPage = new IncomingTrustSearchPage();

export default incomingTrustSearchPage;
