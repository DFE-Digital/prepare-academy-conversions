import BasePage from './basePage';

class HomePage extends BasePage {
    public path = 'home';

    public startCreateNewTransfer(): this {
        cy.getByDataTest('create-transfer').click();
        return this;
    }

    public projectsCountShouldBeVisible(): this {
        cy.getByDataCy('select-projectlist-filter-count').should('be.visible');
        cy.getByDataCy('select-projectlist-filter-count').should('contain.text', 'projects found');
        return this;
    }

    public getProjectsCount(): Number {
        let projectsCount = 0;

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((txt) => {
                projectsCount = Number(txt.split(' ')[0]);
            });

        return projectsCount;
    }

    public toggleFilterProjects(isOpen): this {
        cy.getByDataCy('select-projectlist-filter-expand').click();
        if (isOpen) cy.get('details').should('have.attr', 'open');
        else cy.get('details').should('not.have.attr', 'open');
        return this;
    }

    public filterProjects(projectTitle): this {
        cy.getById('Title').type(projectTitle);

        cy.getByDataCy('select-projectlist-filter-apply').first().click();

        cy.getByDataModule('govuk-notification-banner').should('be.visible');
        cy.getByDataModule('govuk-notification-banner').should('contain.text', 'Success');
        cy.getByDataModule('govuk-notification-banner').should('contain.text', 'Projects filtered');

        cy.getByDataCy('select-projectlist-filter-count').should('be.visible');
        cy.getByDataCy('select-projectlist-filter-count').should('contain.text', 'projects found');

        cy.get('tbody > tr').should('have.length.at.least', 1);

        return this;
    }

    public clearFilters(): this {
        cy.getByDataCy('clear-filter').click();
        return this;
    }

    public selectFirstProject(): this {
        cy.get('[data-id*="project-link"]').first().click();
        return this;
    }
}

const homePage = new HomePage();

export default homePage;
