class CookiePolicyPage {
    public slug = 'cookie-preferences';

    private cookieName = '.ManageAnAcademyConversion.Consent';

    public cookieBannerShouldBeVisible(): this {
        cy.getByDataTest('cookie-banner').should('be.visible');
        return this;
    }

    public cookieBannerShouldNotBeVisible(): this {
        cy.getByDataTest('cookie-banner').should('not.exist');
        return this;
    }

    public acceptCookiesFromBanner(): this {
        cy.getByDataTest('cookie-banner-accept').click();
        return this;
    }

    public verifyCookieConsentIsSet(value: string): this {
        cy.getCookie(this.cookieName).should('exist').should('have.property', 'value', value);
        return this;
    }

    public acceptedBannerShouldBeVisible(): this {
        cy.getById('acceptCookieBanner').should('be.visible');
        return this;
    }

    public clickCookiePreferencesLink(): this {
        cy.getByDataTest('cookie-preferences').click();
        return this;
    }

    public verifyOnCookiePreferencesPage(): this {
        cy.checkPath(this.slug);
        return this;
    }

    public selectDenyCookies(): this {
        cy.getById('cookie-consent-deny').click();
        return this;
    }

    public selectAcceptCookies(): this {
        cy.getById('cookie-consent-accept').click();
        return this;
    }

    public submitCookiePreferences(): this {
        cy.get('[data-qa="submit"]').click();
        return this;
    }

    public clickSuccessBannerReturnLink(): this {
        cy.getByDataTest('success-banner-return-link').click();
        return this;
    }

    public verifyRedirectedToProjectList(): this {
        cy.checkPath('/project-list');
        return this;
    }
}

const cookiePolicyPage = new CookiePolicyPage();

export default cookiePolicyPage;
