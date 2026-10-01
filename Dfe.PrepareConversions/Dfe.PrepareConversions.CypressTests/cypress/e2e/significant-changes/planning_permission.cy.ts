/// <reference types="cypress" />
import significantChangePlanningPermission from '../../pages/significantChangePlanningPermission';
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import { Logger } from '../../support/logger';

const TASK_LIST_URL = /\/significant-change\/task-list\/\d+(\?.*)?$/;

const projectIdFrom = (url: string): string => url.split('?')[0].split('/').pop() as string;

describe('Significant change - planning permission', () => {
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

    it('Should open the planning permission page from the significant change task list', () => {
        withProject(() => {
            significantChangeTaskList.openPlanningPermissionTask();

            significantChangePlanningPermission.verifyPageLoaded().verifyAllOptionsPresent();
        });
    });

    it('Should show additional information only when No is selected', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/planning-permission`);

            significantChangePlanningPermission
                .verifyPageLoaded()
                .selectYes()
                .verifyAdditionalInformationHidden()
                .selectNotApplicable()
                .verifyAdditionalInformationHidden()
                .selectNo()
                .verifyAdditionalInformationVisible();
        });
    });

    it('Should save no with additional information and supporting evidence', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/planning-permission`);

            significantChangePlanningPermission
                .verifyPageLoaded()
                .selectNo()
                .enterAdditionalInformation('Planning permission is awaiting local authority approval')
                .enterSupportingEvidence('Planning application reference 67890')
                .save();

            cy.url().should('match', TASK_LIST_URL);
        });
    });

    it('Should save not applicable without additional information', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/planning-permission`);

            significantChangePlanningPermission
                .verifyPageLoaded()
                .selectNotApplicable()
                .enterSupportingEvidence('Planning permission is not required')
                .save();

            cy.url().should('match', TASK_LIST_URL);
        });
    });
});
