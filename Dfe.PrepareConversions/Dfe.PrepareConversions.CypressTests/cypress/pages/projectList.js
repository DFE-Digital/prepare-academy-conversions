/// <reference types ='Cypress'/>

export const path = 'project-list';

import BasePage from './BasePage'

export default new class projectList extends BasePage {

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

//export default new projectList();