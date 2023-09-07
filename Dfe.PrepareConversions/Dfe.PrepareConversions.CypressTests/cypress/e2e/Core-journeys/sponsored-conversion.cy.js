/// <reference types ='Cypress'/>

import projectList from "../../pages/projectList";
import projectTaskList from "../../pages/projectTaskList";
import projectAssignment from "../../pages/projectAssignment";

const projectName = 'Sponsored Cypress Project';
const deliveryOfficer = 'Richika Dogra';

describe('Sponsored conversion', { tags: ['@dev', '@stage'] }, () => {
   beforeEach(() => {
      cy.callAcademisationApi('POST', `cypress-data/add-sponsored-project.cy`, "{}")
         .then(() => {
            projectList.selectProject(projectName)
         });
   })

   it('TC01: Sponsored conversion journey ', () => {
      // Assign Delivery Officer

      projectTaskList.selectAssignProject();
      projectAssignment.assignProject(deliveryOfficer)
      projectTaskList.getNotificationMessage().should('contain.text', 'Project is assigned');
      projectTaskList.getAssignedUser().should('contain.text', deliveryOfficer);
      projectList.filterProjectList(projectName);
      projectList.getNthProjectDeliveryOfficer().should('contain.text', deliveryOfficer);
   })
})
