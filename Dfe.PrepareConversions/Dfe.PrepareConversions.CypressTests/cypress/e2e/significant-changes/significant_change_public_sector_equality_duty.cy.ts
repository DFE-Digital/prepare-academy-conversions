/// <reference types="cypress" />
import significantChangePublicSectorEqualityDuty from '../../pages/significantChangePublicSectorEqualityDuty';
import significantChangeTaskList from '../../pages/significantChangeTaskList';
import { Logger } from '../../support/logger';

const TASK_LIST_URL = /\/significant-change\/task-list\/\d+(\?.*)?$/;

const projectIdFrom = (url: string): string => url.split('?')[0].split('/').pop() as string;

describe('Significant change - public sector equality duty', () => {
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

    it('Should open the public sector equality duty page from the significant change task list', () => {
        withProject(() => {
            significantChangeTaskList.openPublicSectorEqualityDutyTask();

            significantChangePublicSectorEqualityDuty.verifyPageLoaded().verifyAllOptionsPresent();
        });
    });

    it('Should show additional information only when Likely is selected', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/public-sector-equality-duty`);

            significantChangePublicSectorEqualityDuty
                .verifyPageLoaded()
                .selectUnlikely()
                .verifyAdditionalInformationHidden()
                .selectSomeImpact()
                .verifyAdditionalInformationHidden()
                .selectLikely()
                .verifyAdditionalInformationVisible();
        });
    });

    it('Should save unlikely with supporting evidence and keep that evidence', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/public-sector-equality-duty`);

            const supportingEvidence = `Unlikely evidence ${Date.now()}`;

            significantChangePublicSectorEqualityDuty
                .verifyPageLoaded()
                .selectAssessmentCompletedYes()
                .selectUnlikely()
                .enterSupportingEvidence(supportingEvidence)
                .save();

            cy.url().should('match', TASK_LIST_URL);
            significantChangeTaskList.verifyPublicSectorEqualityDutyTaskVisible();

            cy.visit(`/significant-change/task-list/${projectId}/public-sector-equality-duty`);

            significantChangePublicSectorEqualityDuty.verifyPageLoaded().verifySupportingEvidence(supportingEvidence);
            cy.getByDataTest('assessment-completed-yes').should('be.checked');
            cy.getByDataTest('equalities-impact-unlikely').should('be.checked');
        });
    });

    it('Should save some impact with supporting evidence', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/public-sector-equality-duty`);

            const supportingEvidence = `Some impact evidence ${Date.now()}`;

            significantChangePublicSectorEqualityDuty
                .verifyPageLoaded()
                .selectAssessmentCompletedNo()
                .selectSomeImpact()
                .enterSupportingEvidence(supportingEvidence)
                .save();

            cy.url().should('match', TASK_LIST_URL);

            cy.visit(`/significant-change/task-list/${projectId}/public-sector-equality-duty`);

            significantChangePublicSectorEqualityDuty.verifySupportingEvidence(supportingEvidence);
            cy.getByDataTest('assessment-completed-no').should('be.checked');
            cy.getByDataTest('equalities-impact-some').should('be.checked');
            significantChangePublicSectorEqualityDuty.verifyAdditionalInformationHidden();
        });
    });

    it('Should save likely with additional information and supporting evidence', () => {
        withProject((projectId) => {
            cy.visit(`/significant-change/task-list/${projectId}/public-sector-equality-duty`);

            const additionalInformation = 'Pupils with SEND will receive additional transitional support';
            const supportingEvidence = `Likely evidence ${Date.now()}`;

            significantChangePublicSectorEqualityDuty
                .verifyPageLoaded()
                .selectAssessmentCompletedYes()
                .selectLikely()
                .enterAdditionalInformation(additionalInformation)
                .enterSupportingEvidence(supportingEvidence)
                .save();

            cy.url().should('match', TASK_LIST_URL);

            cy.visit(`/significant-change/task-list/${projectId}/public-sector-equality-duty`);

            significantChangePublicSectorEqualityDuty
                .verifyPageLoaded()
                .verifyAdditionalInformationVisible()
                .verifySupportingEvidence(supportingEvidence);
            cy.getByDataTest('equalities-impact-likely').should('be.checked');
            cy.getByDataTest('which-groups-affected').should('have.value', additionalInformation);
        });
    });
});
