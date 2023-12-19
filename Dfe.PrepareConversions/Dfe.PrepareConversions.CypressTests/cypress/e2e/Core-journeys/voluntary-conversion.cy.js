/// <reference types ='Cypress'/>
import voluntaryProjectTaskList from '../../pages/voluntaryProjectTaskList'

describe('Voluntary conversion', { tags: ['@dev', '@stage'] }, () => {
   before(() => {
      // Do the spin up a project journey
   })

   it('TC01: Voluntary conversion journey ', () => {
    // Go wild Dan!
      voluntaryProjectTaskList.voluntaryProjectElementsVisible()

   })
})