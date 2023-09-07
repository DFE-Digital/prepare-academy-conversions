/// <reference types ='Cypress'/>
import projectList from '../../pages/projectList'

var projectName = 'Voluntary Cypress Project'

describe('Voluntary conversion', { tags: ['@dev', '@stage'] }, () => {
   beforeEach(() => {
      cy.callAcademisationApi('POST', `cypress-data/add-voluntary-project.cy`, "{}")
         .then(() => {
            projectList.selectVoluntaryProject(projectName)
         });
   })

   it('TC01: Voluntary conversion journey ', () => {
    // Go wild Dan!


   })
})