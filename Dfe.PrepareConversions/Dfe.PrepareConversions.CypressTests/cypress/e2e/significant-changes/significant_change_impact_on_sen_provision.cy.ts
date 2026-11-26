/// <reference types="cypress" />
import significantChangeImpactOnSenProvision from '../../pages/significantChangeImpactOnSenProvision';
import { Logger } from '../../support/logger';
import { invalidSharePointLinks, verifySharePointLinkValidation } from 'cypress/support/sharePointLinkValidation';

const taskTitle = 'Impact on SEN provision';

describe('Significant change - impact on SEN provision', () => {
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

    it('Should list the task first in the SEN and alternative provision considerations section', () => {
        openTaskList(() => {
            cy.contains('h3', 'SEN and alternative provision considerations')
                .next('.app-task-list')
                .find('a')
                .first()
                .should('contain.text', taskTitle);
        });
    });

    it('Should show all fields', () => {
        openTaskList(() => {
            significantChangeImpactOnSenProvision.openTask(taskTitle);

            significantChangeImpactOnSenProvision
                .verifyPageLoaded()
                .verifyAllOptionsPresent(['answer-yes', 'answer-no', 'not-applicable'])
                .verifySupportingEvidencePresent();
        });
    });

    it('Should require an answer to be selected', () => {
        openTaskList(() => {
            significantChangeImpactOnSenProvision.openTask(taskTitle);

            cy.get('input[name="Answer"]').then(($radios) => {

                if ($radios.filter(':checked').length > 0) {
                    Logger.log('Task already answered - skipping empty answer validation');
                    return;
                }

                const error =
                    'Select whether the proposal is likely to improve the standard, quality and range of SEN provision';
                significantChangeImpactOnSenProvision
                    .clickSaveButton()
                    .verifyErrorSummaryContains(error)
                    .verifyInlineError('Answer', error);
            });
        });
    });

    it('Should only show Give details when No is selected', () => {
        openTaskList(() => {
            significantChangeImpactOnSenProvision.openTask(taskTitle);

            significantChangeImpactOnSenProvision.selectYes().verifyAdditionalInformationVisible(false);
            significantChangeImpactOnSenProvision.selectNo().verifyAdditionalInformationVisible(true);
            cy.contains('label', 'Give details').should('be.visible');
        });
    });

    it('Should require details when No is selected', () => {
        openTaskList(() => {
            significantChangeImpactOnSenProvision.openTask(taskTitle);

            significantChangeImpactOnSenProvision
                .selectNo()
                .enterAdditionalInformation(' ')
                .clickSaveButton()
                .verifyErrorSummaryContains('Enter details')
                .verifyInlineError('AdditionalInformation', 'Enter details');

            significantChangeImpactOnSenProvision
                .selectNo()
                .enterAdditionalInformation('The proposal reduces the range of SEN provision')
                .enterSupportingEvidence('https://educationgovuk.sharepoint.com/sites/sen/evidence')
                .clickSaveButton();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            cy.contains('a', taskTitle).should('be.visible');
        });
    });

    it('Should save Yes without requiring details', () => {
        openTaskList(() => {
            significantChangeImpactOnSenProvision.openTask(taskTitle);

            significantChangeImpactOnSenProvision.selectYes().clickSaveButton();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            cy.contains('a', taskTitle).should('be.visible');
        });
    });

    invalidSharePointLinks.forEach(({ name, value, error }) => {
        it(`rejects ${name} as supporting evidence`, () => {
            openTaskList(() => {
                significantChangeImpactOnSenProvision.openTask(taskTitle);
                significantChangeImpactOnSenProvision.selectYes();
                verifySharePointLinkValidation('SupportingEvidence', value, error);
            });
        });
    });
});
