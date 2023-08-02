export default class BasePage {

    static setDesktopViewport() {
        cy.viewport('macbook-13')
    }

    static setLargeDesktopViewport() {
        cy.viewport(1980, 1080)
    }
}