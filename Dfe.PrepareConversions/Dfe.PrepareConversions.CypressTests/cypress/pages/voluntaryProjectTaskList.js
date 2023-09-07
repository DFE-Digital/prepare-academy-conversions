var projectName = 'Voluntary Cypress Project'
var routeType = 'Voluntary conversion'
var projectOwner = ''
var projectStatus = 'Approved with Conditions'

class voluntaryProjectTaskList {
    
    static voluntaryProjectElementsVisible()
    {
        // Check Header section of main Project Page
        cy.get('h1[data-cy]').contains(projectName)
        cy.get('p[class="govuk-body govuk-!-margin-bottom-1"]').contains('Route: ' + routeType)
        cy.get('p[class="govuk-body govuk-!-margin-bottom-5"]').contains('Project owner: ' + projectOwner)
        cy.get('.empty').contains('Empty')
        cy.get('a[class="govuk-link govuk-!-padding-left-50"]').contains('Change')
        cy.get('#project-status-28470').contains(projectStatus)

        // Check Project Tabs Section of main Project Page
        cy.get('ul[class="moj-sub-navigation__list govuk-!-margin-top-6"]').should('have.length',5)


    }
}

export default new voluntaryProjectTaskList