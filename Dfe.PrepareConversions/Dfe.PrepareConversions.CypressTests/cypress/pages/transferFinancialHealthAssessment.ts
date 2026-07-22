/// <reference types="cypress" />
import FinancialHealthAssessmentBasePage from './financialHealthAssessmentBasePage';

class TransferFinancialHealthAssessment extends FinancialHealthAssessmentBasePage {
    public path = 'financial-health-assessment';

    protected overviewField(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.getByDataTest('sfso-commissioning-overview');
    }
}

const transferFinancialHealthAssessment = new TransferFinancialHealthAssessment();
export default transferFinancialHealthAssessment;