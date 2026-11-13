/// <reference types="cypress" />
import significantChangeSpecialSchoolSatelliteConsiderations from '../../pages/significantChangeSpecialSchoolSatelliteConsiderations';
import { Logger } from '../../support/logger';

describe('Significant change - special school satellite considerations', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    const openTaskList = (then: () => void) => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count === 0) {
                    Logger.log('No significant change projects available - skipping');
                    cy.contains('There are no matching results.').should('be.visible');
                    return;
                }

                cy.getById('school-name-0').click();
                cy.url().should('match', /\/significant-change\/task-list\/\d+(\?.*)?$/);
                then();
            });
    };

    it('Should show the task and require an answer and additional information when No is selected', () => {
        openTaskList(() => {
            cy.contains('h3.app-task-list__section', 'SEN and alternative provision considerations')
                .next('.app-task-list')
                .contains('a', 'Special school satellite site considerations')
                .should('be.visible')
                .click();

            significantChangeSpecialSchoolSatelliteConsiderations
                .selectNo()
                .save()
                .verifyErrorSummaryContains('Add additional information')
                .verifyInlineError('AdditionalInformation', 'Add additional information');

            significantChangeSpecialSchoolSatelliteConsiderations
                .selectNo()
                .enterAdditionalInformation('Satellite site requirements are being addressed')
                .enterSupportingEvidence('https://example.test/satellite-site-review')
                .save();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            cy.contains('a', 'Special school satellite site considerations').should('be.visible');
        });
    });
});
