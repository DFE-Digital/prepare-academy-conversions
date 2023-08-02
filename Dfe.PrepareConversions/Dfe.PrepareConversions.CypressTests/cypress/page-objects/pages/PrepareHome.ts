import BasePage from "../BasePage"
export default class PrepareHome extends BasePage {


static selectManageAnAcademyConversionAndClickContinue() 
{
    cy.get('#conversion-radio').click()
    cy.get('#submit-btn').click()
}
}