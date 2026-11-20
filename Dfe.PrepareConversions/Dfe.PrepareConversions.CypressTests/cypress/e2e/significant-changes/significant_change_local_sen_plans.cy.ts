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

    it('Should list Local SEN plans first in the SEN and alternative provision considerations section', () => {
        openTaskList(() => {
            cy.contains('h3.app-task-list__section', 'SEN and alternative provision considerations')
                .next('.app-task-list')
                .find('.app-task-list__task-name a')
                .first()
                .invoke('text')
                .should((text) => expect(text.trim()).to.eq('Local SEN plans'));
        });
    });

    it('Should show all fields', () => {
        openTaskList(() => {
            significantChangeLocalSenPlans.openTask('Local SEN plans');

            significantChangeLocalSenPlans
                .verifyPageLoaded()
                .verifyAllOptionsPresent(['answer-yes', 'answer-no', 'not-applicable'])
                .verifySupportingEvidencePresent();
        });
    });

    it('Should require an answer', () => {
        openTaskList(() => {
            significantChangeLocalSenPlans.openTask('Local SEN plans');

            cy.get('input[name="Answer"]').should('have.length', 3);
            cy.get('input[name="Answer"]:checked').then(($checked) => {
                if ($checked.length > 0) {
                    Logger.log('Answer already saved on this project - skipping no-answer validation');
                    return;
                }

                const message =
                    'Select whether the proposal conflicts with a Local SEN Reform Plan (or equivalent) for SEN, AP and high needs provision';
                significantChangeLocalSenPlans
                    .clickSaveButton()
                    .verifyErrorSummaryContains(message)
                    .verifyInlineError('Answer', message);
            });
        });
    });

    it('Should only show Give details when Yes is selected', () => {
        openTaskList(() => {
            significantChangeLocalSenPlans.openTask('Local SEN plans');

            significantChangeLocalSenPlans.selectNo().verifyAdditionalInformationVisible(false);
            significantChangeLocalSenPlans.selectYes().verifyAdditionalInformationVisible(true);
            cy.get('label[for="AdditionalInformation"]').should('have.text', 'Give details');
        });
    });

    it('Should require details when Yes is selected and save once provided', () => {
        openTaskList(() => {
            significantChangeLocalSenPlans.openTask('Local SEN plans');

            significantChangeLocalSenPlans
                .selectYes()
                .enterAdditionalInformation(' ')
                .clickSaveButton()
                .verifyErrorSummaryContains('Enter details')
                .verifyInlineError('AdditionalInformation', 'Enter details');

            significantChangeLocalSenPlans
                .selectYes()
                .enterAdditionalInformation('The proposal conflicts with the local SEN reform plan')
                .enterSupportingEvidence('https://educationgovuk.sharepoint.com/sites/sen/evidence')
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
