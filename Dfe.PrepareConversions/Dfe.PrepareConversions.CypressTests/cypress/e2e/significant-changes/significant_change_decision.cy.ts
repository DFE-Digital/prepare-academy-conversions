/// <reference types="cypress" />
import significantChangeDecision from '../../pages/significantChangeDecision';
import { Logger } from '../../support/logger';

const TASK_LIST_URL = /\/significant-change\/task-list\/\d+(\?.*)?$/;
const projectIdFrom = (url: string): string => url.split('?')[0].split('/').pop() as string;

describe('Significant change record a decision', () => {
    let projectDatesToRestore:
        | {
              projectId: string;
              decisionDay: string;
              decisionMonth: string;
              decisionYear: string;
              changeDay: string;
              changeMonth: string;
              changeYear: string;
          }
        | undefined;

    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    afterEach(() => {
        if (!projectDatesToRestore) {
            return;
        }

        const dates = projectDatesToRestore;
        const restoreInput = (id: string, value: string) => {
            cy.getById(id).clear();
            if (value) {
                cy.getById(id).type(value);
            }
        };
        cy.visit(`/significant-change/task-list/${dates.projectId}/confirm-project-dates`);
        restoreInput('proposed-decision-date-day', dates.decisionDay);
        restoreInput('proposed-decision-date-month', dates.decisionMonth);
        restoreInput('proposed-decision-date-year', dates.decisionYear);
        restoreInput('proposed-change-date-day', dates.changeDay);
        restoreInput('proposed-change-date-month', dates.changeMonth);
        restoreInput('proposed-change-date-year', dates.changeYear);
        cy.getByDataCy('select-common-submitbutton').click();
        cy.location('pathname').should('eq', `/significant-change/task-list/${dates.projectId}`);
        cy.then(() => {
            projectDatesToRestore = undefined;
        });
    });

    const withAProject = (then: (_projectId: string) => void) => {
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
                cy.url().should('match', TASK_LIST_URL);
                cy.url().then((taskListUrl) => then(projectIdFrom(taskListUrl)));
            });
    };

    it('Should show a Record a decision tab on the project details page', () => {
        withAProject((_projectId) => {
            cy.getByDataCy('significant_change_record_decision_menu')
                .should('be.visible')
                .and('contain.text', 'Record a decision');
        });
    });

    it('Should navigate to the wizard from the tab', () => {
        withAProject(() => {
            cy.getByDataCy('significant_change_record_decision_menu').click();
            significantChangeDecision.verifyOnStep('record-decision');
            significantChangeDecision.verifyHeading('Record the decision');
        });
    });

    it('Should complete an approved decision with conditions', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('approved')
                .submit()
                .verifyOnStep('any-conditions')
                .setConditions(true, 'Trust must appoint a new chair')
                .submit()
                .verifyOnStep('who-decided')
                .selectDecisionMaker('regionaldirectorforregion')
                .submit()
                .verifyOnStep('decision-maker')
                .enterDecisionMakerName('Jane Smith')
                .submit()
                .verifyOnStep('decision-date')
                .enterDecisionDate('27', '3', '2026')
                .submit()
                .verifyOnStep('summary');

            significantChangeDecision
                .verifySummaryRow('decision', 'Approved with conditions')
                .verifySummaryRow('condition-set', 'Trust must appoint a new chair')
                .verifySummaryRow('decision-made-by', 'Regional Director for the region')
                .verifySummaryRow('decision-maker-name', 'Jane Smith')
                .verifySummaryRow('decision-date', '27 March 2026');
        });
    });

    it('Should return to the decision branch when changing the decision', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('approved')
                .submit()
                .setConditions(true, 'Trust must appoint a new chair')
                .submit()
                .selectDecisionMaker('regionaldirectorforregion')
                .submit()
                .enterDecisionMakerName('Jane Smith')
                .submit()
                .enterDecisionDate('27', '3', '2026')
                .submit()
                .verifyOnStep('summary');

            cy.getById('change-decision-btn').click();
            significantChangeDecision
                .verifyOnStep('record-decision')
                .selectDecision('declined')
                .submit()
                .verifyOnStep('declined-reason');
        });
    });

    it('Should require a reason when declining', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('declined')
                .submit()
                .verifyOnStep('declined-reason')
                .submit()
                .verifyErrorSummaryContains('Select at least one reason');
        });
    });

    it('Should return to Record a decision after completing an incomplete task from its error link', () => {
        withAProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/confirm-project-dates`);

            const originalDates = {
                decisionDay: '',
                decisionMonth: '',
                decisionYear: '',
                changeDay: '',
                changeMonth: '',
                changeYear: '',
            };

            cy.getById('proposed-decision-date-day')
                .invoke('val')
                .then((value) => (originalDates.decisionDay = String(value ?? '')));
            cy.getById('proposed-decision-date-month')
                .invoke('val')
                .then((value) => (originalDates.decisionMonth = String(value ?? '')));
            cy.getById('proposed-decision-date-year')
                .invoke('val')
                .then((value) => (originalDates.decisionYear = String(value ?? '')));
            cy.getById('proposed-change-date-day')
                .invoke('val')
                .then((value) => (originalDates.changeDay = String(value ?? '')));
            cy.getById('proposed-change-date-month')
                .invoke('val')
                .then((value) => (originalDates.changeMonth = String(value ?? '')));
            cy.getById('proposed-change-date-year')
                .invoke('val')
                .then((value) => (originalDates.changeYear = String(value ?? '')));

            cy.then(() => {
                projectDatesToRestore = { projectId, ...originalDates };
            });

            cy.getById('proposed-decision-date-day').clear();
            cy.getById('proposed-decision-date-month').clear();
            cy.getById('proposed-decision-date-year').clear();
            cy.getById('proposed-change-date-day').clear();
            cy.getById('proposed-change-date-month').clear();
            cy.getById('proposed-change-date-year').clear();
            cy.getByDataCy('select-common-submitbutton').click();
            cy.location('pathname').should('eq', `/significant-change/task-list/${projectId}`);

            significantChangeDecision
                .openFor(projectId)
                .selectDecision('approved')
                .submit()
                .verifyOnStep('record-decision');

            cy.getByDataCy('error-summary')
                .contains('a', "The task 'Confirm project dates' must be completed")
                .should(
                    'have.attr',
                    'href',
                    `/significant-change/task-list/${projectId}/confirm-project-dates?return=%2FSignificantChange%2FDecision%2FRecordDecision`
                )
                .click();
            cy.location('pathname').should('eq', `/significant-change/task-list/${projectId}/confirm-project-dates`);

            cy.getById('proposed-decision-date-day').type('21');
            cy.getByDataCy('select-common-submitbutton').click();
            cy.getByDataCy('error-summary').should('contain.text', 'must include a month and year');
            cy.location('pathname').should('eq', `/significant-change/task-list/${projectId}/confirm-project-dates`);
            cy.location('search').should('eq', '?return=%2FSignificantChange%2FDecision%2FRecordDecision');

            cy.getById('proposed-decision-date-month').type('11');
            cy.getById('proposed-decision-date-year').type('2026');
            cy.getById('proposed-change-date-day').type('22');
            cy.getById('proposed-change-date-month').type('12');
            cy.getById('proposed-change-date-year').type('2026');
            cy.getByDataCy('select-common-submitbutton').click();

            significantChangeDecision.verifyOnStep('record-decision').verifyHeading('Record the decision');
            significantChangeDecision.selectDecision('approved').submit();
            cy.get('body').should('not.contain.text', "The task 'Confirm project dates' must be completed");
            cy.visit(`/significant-change/task-list/${projectId}`);
            cy.getById('task-status-confirm-project-dates').should('contain.text', 'Completed');
        });
    });

    it('Should complete a declined decision with multiple reasons', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('declined')
                .submit()
                .checkReason('governance', 'Weak governance')
                .checkReason('choiceoftrust', 'Trust not suitable')
                .submit()
                .verifyOnStep('who-decided')
                .selectDecisionMaker('minister')
                .submit()
                .enterDecisionMakerName('Jane Smith')
                .submit()
                .enterDecisionDate('27', '3', '2026')
                .submit()
                .verifyOnStep('summary')
                .verifySummaryRow('decision', 'Declined')
                .verifySummaryRow('reasons', 'Weak governance');
        });
    });

    it('Should skip the decision maker name when nobody made the decision', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('declined')
                .submit()
                .checkReason('performanceconcerns', 'Results declining')
                .submit()
                .verifyOnStep('who-decided')
                .selectDecisionMaker('none')
                .submit()
                .verifyOnStep('decision-date');
        });
    });

    it('Should reject a decision date in the future', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('withdrawn')
                .submit()
                .checkReason('other', 'Trust withdrew')
                .submit()
                .selectDecisionMaker('grade6')
                .submit()
                .enterDecisionMakerName('Jane Smith')
                .submit()
                .enterDecisionDate('1', '1', '2099')
                .submit()
                .verifyOnStep('decision-date');

            cy.getByDataCy('error-summary').should('be.visible');
        });
    });
});
