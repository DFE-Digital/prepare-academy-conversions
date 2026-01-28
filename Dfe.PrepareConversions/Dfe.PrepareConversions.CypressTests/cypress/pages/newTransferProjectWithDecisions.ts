import { AcademisationApiKey, AcademisationApiUrl } from '../constants/cypressConstants';
import FormBasePage from './formBasePage';

class NewTransferProjectWithDecisions extends FormBasePage {
    public path = 'transfers';

    public visit(url: string): this {
        cy.visit(url);
        return this;
    }

    public createNewTransferProject(): this {
        cy.getByDataTest('create-transfer').click();
        cy.contains('button.govuk-button', 'Create a new transfer').click();
        return this;
    }

    public searchOutgoingTrust(trustName: string): this {
        cy.getById('SearchQuery').type(trustName);
        cy.get('button.govuk-button').contains('Search').click();
        return this;
    }

    public selectOutgoingTrust(trustId: string): this {
        cy.getById(trustId).check();
        cy.clickContinueBtn();
        return this;
    }

    public createProjectButton(): this {
        cy.getByDataTest('create-project').click();
        return this;
    }

    public confirmOutgoingTrust(): this {
        cy.getByDataTest('confirm-outgoing-trust').click();
        return this;
    }

    public selectOptionById(optionId: string): this {
        cy.getById(optionId).click();
        return this;
    }

    public submitForm(): this {
        cy.get('[type="submit"]').click();
        return this;
    }

    public submitFormRecordDecision(): this {
        cy.clickSubmitBtn();
        return this;
    }

    public recordDecision(): this {
        cy.containsText('Record a decision').click();
        cy.getById('record-decision-link').click();
        return this;
    }

    public addPerformanceConcerns(concernsText: string): this {
        cy.getById('performanceconcerns-checkbox').check();
        cy.getById('performanceconcerns-txtarea').type(concernsText);
        cy.clickSubmitBtn();
        return this;
    }

    public verifyDecisionDetails(): this {
        cy.containsText('Record a decision').click();
        cy.getById('decision').should('contain', 'Deferred');
        cy.getById('decision-made-by').should('contain', 'Grade 6');
        cy.getById('deferred-reasons').should('contain', 'Performance concerns:');
        cy.getById('deferred-reasons').should('contain', 'Cypress Test');
        cy.getById('decision-date').should('contain', '12 December 2023');
        return this;
    }

    public assertTrustName(expectedName: string): this {
        cy.getByDataCy('trust_Name').should('contain', expectedName);
        return this;
    }

    public assertURNNumber(expectedNumber: string): this {
        cy.getByDataCy('URN_Id').should('contain', expectedNumber);
        return this;
    }

    public deleteProject(projectId: string): this {
        const deleteUrl = `${Cypress.env(AcademisationApiUrl)}/transfer-project/${projectId}/delete`;
        const academisationApiKey = Cypress.env(AcademisationApiKey);

        cy.request({
            method: 'DELETE',
            url: deleteUrl,
            headers: {
                'x-api-key': academisationApiKey,
            },
        }).then((response) => {
            expect(response.status).to.eq(200); // Verify the response status
        });

        return this;
    }
}

const newTransferProjectWithDecisions = new NewTransferProjectWithDecisions();

export { NewTransferProjectWithDecisions };
export default newTransferProjectWithDecisions;
