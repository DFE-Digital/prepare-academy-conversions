/// <reference types="cypress" />
import significantChangeDecision from '../../pages/significantChangeDecision';
import { Logger } from '../../support/logger';

describe('Significant change record a decision', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    const withAProject = (test: (projectId: string) => void) => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count === 0) {
                    cy.contains('There are no matching results.').should('be.visible');
                    return;
                }

                cy.getById('school-name-0').click();
                cy.url().then((url) => {
                    const idMatch = url.match(/\/significant-change\/task-list\/(\d+)/);
                    expect(idMatch, 'project id in task list url').to.not.be.null;
                    test(idMatch![1]);
                });
            });
    };

    it('Should show a Record a decision tab on the project details page', () => {
        withAProject(() => {
            cy.getByDataCy('significant_change_record_decision_menu')
                .should('be.visible')
                .and('contain.text', 'Record a decision');
        });
    });

    it('Should navigate to the wizard from the tab', () => {
        withAProject(() => {
            cy.getByDataCy('significant_change_record_decision_menu').click();
            significantChangeDecision.verifyOnStep('record-decision');
            significantChangeDecision.verifyHeading('Record the decision');
        });
    });

    it('Should show an error when no decision is selected', () => {
        withAProject((projectId) => {
            significantChangeDecision.openFor(projectId).submit();
            significantChangeDecision.verifyErrorSummaryContains('Select a decision');
        });
    });

    it('Should complete an approved decision with conditions', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('approved')
                .submit()
                .verifyOnStep('any-conditions')
                .setConditions(true, 'Trust must appoint a new chair')
                .submit()
                .verifyOnStep('who-decided')
                .selectDecisionMaker('regionaldirectorforregion')
                .submit()
                .verifyOnStep('decision-maker')
                .enterDecisionMakerName('Jane Smith')
                .submit()
                .verifyOnStep('decision-date')
                .enterDecisionDate('27', '3', '2026')
                .submit()
                .verifyOnStep('summary');

            significantChangeDecision
                .verifySummaryRow('decision', 'Approved with conditions')
                .verifySummaryRow('condition-set', 'Trust must appoint a new chair')
                .verifySummaryRow('decision-made-by', 'Regional Director for the region')
                .verifySummaryRow('decision-maker-name', 'Jane Smith')
                .verifySummaryRow('decision-date', '27 March 2026');
        });
    });

    it('Should require a reason when declining', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('declined')
                .submit()
                .verifyOnStep('declined-reason')
                .submit()
                .verifyErrorSummaryContains('Select at least one reason');
        });
    });

    it('Should complete a declined decision with multiple reasons', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('declined')
                .submit()
                .checkReason('governance', 'Weak governance')
                .checkReason('choiceoftrust', 'Trust not suitable')
                .submit()
                .verifyOnStep('who-decided')
                .selectDecisionMaker('minister')
                .submit()
                .enterDecisionMakerName('Jane Smith')
                .submit()
                .enterDecisionDate('27', '3', '2026')
                .submit()
                .verifyOnStep('summary')
                .verifySummaryRow('decision', 'Declined')
                .verifySummaryRow('reasons', 'Weak governance');
        });
    });

    it('Should skip the decision maker name when nobody made the decision', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('deferred')
                .submit()
                .checkReason('performanceconcerns', 'Results declining')
                .submit()
                .verifyOnStep('who-decided')
                .selectDecisionMaker('none')
                .submit()
                .verifyOnStep('decision-date');
        });
    });

    it('Should reject a decision date in the future', () => {
        withAProject((projectId) => {
            significantChangeDecision
                .openFor(projectId)
                .selectDecision('withdrawn')
                .submit()
                .checkReason('other', 'Trust withdrew')
                .submit()
                .selectDecisionMaker('grade6')
                .submit()
                .enterDecisionMakerName('Jane Smith')
                .submit()
                .enterDecisionDate('1', '1', '2099')
                .submit()
                .verifyOnStep('decision-date');

            cy.getByDataCy('error-summary').should('be.visible');
        });
    });
});
