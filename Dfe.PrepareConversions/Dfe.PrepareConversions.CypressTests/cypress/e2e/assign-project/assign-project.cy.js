/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
    describe(`Assign project to user ${viewport}`, () => {
        beforeEach(() => {
            cy.selectFirstProject();
        })

        it('TC01: Assign a project to user for the first time', () => {
            cy.unassignUser();
            cy.get('a[href*="project-assignment"]').click();
            cy.get('[id="delivery-officer"]').click().type('Richika Dogra').type('{enter}');
            cy.get('[class="govuk-button"]').click();
            cy.get('[id="notification-message"]').should('contain.text', 'Project is assigned');
            cy.selectsConversion();
            cy.get('[id="delivery-officer-0"]').should('contain.text', 'Richika Dogra');
        });

        it('TC02: Remove assigned user by unassigning', () => {
            cy.get('a[href*="project-assignment"]').click();
            cy.get('[id="unassign-link"]').click();
            cy.get('[id="notification-message"]').should('contain.text', 'Project is unassigned');
        });

        it('TC03: Remove assigned user via keyboard delete', () => {
            cy.assignUser();
            cy.get('a[href*="project-assignment"]').click();
            cy.get('[id="delivery-officer"]').click().type('{backspace}');
            cy.get('[class="govuk-button"]').click();
        });

        it('TC04: Click on the continue button while an assigned user is assigned', () => {
            cy.assignUser();
            cy.get('a[href*="project-assignment"]').click();
            cy.get('[class="govuk-button"]').click();
            cy.get('[id="notification-message"]').should('contain.text', 'Project is assigned');
        });

        it('TC05: Click on the unassigned button with no assigned user', () => {
            cy.unassignUser();
            cy.get('a[href*="project-assignment"]').click();
            cy.get('[id="unassign-link"]');
            cy.get('[class="govuk-button"]').click();
        });

        it('TC06: Project list is updated accordingly with unassigned', () => {
            cy.unassignUser();
            cy.selectsConversion();
            cy.get('[id="delivery-officer-0"]').should('contain.text', 'Empty')
        });
    });
});    