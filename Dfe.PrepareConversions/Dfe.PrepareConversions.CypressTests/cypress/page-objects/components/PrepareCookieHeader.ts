import BasePage from "../BasePage"
export default class PrepareCookieHeader extends BasePage {
    static clickAcceptAnalyticsCookies() {
        cy.get('a[role="button"').eq(0).click()
    }

}