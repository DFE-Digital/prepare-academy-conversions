/// <reference types ='Cypress'/>

describe('Form a MAT conversion', { tags: ['@dev', '@stage'] }, () => {
   beforeEach(() => {
      cy.callAcademisationApi('POST', `cypress-data/add-form-a-mat-project.cy`, "{}")
         .then(() => {
            cy.login({ titleFilter: 'Cypress Project' })
               .then(() => {
                  cy.get('[id="school-name-0"]').click();                  
               });
         });
   })

   it('TC01: Form a MAT conversion journey ', () => {
    // Go wild Dan!
   })
})