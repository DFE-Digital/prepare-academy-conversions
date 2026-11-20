/// <reference types="cypress" />
import significantChangeLocalSenPlans from '../../pages/significantChangeLocalSenPlans';
import { Logger } from '../../support/logger';
import { invalidSharePointLinks, verifySharePointLinkValidation } from 'cypress/support/sharePointLinkValidation';

describe('Significant change - local SEN plans', () => {
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
            significantChangeLocalSenPlans.openTask('Local SEN plans');

            significantChangeLocalSenPlans
                .verifyPageLoaded()
                .verifyAllOptionsPresent(['answer-yes', 'answer-no', 'not-applicable'])
                .verifySupportingEvidencePresent();
        });
    });

    it('Should show the task and require additional information when Yes is selected', () => {
        openTaskList(() => {
            significantChangeLocalSenPlans.openTask(
                'Local SEN plans'
            );

            significantChangeLocalSenPlans
                .selectYes()
                .enterAdditionalInformation(' ')
                .clickSaveButton()
                .verifyErrorSummaryContains('Enter details')
                .verifyInlineError('AdditionalInformation', 'Enter details');

            significantChangeLocalSenPlans
                .selectYes()
                .enterAdditionalInformation('Some details')
                .enterSupportingEvidence('https://educationgovuk.sharepoint.com/sites/satellite/evidence')
                .clickSaveButton();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            cy.contains('a', 'Local SEN plans').should('be.visible');
        });
    });

    it('Should save when No is selected without details', () => {
        openTaskList(() => {
            significantChangeLocalSenPlans.openTask('Local SEN plans');

            significantChangeLocalSenPlans.selectNo().clickSaveButton();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
        });
    });

    invalidSharePointLinks.forEach(({ name, value, error }) => {
        it(`rejects ${name} as supporting evidence`, () => {
            openTaskList(() => {
                significantChangeLocalSenPlans.openTask('Local SEN plans');
                significantChangeLocalSenPlans.selectNo();
                verifySharePointLinkValidation('SupportingEvidence', value, error);
            });
        });
    });
});
