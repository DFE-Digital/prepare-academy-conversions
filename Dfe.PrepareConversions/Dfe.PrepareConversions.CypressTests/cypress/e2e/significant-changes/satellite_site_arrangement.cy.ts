/// <reference types="cypress" />
import satelliteSiteArrangement from '../../pages/significantChangeSatelliteSiteArrangement';
import { Logger } from '../../support/logger';

describe('Significant change - satellite site arrangement', () => {
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

    it('Should show the task and require additional information when No is selected', () => {
        openTaskList(() => {
            cy.contains('h3.app-task-list__section', 'SEN and alternative provision considerations')
                .next('.app-task-list')
                .within(() => {
                    cy.get('a').then(($links) => {
                        const titles = [...$links].map((link) => link.textContent?.trim());
                        expect(titles.indexOf('Satellite site arrangement')).to.be.lessThan(
                            titles.indexOf('Special school satellite site considerations')
                        );
                    });
                    cy.contains('a', 'Satellite site arrangement').click();
                });

            satelliteSiteArrangement
                .verifyPageLoaded()
                .selectAnswer('yes')
                .verifyAdditionalInformationVisible(false)
                .selectAnswer('no')
                .verifyAdditionalInformationVisible(true)
                .selectAnswer('not-applicable')
                .verifyAdditionalInformationVisible(false)
                .selectAnswer('no');

            cy.getByDataTest('additional-information').clear();
            satelliteSiteArrangement.save();
            cy.get('.govuk-error-summary')
                .should('be.visible')
                .and('contain.text', 'Enter details');
            cy.getById('AdditionalInformation-error').should('contain.text', 'Enter details');

            satelliteSiteArrangement
                .enterAdditionalInformation('Further investigation of the proposed expansion is required')
                .enterSupportingEvidence('https://sharepoint.edu.gov.uk/evidence')
                .save();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            cy.contains('a', 'Satellite site arrangement').should('be.visible').click();

            satelliteSiteArrangement.verifyPageLoaded();
            cy.getById('answer-no').should('be.checked');
            cy.getByDataTest('additional-information').should(
                'have.value',
                'Further investigation of the proposed expansion is required'
            );
            cy.getByDataTest('supporting-evidence').should('have.value', 'https://sharepoint.edu.gov.uk/evidence');
        });
    });
});
