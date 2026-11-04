/// <reference types="cypress" />
import significantChangeFhaRisksOrIssuesRaised from '../../pages/significantChangeFhaRisksOrIssuesRaised';
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import { Logger } from '../../support/logger';

const TASK_LIST_URL = /\/significant-change\/task-list\/\d+(\?.*)?$/;
const projectIdFrom = (url: string): string => url.split('?')[0].split('/').pop() as string;

describe('Significant change - FHA risks or issues raised', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    const withProject = (then: (_projectId: string) => void) => {
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

    it('opens from the significant change task list', () => {
        withProject((_projectId) => {
            significantChangeTaskList.openFhaRisksOrIssuesRaisedTask();
            significantChangeFhaRisksOrIssuesRaised.verifyPageLoaded();
        });
    });

    it('shows additional information only when Yes is selected', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/fha-risks-or-issues-raised`);

            significantChangeFhaRisksOrIssuesRaised
                .verifyPageLoaded()
                .verifyAdditionalInformationHidden()
                .selectNo()
                .verifyAdditionalInformationHidden()
                .selectYes()
                .verifyAdditionalInformationVisible();
        });
    });

    it('requires additional information for Yes and saves the response', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/fha-risks-or-issues-raised`);

            significantChangeFhaRisksOrIssuesRaised.selectYes().save();
            significantChangeFhaRisksOrIssuesRaised
                .verifyErrorSummaryContains('Add details of the risks or issues raised')
                .verifyInlineError('AdditionalInformation', 'Add details of the risks or issues raised');

            significantChangeFhaRisksOrIssuesRaised
                .selectYes()
                .enterAdditionalInformation('A deficit is forecast')
                .enterSupportingEvidence('Financial forecast')
                .save();

            cy.url().should('match', TASK_LIST_URL);
            significantChangeTaskList.verifyFhaRisksOrIssuesRaisedTaskVisible();
        });
    });

    it('saves the No answer without additional information', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/fha-risks-or-issues-raised`);

            significantChangeFhaRisksOrIssuesRaised
                .verifyPageLoaded()
                .selectNo()
                .enterSupportingEvidence('No risks or issues identified')
                .save();

            cy.url().should('match', TASK_LIST_URL);
            significantChangeTaskList.verifyFhaRisksOrIssuesRaisedTaskVisible();
        });
    });
});
