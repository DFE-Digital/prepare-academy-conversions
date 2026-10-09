/// <reference types="cypress" />

describe('Significant application page', () => {
    const navigateToApplicationPage = () => {
    cy.login();
    cy.acceptCookies();
    cy.visit('/significant-change/project-list');
    cy.getById('school-name-0').click();
    cy.contains('a', 'Application form').click();
    };

    beforeEach(() => {
        navigateToApplicationPage();
    });

    it('should render the application page', () => {
        cy.url().should('include', '/significant-change/application');

        cy.getByDataTest('application-form-page').should('exist').within(() => {
            cy.getByDataTest('significant-change-project-header').should('exist');
            cy.getByDataTest('significant-change-submenu').should('exist');
        });
    });

    it('should higlight the appliaction form link in the submenu', () => {
        cy.getByDataTest('significant-change-submenu').within(() => {
            cy.get('.moj-sub-navigation__link--active').should('contain.text', 'Application form');
        });
    });

    it('should contain the Application form heading', () => {
        cy.getByDataTest('application-form-page').within(() => {
            cy.get('h2').should('contain.text', 'Application form');
        });
    });
});
