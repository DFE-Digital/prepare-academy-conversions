/// <reference types="cypress" />
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import significantChangeConsultationDuration from '../../pages/significantChangeConsultationDuration';
import { Logger } from '../../support/logger';

describe('Significant change - consultation', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    const openTaskListAndContinue = (then: () => void) => {
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

    it('Should show all three options and hide the comment box until No is selected', () => {
        openTaskListAndContinue(() => {
            significantChangeTaskList.openConsultationDurationTask();

            significantChangeConsultationDuration
                .verifyPageLoaded()
                .verifyAllOptionsPresent()
                .verifyReasonHidden()
                .selectYes()
                .verifyReasonHidden()
                .selectNoSatisfactory()
                .verifyReasonHidden()
                .selectNo()
                .verifyReasonVisible();
        });
    });

    it('Should require a reason when No is selected, then save and return to the task list', () => {
        openTaskListAndContinue(() => {
            significantChangeTaskList.openConsultationDurationTask();

            significantChangeConsultationDuration.selectNo().save();

            significantChangeConsultationDuration
                .verifyErrorSummaryContains('Add a reason')
                .verifyInlineError('ConsultationDurationNotMetReason', 'Add a reason');

            significantChangeConsultationDuration.selectNo().enterReason('Consultation ran for two weeks only').save();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            significantChangeTaskList.verifyConsultationDurationTaskVisible();
        });
    });

    it('Should save the satisfactory consultation answer without a comment', () => {
        openTaskListAndContinue(() => {
            significantChangeTaskList.openConsultationDurationTask();

            significantChangeConsultationDuration.selectNoSatisfactory().save();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            significantChangeTaskList.verifyConsultationDurationTaskVisible();
        });
    });
});
