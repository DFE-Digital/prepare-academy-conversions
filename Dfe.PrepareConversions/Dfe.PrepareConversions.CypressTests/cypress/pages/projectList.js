/// <reference types ='Cypress'/>
class ProjectList {

    checkProjectListPage() {
        cy.url().should('include', 'project-list');
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
        cy.visit(`${Cypress.env('url')}/project-list${filterQuery}`)
    };

    selectFirstItem(){
        this.checkProjectListPage();
        this.getNthProject().click();
    }

    selectProject(projectName = 'Gloucester school') {
        this.filterProjectList(projectName);
        this.selectFirstItem();
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

export default new ProjectList();