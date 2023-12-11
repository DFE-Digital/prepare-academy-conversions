/// <reference types ='Cypress'/>
import projectList from '../../pages/projectList'
import voluntaryProjectTaskList from '../../pages/voluntaryProjectTaskList'

var projectName = 'Voluntary Cypress Project'

describe('Voluntary conversion', { tags: ['@dev', '@stage'] }, () => {
   before(() => {
      // Do the spin up a project journey
   })

   it('TC01: Voluntary conversion journey ', () => {
    // Go wild Dan!
      voluntaryProjectTaskList.voluntaryProjectElementsVisible()

   })
})