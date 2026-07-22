/// <reference types="cypress" />
import FinancialHealthAssessmentBasePage from './financialHealthAssessmentBasePage';

class RequestFinancialHealthAssessment extends FinancialHealthAssessmentBasePage {
    public path = 'request-financial-health-assessment';

    protected overviewField(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.getById('sfso-commissioning-overview');
    }
}

const requestFinancialHealthAssessment = new RequestFinancialHealthAssessment();
export default requestFinancialHealthAssessment;
