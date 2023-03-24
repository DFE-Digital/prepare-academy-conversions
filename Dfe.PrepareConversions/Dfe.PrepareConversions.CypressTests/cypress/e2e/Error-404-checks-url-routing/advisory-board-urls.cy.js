/// <reference types ='Cypress'/>

import ProjectList from '../../pages/projectList'

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`86339 validate advisory board urls on ${viewport}`, () => {
        beforeEach(() => {
            ProjectList.selectProject().then(id => {
                cy.sqlServer(`update
                                academisation.Project set
                                LocalAuthorityInformationTemplateReturnedDate = '2023-01-01',
                                LocalAuthorityInformationTemplateSentDate = '2023-01-01',
                                HeadTeacherBoardDate = '2024-01-01'
                            where Id = ${id}`);
            });
        });

        after(function () {
            cy.clearLocalStorage()
        });

        it('TC01: should correctly render a page when user visits /confirm-school-trust-information-project-dates/advisory-board-date', () => {
            cy.viewport(viewport)
            //Changes the current URL
            cy.urlPath().then(url =>  cy.visit(url + '/confirm-school-trust-information-project-dates/advisory-board-date'));
            cy.get('h1').contains('Set the advisory board date')
        });

        it('TC02: should correctly render a page when user visits /confirm-school-trust-information-project-dates/previous-advisory-board-date', () => {
            cy.viewport(viewport)
            cy.urlPath().then(url =>  cy.visit(url + '/confirm-school-trust-information-project-dates/previous-advisory-board-date'));
            //Changes the current URL

            cy.get('h1').contains('Date of previous advisory board')
        });

        it('TC03: should correctly render a page when user visits /confirm-school-trust-information-project-dates/previous-advisory-board', () => {
            cy.viewport(viewport)
            //Changes the current URL
            cy.urlPath().then(url =>  cy.visit(url + '/confirm-school-trust-information-project-dates/previous-advisory-board'));
            cy.get('h1').contains('Has this project been to a previous advisory board?')
        });

        it('TC04: should correctly render a page when user visits /preview-project-template', () => {
            cy.viewport(viewport)
            //Changes the current URL
            cy.urlPath().then(url =>  cy.visit(url +  '/preview-project-template'));
            cy.get('h1').contains('Preview project template')
        });

        // download-project-template feature is currently removed
        it('TC05: should correctly render a page when user visits /download-project-template', () => {
            cy.viewport(viewport)
            //Changes the current URL
            cy.urlPath().then(url =>  cy.visit(url +  '/download-project-template'));
            cy.get('h1').contains('Download project template')
        });

        it('TC06: Should generate correct error message if and when text  is typed in date field', () => {
            cy.viewport(viewport)
            cy.urlPath().then(url =>  cy.visit(url + '/confirm-local-authority-information-template-dates'));
            cy.get('[data-test="change-la-info-template-sent-date"]').click()
            cy.get('[id="la-info-template-sent-date-day"]').clear().type('a')
            cy.get('[data-module="govuk-button"]').click()
            cy.get('[id="error-summary-title"]').invoke('text').should('contain', 'There is a problem')
        });
    });
});