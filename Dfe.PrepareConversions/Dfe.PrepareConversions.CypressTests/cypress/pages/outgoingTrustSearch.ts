class OutgoingTrustSearchPage {
    public slug = 'transfers/trustname';

    public searchTrustsByName(trustName): this {
        cy.getById('SearchQuery').type(trustName);

        cy.containsText('Search').click();

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

const outgoingTrustSearchPage = new OutgoingTrustSearchPage();

export default outgoingTrustSearchPage;
