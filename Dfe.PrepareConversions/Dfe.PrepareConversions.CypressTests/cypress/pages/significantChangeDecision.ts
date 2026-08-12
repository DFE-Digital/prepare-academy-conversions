/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeDecision extends BasePage {
    public path = 'significant-change';

    public openFor(projectId: number | string): this {
        cy.visit(`/significant-change/${projectId}/decision/record-decision`);
        return this;
    }

    public selectDecision(decision: 'approved' | 'declined' | 'deferred' | 'withdrawn'): this {
        cy.getById(`${decision}-radio`).check();
        return this;
    }

    public setConditions(conditionsSet: boolean, details?: string): this {
        cy.getById(conditionsSet ? 'yes-radio' : 'no-radio').check();
        if (conditionsSet && details) {
            cy.getById('ApprovedConditionsDetails').clear().type(details);
        }
        return this;
    }

    public checkReason(reason: string, details: string): this {
        cy.getById(`${reason}-checkbox`).check();
        cy.getById(`${reason}-txtarea`).clear().type(details);
        return this;
    }

    public selectDecisionMaker(role: string): this {
        cy.getById(`${role}-radio`).check();
        return this;
    }

    public enterDecisionMakerName(name: string): this {
        cy.getById('decision-maker-name').clear().type(name);
        return this;
    }

    public enterDecisionDate(day: string, month: string, year: string): this {
        this.enterDate('decision-date', day, month, year);
        return this;
    }

    public submit(): this {
        cy.getById('submit-btn').click();
        return this;
    }

    public verifyErrorSummaryContains(text: string): this {
        cy.getByDataCy('error-summary').should('be.visible').and('contain.text', text);
        return this;
    }

    public verifySummaryRow(id: string, expected: string): this {
        cy.getById(id).should('contain.text', expected);
        return this;
    }

    public verifyOnStep(step: string): this {
        cy.url().should('match', new RegExp(String.raw`/significant-change/\d+/decision/${step}$`));
        return this;
    }
}

const significantChangeDecision = new SignificantChangeDecision();

export default significantChangeDecision;
