/// <reference types="cypress" />
import admissionVariationConsultation from '../../pages/admissionVariationConsultation';
import { Logger } from '../../support/logger';

const TASK_LIST_URL = /\/significant-change\/task-list\/\d+(\?.*)?$/;

const projectIdFrom = (url: string): string => url.split('?')[0].split('/').pop() as string;

describe('Admission variation consultation', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    it('Should open the admission variation consultation page from the significant change task list', () => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getById('school-name-0').click();
                    cy.contains('a', 'Admission variation consultation').click();

                    admissionVariationConsultation.verifyPageIsVisible();
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });

    it('Should show validation error when no is selected without a reason', () => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getById('school-name-0').click();
                    cy.contains('a', 'Admission variation consultation').click();

                    admissionVariationConsultation
                        .verifyPageIsVisible()
                        .selectNoAndContinueWithoutReason()
                        .verifyMissingReasonError();

                    cy.url().should(
                        'match',
                        /\/significant-change\/task-list\/\d+\/admission-variation-consultation(\?.*)?$/
                    );
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });

    it('Should save yes and redirect to the significant change task list', () => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getById('school-name-0').click();

                    cy.url().then((taskListUrl) => {
                        const projectId = projectIdFrom(taskListUrl);
                        cy.visit(`/significant-change/task-list/${projectId}/admission-variation-consultation`);

                        admissionVariationConsultation.verifyPageIsVisible().selectYesAndContinue();

                        cy.url().should('match', TASK_LIST_URL);
                    });
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });

    it('Should save no with reason and redirect to the significant change task list', () => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getById('school-name-0').click();

                    cy.url().then((taskListUrl) => {
                        const projectId = projectIdFrom(taskListUrl);
                        cy.visit(`/significant-change/task-list/${projectId}/admission-variation-consultation`);

                        admissionVariationConsultation
                            .verifyPageIsVisible()
                            .selectNoWithReasonAndContinue('Consultation did not include admission variation details');

                        cy.url().should('match', TASK_LIST_URL);
                    });
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });
});
