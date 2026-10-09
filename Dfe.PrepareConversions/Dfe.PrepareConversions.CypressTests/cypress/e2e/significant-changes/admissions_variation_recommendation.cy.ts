/// <reference types="cypress" />
import admissionsVariationRecommendation from '../../pages/admissionsVariationRecommendation';
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import { Logger } from '../../support/logger';

const TASK_LIST_URL = /\/significant-change\/task-list\/\d+(\?.*)?$/;
const RECOMMENDATION_PAGE_URL =
    /\/significant-change\/task-list\/\d+\/admissions-variation-recommendation(\?.*)?$/;

const projectIdFrom = (url: string): string => url.split('?')[0].split('/').pop() as string;

describe('Significant change - admissions variation recommendation', () => {
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

    it('Should open the recommendation page from the significant change task list', () => {
        withProject(() => {
            significantChangeTaskList.openAdmissionsVariationRecommendationTask();

            admissionsVariationRecommendation.verifyPageIsVisible();
        });
    });

    it('Should show validation error when no recommendation is selected', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/admissions-variation-recommendation`);

            admissionsVariationRecommendation
                .verifyPageIsVisible()
                .continueWithoutSelectingOption()
                .verifyMissingSelectionError();

            cy.url().should('match', RECOMMENDATION_PAGE_URL);
        });
    });

    it('Should save approve and redirect to the significant change task list', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/admissions-variation-recommendation`);

            admissionsVariationRecommendation.verifyPageIsVisible().selectApproveAndContinue();

            cy.url().should('match', TASK_LIST_URL);
        });
    });

    it('Should save defer with further information and redirect to the significant change task list', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/admissions-variation-recommendation`);

            admissionsVariationRecommendation
                .verifyPageIsVisible()
                .selectDeferWithFurtherInformationAndContinue('Subject to board approval');

            cy.url().should('match', TASK_LIST_URL);
        });
    });
});
