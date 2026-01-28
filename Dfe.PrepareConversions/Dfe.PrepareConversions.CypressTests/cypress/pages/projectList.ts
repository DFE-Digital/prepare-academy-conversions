/// <reference types="cypress" />
import BasePage from './basePage';

class ProjectList extends BasePage {
    public path = 'project-list';

    public checkProjectListPage(): this {
        cy.url().should('include', this.path);
        return this;
    }

    public getNthProject(n = 1): Cypress.Chainable<JQuery<HTMLElement>> {
        this.checkProjectListPage();
        return cy.getById(`school-name-${n}`);
    }

    public getNthProjectDeliveryOfficer(n = 1): Cypress.Chainable<JQuery<HTMLElement>> {
        this.checkProjectListPage();
        return cy.getById(`assigned-to-${n}`);
    }

    public filterProjectList(titleFilter: string): this {
        const filterQuery = `?Title=${encodeURIComponent(titleFilter)}`;
        cy.visit(`/${this.path}${filterQuery}`);
        return this;
    }

    public filterByRegion(region: string): this {
        this.filterProjectList(region);
        return this;
    }

    public filterByStatus(status: string): this {
        this.filterProjectList(status);
        return this;
    }

    public filterByAdvisoryBoardDate(advisoryBoardDate: string): this {
        this.filterProjectList(advisoryBoardDate);
        return this;
    }

    public filterByTitle(title: string): this {
        this.filterProjectList(title);
        return this;
    }

    public selectFirstItem(): this {
        this.checkProjectListPage();
        this.getNthProject().click();
        return this;
    }

    public selectProject(projectName = 'Gloucester school'): Cypress.Chainable<number | string> {
        this.filterProjectList(projectName);
        this.selectFirstItem();
        return cy.url().then((url) => this.getIdFromUrl(url));
    }

    public filterProject(projectName = 'Gloucester school'): this {
        this.filterProjectList(projectName);
        return this;
    }

    public selectVoluntaryProject(): Cypress.Chainable<number | string> {
        cy.login({ titleFilter: 'Voluntary Cypress Project' });
        cy.getById('school-name-0').click();

        return cy.url().then((url) => this.getIdFromUrl(url));
    }

    public getIdFromUrl(url: string): number | string {
        const urlSplit = url.toString().split('/');
        for (let i = urlSplit.length - 1; i > 0; i--) {
            const potentialId = parseInt(urlSplit[i]);

            if (!isNaN(potentialId)) return potentialId;
        }

        return '';
    }
}

const projectList = new ProjectList();

export default projectList;
