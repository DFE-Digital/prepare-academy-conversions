/// <reference types="cypress" />
import BasePage from './basePage';

class AdmissionsVariationDetails extends BasePage {
    public path = 'admissions-variation-details';

    public verifyFirstPage(): this {
        cy.checkPath(this.path);
        cy.get('h1').should('contain.text', 'Admissions variation details');
        cy.contains('legend', 'Is an admissions variation required for this significant change?').should('be.visible');
        cy.getByDataTest('admissions-variation-required-yes').should('exist');
        cy.getByDataTest('admissions-variation-required-no').should('exist');
        cy.getByDataTest('admissions-variation-supporting-evidence').should('exist');
        return this;
    }

    public chooseNo(): this {
        cy.getByDataTest('admissions-variation-required-no').check();
        return this;
    }

    public chooseYes(): this {
        cy.getByDataTest('admissions-variation-required-yes').check();
        return this;
    }

    public enterSupportingEvidence(value: string): this {
        cy.getByDataTest('admissions-variation-supporting-evidence').clear().type(value);
        return this;
    }

    public continue(): this {
        cy.getById('save-and-continue-button').click();
        return this;
    }

    public verifySupportPage(): this {
        cy.url().should('include', `/${this.path}/support`);
        cy.get('h1').should('contain.text', 'Admissions variation details');
        cy.contains('legend', 'Is the local authority supportive of the admissions variation?').should('be.visible');
        cy.contains('legend', 'Is the Pupil Place Planning Team supportive of the admissions variation?').should(
            'be.visible'
        );
        cy.contains('legend', 'Is the Admissions Team supportive of the admissions variation?').should('be.visible');
        return this;
    }

    public answerSupportQuestions(): this {
        cy.getByDataTest('local-authority-supportive-yes').check();
        cy.getByDataTest('pupil-place-planning-supportive-no').check();
        cy.getByDataTest('admissions-team-supportive-yes').check();
        return this;
    }
}

const admissionsVariationDetails = new AdmissionsVariationDetails();

export default admissionsVariationDetails;
