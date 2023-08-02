import BasePage from "../BasePage"
export default class PrepareHome extends BasePage {


static selectManageAnAcademyConversionAndClickContinue() 
{
        cy.get('.govuk-grid-column-two-thirds > .govuk-button').click()
}
}