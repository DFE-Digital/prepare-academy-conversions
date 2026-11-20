/// <reference types="cypress" />
import { invalidSharePointLinks, verifySharePointLinkValidation } from 'cypress/support/sharePointLinkValidation';
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

    it('Should show all fields', () => {
        openTaskList(() => {
            significantChangeSpecialSchoolSatelliteConsiderations.openTask(
                'Special school satellite site considerations'
            );
            significantChangeSpecialSchoolSatelliteConsiderations
                .verifyPageLoaded()
                .verifySupportingEvidencePresent()
                .verifyAllOptionsPresent(['answer-yes', 'answer-no', 'not-applicable']);
        });
    });

    it('Should show the task and require an answer and additional information when No is selected', () => {
        openTaskList(() => {
            significantChangeSpecialSchoolSatelliteConsiderations.openTask(
                'Special school satellite site considerations'
            );

            significantChangeSpecialSchoolSatelliteConsiderations
                .selectNo()
                .clickSaveButton()
                .verifyErrorSummaryContains('Enter details')
                .verifyInlineError('AdditionalInformation', 'Enter details');

            significantChangeSpecialSchoolSatelliteConsiderations
                .selectNo()
                .enterAdditionalInformation('Satellite site requirements are being addressed')
                .enterSupportingEvidence('https://educationgovuk.sharepoint.com/sites/satellite/evidence')
                .clickSaveButton();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            cy.contains('a', 'Special school satellite site considerations').should('be.visible');
        });
    });

    invalidSharePointLinks.forEach(({ name, value, error }) => {
        it(`rejects ${name} as supporting evidence`, () => {
            openTaskList(() => {
                significantChangeSpecialSchoolSatelliteConsiderations.openTask(
                    'Special school satellite site considerations'
                );
                significantChangeSpecialSchoolSatelliteConsiderations.selectYes();
                verifySharePointLinkValidation('SupportingEvidence', value, error);
            });
        });
    });
});
