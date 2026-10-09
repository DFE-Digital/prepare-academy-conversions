/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangePublicSectorEqualityDuty extends BasePage {
    public path = 'public-sector-equality-duty';

    public verifyPageLoaded(): this {
        cy.checkPath(this.path);
        this.verifyHeading('Public Sector Equality Duty');
        cy.contains(
            'legend',
            'Has the Public Sector Equality Duty been considered and an Equalities Impact Assessment been completed?'
        ).should('be.visible');
        cy.contains(
            'legend',
            'Is the decision likely to disproportionately affect any particular person or group who share protected characteristics?'
        ).should('be.visible');
        cy.contains('label', 'Supporting evidence').should('be.visible');
        return this;
    }

    public verifyAllOptionsPresent(): this {
        cy.getByDataTest('assessment-completed-yes').should('exist');
        cy.getByDataTest('assessment-completed-no').should('exist');
        cy.getByDataTest('equalities-impact-unlikely').should('exist');
        cy.getByDataTest('equalities-impact-some').should('exist');
        cy.getByDataTest('equalities-impact-likely').should('exist');
        cy.contains('label', 'Yes').should('be.visible');
        cy.contains('label', 'No').should('be.visible');
        cy.contains('label', 'Unlikely').should('be.visible');
        cy.contains('label', 'Some impact').should('be.visible');
        cy.contains('label', 'Likely').should('be.visible');
        cy.getByDataTest('psed-supporting-evidence').should('be.visible');
        return this;
    }

    public selectAssessmentCompletedYes(): this {
        cy.getByDataTest('assessment-completed-yes').check();
        return this;
    }

    public selectAssessmentCompletedNo(): this {
        cy.getByDataTest('assessment-completed-no').check();
        return this;
    }

    public selectUnlikely(): this {
        cy.getByDataTest('equalities-impact-unlikely').check();
        return this;
    }

    public selectSomeImpact(): this {
        cy.getByDataTest('equalities-impact-some').check();
        return this;
    }

    public selectLikely(): this {
        cy.getByDataTest('equalities-impact-likely').check();
        return this;
    }

    public verifyLikelyAdditionalInformationHidden(): this {
        cy.getByDataTest('which-groups-affected').should('not.be.visible');
        return this;
    }

    public verifyLikelyAdditionalInformationVisible(): this {
        cy.getByDataTest('which-groups-affected').should('be.visible');
        return this;
    }

    public verifySomeImpactAdditionalInformationHidden(): this {
        cy.getByDataTest('some-impact-details').should('not.be.visible');
        return this;
    }

    public verifySomeImpactAdditionalInformationVisible(): this {
        cy.getByDataTest('some-impact-details').should('be.visible');
        return this;
    }

    public enterLikelyAdditionalInformation(text: string): this {
        cy.getByDataTest('which-groups-affected').clear();
        cy.getByDataTest('which-groups-affected').type(text);
        return this;
    }

    public enterSomeImpactAdditionalInformation(text: string): this {
        cy.getByDataTest('some-impact-details').clear();
        cy.getByDataTest('some-impact-details').type(text);
        return this;
    }

    public clearLikelyAdditionalInformation(): this {
        cy.getByDataTest('which-groups-affected').clear();
        return this;
    }

    public clearSomeImpactAdditionalInformation(): this {
        cy.getByDataTest('some-impact-details').clear();
        return this;
    }

    public enterSupportingEvidence(text: string): this {
        cy.getByDataTest('psed-supporting-evidence').clear();
        cy.getByDataTest('psed-supporting-evidence').type(text);
        return this;
    }

    public verifySupportingEvidence(text: string): this {
        cy.getByDataTest('psed-supporting-evidence').should('have.value', text);
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

    public verifyStillOnPage(projectId: string): this {
        cy.url().should('include', `/significant-change/task-list/${projectId}/public-sector-equality-duty`);
        return this;
    }
}

const significantChangePublicSectorEqualityDuty = new SignificantChangePublicSectorEqualityDuty();

export default significantChangePublicSectorEqualityDuty;
