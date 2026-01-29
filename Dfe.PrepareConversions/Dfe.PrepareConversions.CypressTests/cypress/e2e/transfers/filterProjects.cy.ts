import homePage from 'cypress/pages/home';
import { Logger } from '../../support/logger';

describe('Filter projects', () => {
    const projectTitle = 'Burnt Ash Primary School';

    beforeEach(() => {
        Logger.log('Visit the transfers homepage before each test');
        cy.visit('/transfers/home');
        cy.acceptCookies();
    });

    it('Filters the list of projects', () => {
        const baseCount = homePage.getProjectsCount();
        homePage.projectsCountShouldBeVisible().filterProjects(projectTitle);
        const filterCount = homePage.getProjectsCount();
        expect(filterCount < baseCount);
    });

    it('Clears filters', () => {
        const baseCount = homePage.getProjectsCount();
        homePage.projectsCountShouldBeVisible().filterProjects(projectTitle).clearFilters();
        const afterClearCount = homePage.getProjectsCount();
        expect(afterClearCount).to.equal(baseCount);
    });

    it('Check accessibility across pages', () => {
        cy.checkAccessibilityAcrossPages();
    });
});
