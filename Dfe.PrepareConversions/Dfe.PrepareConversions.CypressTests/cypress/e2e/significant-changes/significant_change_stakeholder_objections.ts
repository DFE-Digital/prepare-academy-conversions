/// <reference types="cypress" />
const TASK_LIST_URL = /^.*\/significant-change\/task-list\/\d+(\?.*)?$/;

const navigateToTaskPage = () => {
    cy.login();
    cy.acceptCookies();
    cy.visit('/significant-change/project-list');
    cy.getById('school-name-0').click();
    cy.contains('a', 'Stakeholder objections').click();
};

describe('Stakeholder Objections Form', () => {
    beforeEach(() => {
        navigateToTaskPage();

        // Force a blank form state for each test
        cy.get('input[name="StakeholderObjections"]').invoke('prop', 'checked', false);
        cy.get('textarea[name="StakeholderObjectionsComment"]').clear({ force: true });
    });

    describe('stakeholder-objections-form-group', () => {
        it('should render the form group', () => {
            cy.getByDataTest('stakeholder-objections-form-group').should('exist');
        });

        it('should have the correct initial empty form state', () => {
            cy.get('input[name="StakeholderObjections"]').should('not.be.checked');
            cy.get('textarea[name="StakeholderObjectionsComment"]').should('not.be.visible');
            cy.get('.govuk-error-message').should('not.exist');
        });

        it('should tie StakeholderObjections labels to radio options', () => {
            cy.getByDataTest('stakeholder-objections-form-group').within(() => {
                cy.contains('label', 'No').click();
                cy.get('input[value="No"]').should('be.checked');

                cy.contains('label', 'Yes - all objections have been addressed').click();
                cy.get('input[value="YesAllObjectionsAddressed"]').should('be.checked');

                cy.contains('label', 'Yes - no further information provided').click();
                cy.get('input[value="YesNoFurtherInformationProvided"]').should('be.checked');
            });
        });

        it('should show StakeholderObjectionsComment when StakeholderObjections is YesNoFurtherInformationProvided and transfer focus when its linked label is clicked', () => {
            const inputName = 'StakeholderObjectionsComment';

            cy.getByDataTest('stakeholder-objections-form-group').within(() => {
                // Select the conditional trigger option
                cy.contains('label', 'Yes - no further information provided').click();
                cy.get(`textarea[name="${inputName}"]`).should('be.visible');

                // Accessibility check: Click label -> ensure StakeholderObjectionsComment focuses
                cy.contains(
                    'label',
                    'Use this area for additional Comments relating to objections raised by stakeholders'
                )
                    .should('be.visible')
                    .click();
                cy.get(`textarea[name="${inputName}"]`).should('have.focus');

                // Toggling away should hide the field again
                cy.contains('label', 'Yes - all objections have been addressed').click();
                cy.get(`textarea[name="${inputName}"]`).should('not.be.visible');

                // Selecting No should not display the field
                cy.contains('label', 'No').click();
                cy.get(`textarea[name="${inputName}"]`).should('not.be.visible');
            });
        });

        it('should block submission and display error when StakeholderObjections radio options are not selected', () => {
            cy.contains('button', 'Save and continue').click();

            cy.get('span').contains('Select whether there are any stakeholder objections').should('be.visible');
        });

        it('should block submission and display error when StakeholderObjections is YesNoFurtherInformationProvided and StakeholderObjectionsComment is empty', () => {
            cy.contains('label', 'Yes - no further information provided').click();
            cy.contains(
                'label',
                'Use this area for additional Comments relating to objections raised by stakeholders'
            ).click();

            cy.contains('button', 'Save and continue').click();

            cy.get('span').contains('Enter the additional information provided').should('be.visible');
        });
    });

    describe('Form Submissions', () => {
        it('should submit and redirect when StakeholderObjections is "No"', () => {
            cy.contains('label', 'No').click();
            cy.contains('button', 'Save and continue').click();
            cy.urlPath().should('match', TASK_LIST_URL);
        });

        it('should submit and redirect when StakeholderObjections is "Yes - all objections have been addressed"', () => {
            cy.contains('label', 'Yes - all objections have been addressed').click();
            cy.contains('button', 'Save and continue').click();
            cy.urlPath().should('match', TASK_LIST_URL);
        });

        it('should submit and redirect when StakeholderObjections is "Yes - no further information provided" and StakeholderObjectionsComment is NOT empty', () => {
            cy.contains('label', 'Yes - no further information provided').click();

            cy.contains('label', 'Use this area for additional Comments').click();
            cy.focused().type('Test user feedback comment strings.');

            cy.contains('button', 'Save and continue').click();
            cy.urlPath().should('match', TASK_LIST_URL);
        });

        it('should block submission when nothing is selected', () => {
            cy.contains('button', 'Save and continue').click();

            cy.urlPath().should('not.match', TASK_LIST_URL);
        });
    });
});
