/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeTaskList extends BasePage {
    public path = 'significant-change/task-list';

    public verifyHeaderAndSubNavigation(): this {
        cy.checkPath(this.path);

        cy.getByDataCy('select-heading').should('be.visible');
        cy.get('.govuk-caption-xl').should('contain.text', 'URN:');
        cy.contains('p', 'Route:').should('be.visible');
        cy.contains('p', 'Project owner:').should('be.visible');

        cy.get('strong[id^="project-status-"]')
            .should('be.visible')
            .invoke('attr', 'class')
            .should('match', /govuk-tag--(green|yellow|orange|red|purple)/);

        cy.contains('a.moj-sub-navigation__link', 'Project details')
            .should('be.visible')
            .and('have.attr', 'aria-current', 'page');

        return this;
    }

    public verifyTaskListContentLayout(): this {
        cy.contains('h2', 'Change tasks')
            .should('be.visible')
            .parent()
            .should('have.class', 'govuk-grid-column-full')
            .and('not.have.class', 'govuk-grid-column-two-thirds');

        cy.get('.app-task-list').should('be.visible');

        return this;
    }
}

const significantChangeTaskList = new SignificantChangeTaskList();

export default significantChangeTaskList;
