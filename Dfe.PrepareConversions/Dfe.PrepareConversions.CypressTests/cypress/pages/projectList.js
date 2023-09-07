import BasePage from './BasePage'

export default new class projectList extends BasePage {

    selectProject() {
        cy.login({titleFilter: 'Gloucester school'});
        cy.get('[id="school-name-0"]').click();

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

//export default new projectList();