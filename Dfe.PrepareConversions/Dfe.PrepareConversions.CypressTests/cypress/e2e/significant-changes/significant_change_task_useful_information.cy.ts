/// <reference types="cypress" />

describe('Significant Useful Information', () => {
    const navigateToTaskPage = () => {
        cy.login();
        cy.acceptCookies();
        cy.visit('/significant-change/project-list');
        cy.getById('school-name-0').click();
        cy.get('.app-task-list').within(() => {
            cy.get('a').first().click();
        });
    };

    beforeEach(() => {
        navigateToTaskPage();
    });

    it('should render useful information', () => {
        cy.getByDataTest('useful-information').should('exist');

        cy.get('h2').should('contain.text', 'Useful information');
    });

    it('should contain the application form link', () => {
        cy.getByDataTest('useful-information').within(() => {
            cy.get('a').should('contain.text', 'Application form');
        });
    });

    it('should contain the guidance and documents link', () => {
        cy.getByDataTest('useful-information').within(() => {
            cy.get('a').should('contain.text', 'Guidance and documents');
        });
    });

    it('should open the application form link in a new tab', () => {
        cy.contains('a', 'Application form').should('have.attr', 'target', 'significantChangeApplication');
        cy.contains('a', 'Application form')
            .should('have.attr', 'href')
            .and('include', '/significant-change/application/');

        cy.contains('a', 'Application form').invoke('removeAttr', 'target').click();
        cy.url().should('include', '/significant-change/application');
    });

    it('should open the guidance and documents link in a new tab', () => {
        cy.contains('a', 'Guidance and documents').should('have.attr', 'target', '_blank');
        cy.contains('a', 'Guidance and documents').should(
            'have.attr',
            'href',
            'https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/Our-guidance-and-documents.aspx'
        );
    });
});
