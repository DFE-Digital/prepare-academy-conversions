/// <reference types ='Cypress'/>

describe('Sponsored conversion', { tags: ['@dev', '@stage'] }, () => {
   beforeEach(() => {
      cy.callAcademisationApi('POST', `cypress-data/add-sponsored-project.cy`, "{}")
         .then(() => {
            cy.login({ titleFilter: 'Cypress Project' })
               .then(() => {
                  cy.get('[id="school-name-0"]').click();                  
               });
         });
   })

   it('TC01: Sponsored conversion journey ', () => {
    // Go wild Dan!
   })
})