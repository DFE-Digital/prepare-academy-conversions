/// <reference types="cypress" />
import BasePage from './basePage';

class AdmissionsVariationRecommendation extends BasePage {
    public path = 'admissions-variation-recommendation';

    private readonly selectors = {
        approveRadio: 'admissions-variation-recommendation-approve',
        deferRadio: 'admissions-variation-recommendation-defer',
        declineRadio: 'admissions-variation-recommendation-decline',
        notApplicableRadio: 'admissions-variation-recommendation-notapplicable',
        furtherInformation: 'admissions-variation-recommendation-further-information',
        answerError: 'AdmissionsVariationRecommendationAnswer-error',
        saveAndContinueBtn: 'select-common-submitbutton',
    };

    public verifyPageIsVisible(): this {
        cy.checkPath(this.path);
        cy.get('h1').should('contain.text', 'Recommendation');
        cy.contains('Enter a recommendation for this significant change.').should('be.visible');
        cy.getByDataTest(this.selectors.approveRadio).should('exist');
        cy.getByDataTest(this.selectors.deferRadio).should('exist');
        cy.getByDataTest(this.selectors.declineRadio).should('exist');
        cy.getByDataTest(this.selectors.notApplicableRadio).should('exist');
        cy.getByDataTest(this.selectors.furtherInformation).should('exist');
        return this;
    }

    public selectApproveAndContinue(): this {
        cy.getByDataTest(this.selectors.approveRadio).check();
        cy.getByDataCy(this.selectors.saveAndContinueBtn).click();
        return this;
    }

    public selectDeferWithFurtherInformationAndContinue(furtherInformation: string): this {
        cy.getByDataTest(this.selectors.deferRadio).check();
        cy.getByDataTest(this.selectors.furtherInformation).clear();
        cy.getByDataTest(this.selectors.furtherInformation).type(furtherInformation);
        cy.getByDataCy(this.selectors.saveAndContinueBtn).click();
        return this;
    }

    public continueWithoutSelectingOption(): this {
        cy.getByDataCy(this.selectors.saveAndContinueBtn).click();
        return this;
    }

    public verifyMissingSelectionError(): this {
        cy.getById(this.selectors.answerError).should('contain.text', 'Select an option');
        return this;
    }
}

const admissionsVariationRecommendation = new AdmissionsVariationRecommendation();

export default admissionsVariationRecommendation;
