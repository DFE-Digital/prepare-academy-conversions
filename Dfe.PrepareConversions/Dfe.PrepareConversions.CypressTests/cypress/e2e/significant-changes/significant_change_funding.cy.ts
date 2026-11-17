/// <reference types="cypress" />
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import significantChangeFunding from '../../pages/significantChangeFunding';
import { Logger } from '../../support/logger';
import { invalidSharePointLinks, verifySharePointLinkValidation } from '../../support/sharePointLinkValidation';

describe('Significant change - funding', () => {
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

    invalidSharePointLinks.forEach(({ name, value, error }) => {
        it(`rejects ${name} as supporting evidence`, () => {
            openTaskList(() => {
                significantChangeTaskList.openFundingTask();
                verifySharePointLinkValidation('SupportingEvidence', value, error);
            });
        });
    });

    it('Should show all three options and hide the additional information box until No is selected', () => {
        openTaskList(() => {
            significantChangeTaskList.openFundingTask();

            significantChangeFunding
                .verifyPageLoaded()
                .verifyAllOptionsPresent()
                .verifyFundingAdditionalInformationHidden()
                .selectYes()
                .verifyFundingAdditionalInformationHidden()
                .selectNotApplicable()
                .verifyFundingAdditionalInformationHidden()
                .selectNo()
                .verifyFundingAdditionalInformationVisible();
        });
    });

    it('Should require additional information when No is selected, then save and return to the task list', () => {
        openTaskList(() => {
            significantChangeTaskList.openFundingTask();

            significantChangeFunding.selectNo().save();

            significantChangeFunding
                .verifyErrorSummaryContains('Add additional information')
                .verifyInlineError('AdditionalInformation', 'Add additional information');

            significantChangeFunding
                .selectNo()
                .enterAdditionalInformation('Funding gap identified')
                .enterSupportingEvidence('https://educationgovuk.sharepoint.com/sites/funding/board-minutes')
                .save();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            significantChangeTaskList.verifyFundingTaskVisible();
        });
    });

    it('Should save the not applicable answer without additional information', () => {
        openTaskList(() => {
            significantChangeTaskList.openFundingTask();

            significantChangeFunding.selectNotApplicable();
            cy.getByDataTest('funding-supporting-evidence').clear();
            significantChangeFunding.save();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            significantChangeTaskList.verifyFundingTaskVisible();
        });
    });

    it('Should save a personal SharePoint link and retain it when reopening the task', () => {
        openTaskList(() => {
            significantChangeTaskList.openFundingTask();
            const evidence = 'https://educationgovuk-my.sharepoint.com/personal/user/funding-evidence';

            significantChangeFunding.selectYes().enterSupportingEvidence(evidence).save();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            significantChangeTaskList.openFundingTask();
            cy.getByDataTest('funding-supporting-evidence').should('have.value', evidence);
        });
    });
});
