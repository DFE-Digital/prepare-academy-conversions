/// <reference types="cypress" />
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import significantChangeRecommendation from '../../pages/significantChangeRecommendation';
import { Logger } from '../../support/logger';

describe('Significant change - recommendation', () => {
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

    it('Should show all fields', () => {
        openTaskList(() => {
            significantChangeTaskList.openRecommendationTask();

            significantChangeRecommendation
                .verifyPageLoaded()
                .verifyAllOptionsPresent()
                .verifyRecommendationMoreInformationVisible();
        });
    });

    it('Should require and option to be selected', () => {
        openTaskList(() => {
            significantChangeTaskList.openRecommendationTask();

            significantChangeRecommendation.save();

            significantChangeRecommendation
                .verifyErrorSummaryContains('Select an option')
                .verifyInlineError('Recommendation', 'Select an option');

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            significantChangeTaskList.verifyRecommendationTaskVisible();
        });
    });
});
