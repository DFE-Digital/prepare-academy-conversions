/// <reference types ='Cypress'/>

import projectList from "../../pages/projectList";
import projectTaskList from "../../pages/projectTaskList";
import projectAssignment from "../../pages/projectAssignment";
import schoolOverview from "../../pages/schoolOverview";

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

      // School Overview
      projectList.selectProject(projectName);
      projectTaskList.selectSchoolOverview();
      schoolOverview.changePanNumber('98765');
      schoolOverview.getPan().should('contain.text', '98765');
      //Viability issues
      schoolOverview.changeViabilityIssues(true);
      schoolOverview.getViabilityIssues().should('contain.text', 'Yes');
      //Financial deficit
      
      //PFI + details
      //Distance plus details
      //MP
      
   })
})
