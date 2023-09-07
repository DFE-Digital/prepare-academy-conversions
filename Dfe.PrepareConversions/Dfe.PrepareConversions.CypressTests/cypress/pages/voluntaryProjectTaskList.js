import BasePage from './BasePage'

export default class voluntaryProjectTaskList extends BasePage {


    static voluntaryProjectElementsVisible()
    {
        var projectName = 'Voluntary Cypress Project'
        var routeType = 'Voluntary conversion'
        var projectOwner = ''
        var projectStatus = 'Approved with Conditions'

        // Check Header section of main Project Page
        cy.get('h1[data-cy]').contains(projectName)
        cy.get('p[class="govuk-body govuk-!-margin-bottom-1"]').contains('Route: ' + routeType)
        cy.get('p[class="govuk-body govuk-!-margin-bottom-5"]').contains('Project owner: ' + projectOwner)
        cy.get('.empty').contains('Empty')
        cy.get('a[class="govuk-link govuk-!-padding-left-50"]').contains('Change')
        cy.get('strong[class="govuk-tag govuk-tag--green"]').contains(projectStatus)

        // Check Project Tabs Section of main Project Page
        cy.get('ul').eq(0).find('li').should('have.length',4)

        // Check Project details is default tab selected
        cy.get('a[aria-Current="page"]').contains('Project details')


    }
}

