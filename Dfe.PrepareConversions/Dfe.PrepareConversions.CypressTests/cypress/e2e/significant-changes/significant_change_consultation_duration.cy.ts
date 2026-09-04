/// <reference types="cypress" />
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import significantChangeConsultationDuration from '../../pages/significantChangeConsultationDuration';
import { Logger } from '../../support/logger';

describe('Significant change - consultation duration', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    // The task is only revealed once stakeholders have been consulted, so every test
    // has to establish that precondition through the UI first.
    const openTaskListAndConsultStakeholders = (then: () => void) => {
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

                significantChangeTaskList.openStakeholderConsultationTask();
                cy.getByDataTest('trust-consulted-stakeholders-yes').check();
                cy.getById('save-and-continue-button').click();

                then();
            });
    };

    it('Should reveal the consultation duration task only once stakeholders have been consulted', () => {
        openTaskListAndConsultStakeholders(() => {
            significantChangeTaskList.verifyConsultationDurationTaskVisible();

            significantChangeTaskList.openStakeholderConsultationTask();
            cy.getByDataTest('trust-consulted-stakeholders-no').check();
            cy.getByDataTest('trust-consulted-stakeholders-not-consulted-reason')
                .clear()
                .type('Consultation has not started yet');
            cy.getById('save-and-continue-button').click();

            significantChangeTaskList.verifyConsultationDurationTaskHidden();
        });
    });

    it('Should show all three options and hide the comment box until No is selected', () => {
        openTaskListAndConsultStakeholders(() => {
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

    it('Should show a validation error when no option is selected', () => {
        openTaskListAndConsultStakeholders(() => {
            significantChangeTaskList.openConsultationDurationTask();

            significantChangeConsultationDuration.save();

            significantChangeConsultationDuration
                .verifyPageLoaded()
                .verifyErrorSummaryContains('Select an option')
                .verifyInlineError('ConsultationLastedMinimumThreeWeeks', 'Select an option');
        });
    });

    it('Should require a reason when No is selected, then save and return to the task list', () => {
        openTaskListAndConsultStakeholders(() => {
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
        openTaskListAndConsultStakeholders(() => {
            significantChangeTaskList.openConsultationDurationTask();

            significantChangeConsultationDuration.selectNoSatisfactory().save();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            significantChangeTaskList.verifyConsultationDurationTaskVisible();
        });
    });
});
