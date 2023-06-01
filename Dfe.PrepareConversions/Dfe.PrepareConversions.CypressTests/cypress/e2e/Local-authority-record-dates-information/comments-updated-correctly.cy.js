/// <reference types ='Cypress'/>
import ProjectList from '../../pages/projectList'

Cypress._.each(['ipad-mini'], (viewport) => {
    describe.skip(`86856 Comments should accept alphanumeric inputs on ${viewport}`, () => {
        beforeEach(() => {
            cy.login()

            cy.viewport(viewport)

            ProjectList.selectProject().then(id => {
                cy.sqlServer(`update 
                                academisation.Project set 
                                LocalAuthorityInformationTemplateReturnedDate = '2023-01-01', 
                                LocalAuthorityInformationTemplateSentDate = '2023-01-01'                             
                            where Id = ${id}`);
                cy.url().then(url => {
                    //changes the current URL
                    let modifiedUrl = url + '/confirm-local-authority-information-template-dates'
                    cy.visit(modifiedUrl)
                });
            });
        })

        it('TC01: Precondition comment box', () => {
            cy.get('[id="la-info-template-comments"]').should('be.visible')
                .invoke('text')
                .then((text) => {
                    if (text.includes('Empty')) {
                        return
                    }
                    else {
                        cy.commentBoxClearLaInfo()
                    }
                });
        });

        it('TC02: Navigates to comment section & type alphanumerical characters', () => {
            let alphanumeric = 'abcdefghijklmnopqrstuvwxyz 1234567890 !"£$%^&*(){}[]:@,./<>?~|'
            cy.get('[data-test="change-la-info-template-comments"]').click()
            cy.commentBoxLaInfo().type(alphanumeric)
            cy.saveAndContinueButton().click()
            cy.commentBoxLaInfo().should('contain', alphanumeric)
        });

        it('TC03: Clears text input', () => {
            cy.commentBoxClearLaInfo()
            cy.commentBoxLaInfo().should('contain', 'Empty')
        });
    });
});