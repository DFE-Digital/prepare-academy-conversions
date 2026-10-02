/// <reference types="cypress" />

describe('Land Transaction Form', () => {
    const TASK_LIST_URL = /^.*\/significant-change\/task-list\/\d+(\?.*)?$/;

    const navigateToTaskPage = () => {
        cy.login();
        cy.acceptCookies();
        cy.visit('/significant-change/project-list');
        cy.getById('school-name-0').click();
        cy.contains('a', 'Land Transaction Application').click();
    };

    beforeEach(() => {
        navigateToTaskPage();

        // Clear project values so interaction and validation checks start from a blank form.
        cy.get('input[name="LandTransactionApplication"]').invoke('prop', 'checked', false);
        cy.get('input[name="LandTransactionConsent"]').invoke('prop', 'checked', false);
        cy.get('input[name="LandTransactionSupportingEvidence"]').clear();
    });

    it('should expose both question groups with descriptive legends and correctly associated radio labels', () => {
        cy.getByDataTest('land-transction-application-fieldset')
            .find('legend')
            .should('contain.text', 'Has the landowner submitted a land transaction application?');
        cy.getByDataTest('land-transction-consent-fieldset')
            .find('legend')
            .should('contain.text', 'Have the Land Transaction Team consented to the change?');

        cy.getByDataTest('land-transction-application-fieldset').within(() => {
            cy.contains('label', 'Yes').click();
            cy.get('input[value="Yes"]').should('be.checked');

            cy.contains('label', 'No').click();
            cy.get('input[value="No"]').should('be.checked');

            cy.contains('label', 'Not Applicable').click();
            cy.get('input[value="NotApplicable"]').should('be.checked');
        });

        cy.getByDataTest('land-transction-consent-fieldset').within(() => {
            cy.contains('label', 'Yes').click();
            cy.get('input[value="Yes"]').should('be.checked');

            cy.contains('label', 'No').click();
            cy.get('input[value="No"]').should('be.checked');

            cy.contains('label', 'Not Applicable').click();
            cy.get('input[value="NotApplicable"]').should('be.checked');
        });
    });

    it('should reveal each details field for No and move focus when its label is activated', () => {
        cy.getByDataTest('land-transction-application-fieldset').within(() => {
            cy.contains('label', 'No').click();
            cy.get('textarea[name="LandTransactionApplicationAdditionalInfo"]').should('be.visible');
            cy.contains('label', 'Give Details').click();
            cy.get('textarea[name="LandTransactionApplicationAdditionalInfo"]').should('have.focus');

            cy.contains('label', 'Yes').click();
            cy.get('textarea[name="LandTransactionApplicationAdditionalInfo"]').should('not.be.visible');
        });

        cy.getByDataTest('land-transction-consent-fieldset').within(() => {
            cy.contains('label', 'No').click();
            cy.get('textarea[name="LandTransactionConsentAdditionalInfo"]').should('be.visible');
            cy.contains('label', 'Give Details').click();
            cy.get('textarea[name="LandTransactionConsentAdditionalInfo"]').should('have.focus');

            cy.contains('label', 'Not Applicable').click();
            cy.get('textarea[name="LandTransactionConsentAdditionalInfo"]').should('not.be.visible');
        });
    });

    it('should associate the supporting evidence label with its input and focus it when clicked', () => {
        cy.get('label[for="supporting-evidence"]').should('contain.text', 'Supporting Evidence').click();
        cy.getById('supporting-evidence').should('have.focus');
    });

    it('should have no detectable WCAG 2.1 AA or 2.2 AA violations', () => {
        cy.getByDataTest('land-transction-application-fieldset').should('be.visible');
        cy.executeAccessibilityTests();
    });

    it('should show an accessible validation message for each unanswered question', () => {
        cy.contains('button', 'Save and continue').click();

        cy.getById('LandTransactionApplication-error').should('be.visible').and('contain.text', 'Select an option');
        cy.getById('LandTransactionConsent-error').should('be.visible').and('contain.text', 'Select an option');
    });

    it('should require details when either answer is No', () => {
        cy.getByDataTest('land-transction-application-fieldset').contains('label', 'No').click();
        cy.get('textarea[name="LandTransactionApplicationAdditionalInfo"]').should('be.visible').clear();
        cy.getByDataTest('land-transction-consent-fieldset').contains('label', 'No').click();
        cy.get('textarea[name="LandTransactionConsentAdditionalInfo"]').should('be.visible').clear();
        cy.contains('button', 'Save and continue').click();

        cy.getById('LandTransactionApplicationAdditionalInfo-error')
            .should('be.visible')
            .and('contain.text', 'Enter the additional information provided');
        cy.getById('LandTransactionConsentAdditionalInfo-error')
            .should('be.visible')
            .and('contain.text', 'Enter the additional information provided');
    });

    it('should save both No answers with details and return to the task list', () => {
        cy.getByDataTest('land-transction-application-fieldset').contains('label', 'No').click();
        cy.get('textarea[name="LandTransactionApplicationAdditionalInfo"]').should('be.visible').clear();
        cy.get('textarea[name="LandTransactionApplicationAdditionalInfo"]').type('Application details');

        cy.getByDataTest('land-transction-consent-fieldset').contains('label', 'No').click();
        cy.get('textarea[name="LandTransactionConsentAdditionalInfo"]').should('be.visible').clear();
        cy.get('textarea[name="LandTransactionConsentAdditionalInfo"]').type('Consent details');
        cy.getById('supporting-evidence').type('Evidence link');

        cy.contains('button', 'Save and continue').click();
        cy.urlPath().should('match', TASK_LIST_URL);
    });

    it('should save Yes and Not Applicable answers without conditional details', () => {
        cy.getByDataTest('land-transction-application-fieldset').contains('label', 'Yes').click();
        cy.getByDataTest('land-transction-consent-fieldset').contains('label', 'Not Applicable').click();

        cy.contains('button', 'Save and continue').click();
        cy.urlPath().should('match', TASK_LIST_URL);
    });
});
