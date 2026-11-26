/// <reference types="cypress" />
import significantChangeTrustExperienceAndGovernance from '../../pages/significantChangeTrustExperienceAndGovernance';
import { Logger } from '../../support/logger';
import { invalidSharePointLinks, verifySharePointLinkValidation } from 'cypress/support/sharePointLinkValidation';

describe('Significant change - trust experience and governance', () => {
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
            significantChangeTrustExperienceAndGovernance.openTask('Trust experience and governance');

            significantChangeTrustExperienceAndGovernance
                .verifyPageLoaded()
                .verifyAllOptionsPresent(['answer-yes', 'answer-no', 'not-applicable'])
                .verifySupportingEvidencePresent();
        });
    });

    it('Should require additional information when No is selected', () => {
        openTaskList(() => {
            significantChangeTrustExperienceAndGovernance.openTask('Trust experience and governance');

            significantChangeTrustExperienceAndGovernance
                .selectNo()
                .enterAdditionalInformation(' ')
                .clickSaveButton()
                .verifyErrorSummaryContains('Enter details')
                .verifyInlineError('AdditionalInformation', 'Enter details');

            significantChangeTrustExperienceAndGovernance
                .selectNo()
                .enterAdditionalInformation('Governance arrangements are being strengthened')
                .enterSupportingEvidence('https://educationgovuk.sharepoint.com/sites/trust/evidence')
                .clickSaveButton();

            cy.url().should('match', /\/significant-change\/task-list\/\d+$/);
            cy.contains('a', 'Trust experience and governance').should('be.visible');
        });
    });

    it('Should show a validation error when no answer is selected', () => {
        openTaskList(() => {
            significantChangeTrustExperienceAndGovernance.openTask('Trust experience and governance');

            significantChangeTrustExperienceAndGovernance
                .clickSaveButton()
                .verifyErrorSummaryContains(
                    'Select whether the trust currently has the experience and governance arrangements in place to support the proposed change'
                );
        });
    });

    invalidSharePointLinks.forEach(({ name, value, error }) => {
        it(`rejects ${name} as supporting evidence`, () => {
            openTaskList(() => {
                significantChangeTrustExperienceAndGovernance.openTask('Trust experience and governance');
                significantChangeTrustExperienceAndGovernance.selectYes();
                verifySharePointLinkValidation('SupportingEvidence', value, error);
            });
        });
    });
});
