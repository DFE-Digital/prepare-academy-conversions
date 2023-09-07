/// <reference types ='Cypress'/>
import projectList from '../../pages/projectList'
import voluntaryProjectTaskList from '../../pages/voluntaryProjectTaskList'

var projectName = 'Voluntary Cypress Project'

describe('Voluntary conversion', { tags: ['@dev', '@stage'] }, () => {
   beforeEach(() => {
      cy.callAcademisationApi('POST', `cypress-data/add-voluntary-project.cy`, "{}")
         .then(() => {
            projectList.selectProject(projectName)
         });
   })

   it('TC01: Voluntary conversion journey ', () => {
    // Go wild Dan!
   voluntaryProjectTaskList.voluntaryProjectElementsVisible()

   })
})