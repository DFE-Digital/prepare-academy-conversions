/// <reference types ='Cypress'/>

describe('Voluntary conversion', { tags: ['@dev', '@stage'] }, () => {
   beforeEach(() => {
      cy.callAcademisationApi('POST', `cypress-data/add-voluntary-project.cy`, "{}")
         .then(() => {
            cy.login({ titleFilter: 'Cypress Project' })
               .then(() => {
                  cy.get('[id="school-name-0"]').click();                  
               });
         });
   })

   it('TC01: Voluntary conversion journey ', () => {
    // Go wild Dan!
    cy.log("Hello World!")
   })
})