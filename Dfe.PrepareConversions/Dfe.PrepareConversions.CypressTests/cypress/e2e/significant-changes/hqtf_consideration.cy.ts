/// <reference types="cypress" />
import significantChangeHqtfConsideration from '../../pages/significantChangeHqtfConsideration';
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import { Logger } from '../../support/logger';

const TASK_LIST_URL = /\/significant-change\/task-list\/\d+(\?.*)?$/;

const projectIdFrom = (url: string): string => url.split('?')[0].split('/').pop() as string;

describe('Significant change - HQTF consideration', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    const withProject = (then: (projectId: string) => void) => {
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

    it('Should open the HQTF consideration page from the significant change task list', () => {
        withProject(() => {
            significantChangeTaskList.openHqtfConsiderationTask();

            significantChangeHqtfConsideration.verifyPageLoaded().verifyHqtfAllOptionsPresent();
        });
    });

    it('Should show additional information only when Yes is selected', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/hqtf-consideration`);

            significantChangeHqtfConsideration
                .verifyPageLoaded()
                .selectHqtfNo()
                .verifyHqtfAdditionalInformationHidden()
                .selectHqtfNotApplicable()
                .verifyHqtfAdditionalInformationHidden()
                .selectHqtfYes()
                .verifyHqtfAdditionalInformationVisible();
        });
    });

    it('Should require additional information when Yes is selected, then save and return to the task list', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/hqtf-consideration`);

            significantChangeHqtfConsideration.selectHqtfYes().clearHqtfAdditionalInformation().save();

            significantChangeHqtfConsideration
                .verifyErrorSummaryContains('Add details of the issues or risks identified')
                .verifyInlineError('AdditionalInformation', 'Add details of the issues or risks identified');

            significantChangeHqtfConsideration
                .selectHqtfYes()
                .enterHqtfAdditionalInformation('Workforce capacity risk identified against HQTF pillars')
                .enterHqtfSupportingEvidence('Trust board minutes')
                .save();

            cy.url().should('match', TASK_LIST_URL);
            significantChangeTaskList.verifyHqtfConsiderationTaskVisible();
        });
    });

    it('Should save the No answer without additional information', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/hqtf-consideration`);

            significantChangeHqtfConsideration
                .verifyPageLoaded()
                .selectHqtfNo()
                .enterHqtfSupportingEvidence('No issues identified evidence')
                .save();

            cy.url().should('match', TASK_LIST_URL);
            significantChangeTaskList.verifyHqtfConsiderationTaskVisible();
        });
    });

    it('Should save the not applicable answer without additional information', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/hqtf-consideration`);

            significantChangeHqtfConsideration.verifyPageLoaded().selectHqtfNotApplicable().save();

            cy.url().should('match', TASK_LIST_URL);
            significantChangeTaskList.verifyHqtfConsiderationTaskVisible();
        });
    });
});
