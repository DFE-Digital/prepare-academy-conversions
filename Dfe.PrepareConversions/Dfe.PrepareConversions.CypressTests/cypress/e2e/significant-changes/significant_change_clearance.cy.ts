/// <reference types="cypress" />
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import significantChangeClearance from '../../pages/significantChangeClearance';
import { Logger } from '../../support/logger';

describe('Significant change - clearance details', () => {
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
            significantChangeTaskList.verifyClearanceDetailsTaskVisible();
            significantChangeTaskList.openClearanceDetailsTask();

            significantChangeClearance.verifyPageLoaded().verifyClearanceFieldsVisible();
        });
    });

    it('Should require both clearance fields', () => {
        openTaskList(() => {
            significantChangeTaskList.openClearanceDetailsTask();

            significantChangeClearance.save();

            significantChangeClearance
                .verifyErrorSummaryContains('Enter who cleared the significant change in Regions Group')
                .verifyErrorSummaryContains('Enter who cleared the significant change from Trust Regulation')
                .verifyInlineError('RegionsGroupClearance', 'Enter who cleared the significant change in Regions Group')
                .verifyInlineError(
                    'TrustRegulationClearance',
                    'Enter who cleared the significant change from Trust Regulation'
                );

            cy.url().should('match', /\/significant-change\/task-list\/\d+\/clearance-details$/);
        });
    });

    it('Should save clearance details and return to the task list', () => {
        openTaskList(() => {
            significantChangeTaskList.openClearanceDetailsTask();

            significantChangeClearance
                .enterRegionsGroupClearance('Regions Group clearer')
                .enterTrustRegulationClearance('Trust Regulation clearer')
                .save();

            cy.url().should('match', /\/significant-change\/task-list\/\d+(\?.*)?$/);
            significantChangeTaskList.verifyClearanceDetailsTaskVisible();
        });
    });
});
