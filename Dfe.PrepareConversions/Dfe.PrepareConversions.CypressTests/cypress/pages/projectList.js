/// <reference types ='Cypress'/>

export const path = 'project-list';

import BasePage from './BasePage'

export default new class projectList extends BasePage {

    checkProjectListPage() {
        cy.url().should('include', path);
    }

    getNthProject(n = 0){
        this.checkProjectListPage();
        return cy.get(`[id="school-name-${n}"]`);
    }

    getNthProjectDeliveryOfficer(n = 0){
        this.checkProjectListPage();
        return cy.get(`[id="delivery-officer-${n}"]`);
    }

    filterProjectList( titleFilter ) {
        const filterQuery = `?Title=${encodeURIComponent(titleFilter)}`;
        cy.visit(`${Cypress.env('url')}/${path}${filterQuery}`)
    };

    selectFirstItem(){
        this.checkProjectListPage();
        this.getNthProject().click();
    }

    selectProject(projectName = 'Gloucester School') {
        this.filterProjectList(projectName);
        this.selectFirstItem();
        return cy.url().then(url => this.getIdFromUrl(url));
    };

    selectVoluntaryProject() {
        cy.login({titleFilter: 'Voluntary Cypress Project'});
        cy.get('[id="school-name-0"]').click();

        return cy.url().then(url => this.getIdFromUrl(url));
    };

    getIdFromUrl(url) {
        const urlSplit = url.toString().split('/');
        for (let i = urlSplit.length - 1; i > 0; i--) {
            const potentialId = parseInt(urlSplit[i]);

            if (!isNaN(potentialId)) return potentialId;
        }

        return '';
    };
};