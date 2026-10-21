/// <reference types="cypress" />
import significantChangeOfsted from '../../pages/significantChangeOfsted';
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import { Logger } from '../../support/logger';

const TASK_LIST_URL = /\/significant-change\/task-list\/\d+(\?.*)?$/;
const projectIdFrom = (url: string): string => url.split('?')[0].split('/').pop() as string;

describe('Significant change - Ofsted inspection', () => {
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

    const setInspectionDate = (projectId: string) => {
        cy.visit(`/significant-change/task-list/${projectId}/ofsted`);
        significantChangeOfsted.enterInspectionDate('15', '9', '2025').continueToQuestion();
    };

    it('Should open Ofsted from the task list and continue from the inspection date', () => {
        withProject((_projectId) => {
            significantChangeTaskList.openOfstedTask();
            cy.checkPath('ofsted');

            significantChangeOfsted.enterInspectionDate('15', '9', '2025').continueToQuestion();

            significantChangeOfsted.verifyQuestionPage(
                '15 September 2025',
                'Is the academy rated good or outstanding in leadership and management and quality of education?'
            );
        });
    });

    it('Should require a standards answer before saving', () => {
        withProject((projectId) => {
            setInspectionDate(projectId);
            significantChangeOfsted.saveQuestion().verifyErrorSummaryContains('Select an option');
        });
    });

    it('Should show additional information for No and save the inspection details', () => {
        withProject((projectId) => {
            setInspectionDate(projectId);

            significantChangeOfsted
                .verifyAdditionalInformationHidden()
                .selectMeetsStandards(false)
                .verifyAdditionalInformationVisible()
                .enterAdditionalInformation('Improvement actions are underway')
                .enterSupportingEvidence('School improvement plan')
                .saveQuestion();

            cy.url().should('match', TASK_LIST_URL);
            significantChangeTaskList.verifyOfstedTaskVisible();
        });
    });

    it('Should hide additional information for Yes and return to the task list after saving', () => {
        withProject((projectId) => {
            setInspectionDate(projectId);

            significantChangeOfsted
                .selectMeetsStandards(true)
                .verifyAdditionalInformationHidden()
                .enterSupportingEvidence('Inspection report')
                .saveQuestion();

            cy.url().should('match', TASK_LIST_URL);
            significantChangeTaskList.verifyOfstedTaskVisible();
        });
    });
});
