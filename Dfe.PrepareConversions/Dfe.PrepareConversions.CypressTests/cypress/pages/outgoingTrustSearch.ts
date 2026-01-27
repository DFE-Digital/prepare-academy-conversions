class OutgoingTrustSearchPage {
    public slug = 'transfers/trustname';

    public searchTrustsByName(trustName): this {
        cy.getById('SearchQuery').type(trustName);

        cy.clickButton('Search');

        return this;
    }

    public searchTrustsByUkprn(ukprn): this {
        cy.getById('SearchQuery').type(ukprn);

        cy.clickButton('Search');

        return this;
    }

    public searchTrustsByCompaniesHouseNo(companiesHouseNo): this {
        cy.getById('SearchQuery').type(companiesHouseNo);

        cy.clickButton('Search');

        return this;
    }
}

const outgoingTrustSearchPage = new OutgoingTrustSearchPage();

export default outgoingTrustSearchPage;
