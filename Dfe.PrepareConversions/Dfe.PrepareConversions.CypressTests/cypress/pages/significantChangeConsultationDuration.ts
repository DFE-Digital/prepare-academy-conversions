/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeConsultationDuration extends BasePage {
    public path = 'consultation-duration';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Consultation duration');
        cy.contains('legend', 'Was this for a minimum 3 weeks during term time?').should('be.visible');
        return this;
    }

    public verifyAllOptionsPresent(): this {
        cy.getByDataTest('consultation-duration-yes').should('exist');
        cy.getByDataTest('consultation-duration-no-satisfactory').should('exist');
        cy.getByDataTest('consultation-duration-no').should('exist');
        cy.contains('label', 'No - satisfactory consultation carried out').should('be.visible');
        return this;
    }

    public selectYes(): this {
        cy.getByDataTest('consultation-duration-yes').check();
        return this;
    }

    public selectNoSatisfactory(): this {
        cy.getByDataTest('consultation-duration-no-satisfactory').check();
        return this;
    }

    public selectNo(): this {
        cy.getByDataTest('consultation-duration-no').check();
        return this;
    }

    public verifyReasonHidden(): this {
        cy.getByDataTest('consultation-duration-not-met-reason').should('not.be.visible');
        return this;
    }

    public verifyReasonVisible(): this {
        cy.getByDataTest('consultation-duration-not-met-reason').should('be.visible');
        return this;
    }

    public enterReason(reason: string): this {
        cy.getByDataTest('consultation-duration-not-met-reason').clear().type(reason);
        return this;
    }

    public save(): this {
        cy.getById('save-and-continue-button').click();
        return this;
    }

    public verifyErrorSummaryContains(message: string): this {
        cy.get('.govuk-error-summary').should('be.visible').and('contain.text', message);
        return this;
    }

    public verifyInlineError(field: string, message: string): this {
        cy.getById(`${field}-error`).should('contain.text', message);
        return this;
    }
}

const significantChangeConsultationDuration = new SignificantChangeConsultationDuration();

export default significantChangeConsultationDuration;
