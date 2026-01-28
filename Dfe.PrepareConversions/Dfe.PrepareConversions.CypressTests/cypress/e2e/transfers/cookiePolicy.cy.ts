import cookiePolicyPage from 'cypress/pages/cookiePolicyPage';
import { Logger } from '../../support/logger';

describe('Cookie Policy', () => {
    beforeEach(() => {
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
    });

    it('Should show cookie banner when no preference set', () => {
        cookiePolicyPage.cookieBannerShouldBeVisible();
    });

    context('When cookie banner clicked', () => {
        beforeEach(() => {
            Logger.log('Accept cookies from banner');
            cookiePolicyPage.acceptCookiesFromBanner();
        });

        it('Should consent to cookies from cookie header button', () => {
            cookiePolicyPage.verifyCookieConsentIsSet('True');
        });

        it('Should hide the cookie banner when consent has been given', () => {
            cookiePolicyPage.acceptedBannerShouldBeVisible();
        });
    });

    context('When cookie link in footer clicked', () => {
        beforeEach(() => {
            Logger.log('Click cookie preferences link in footer');
            cookiePolicyPage.clickCookiePreferencesLink();
        });

        it('Should navigate to cookies page', () => {
            cookiePolicyPage.verifyOnCookiePreferencesPage();
        });

        it('Should set cookie preferences', () => {
            cookiePolicyPage.selectDenyCookies().submitCookiePreferences().verifyCookieConsentIsSet('False');
        });

        it('Should show success banner and return to project list', () => {
            cookiePolicyPage
                .selectDenyCookies()
                .submitCookiePreferences()
                .clickSuccessBannerReturnLink()
                .verifyRedirectedToProjectList();
        });
    });

    it('Check accessibility across pages', () => {
        cy.checkAccessibilityAcrossPages();
    });
});
