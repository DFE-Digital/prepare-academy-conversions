/// <reference types ='Cypress'/>

import projectList from "../../pages/projectList";
import projectTaskList from "../../pages/projectTaskList";
import projectAssignment from "../../pages/projectAssignment";
import schoolOverview from "../../pages/schoolOverview";
import budget from "../../pages/budget";


const testData = {
   projectName: 'Sponsored Cypress Project',
   deliveryOfficer: 'Richika Dogra',
   assignedOfficerMessage: 'Project is assigned',
   pan: '98765',
   pfiDescription: 'PFI Description',
   distance: '15',
   distanceDecription: 'Distance description',
   mp: 'Important Politician, Indepentent',
}

describe('Sponsored conversion', { tags: ['@dev', '@stage'] }, () => {
   beforeEach(() => {
      cy.callAcademisationApi('POST', `cypress-data/add-sponsored-project.cy`, "{}")
         .then(() => {
            projectList.selectProject(testData.projectName)
         });
   })

   it('TC01: Sponsored conversion journey ', () => {
      // ---------------------------
      // - Assign Delivery Officer -
      // ---------------------------

      projectTaskList.selectAssignProject();
      projectAssignment.assignProject(testData.deliveryOfficer)
      projectTaskList.getNotificationMessage().should('contain.text', 'Project is assigned');
      projectTaskList.getAssignedUser().should('contain.text', testData.deliveryOfficer);
      projectList.filterProjectList(testData.projectName);
      projectList.getNthProjectDeliveryOfficer().should('contain.text', testData.deliveryOfficer);

      // -------------------
      // - School Overview -
      // -------------------

      projectList.selectProject(testData.projectName);
      projectTaskList.selectSchoolOverview();
      //PAN
      schoolOverview.changePan(testData.pan);
      schoolOverview.getPan().should('contain.text', testData.pan);
      //Viability issues
      schoolOverview.changeViabilityIssues(true);
      schoolOverview.getViabilityIssues().should('contain.text', 'Yes');
      schoolOverview.changeViabilityIssues(false);
      schoolOverview.getViabilityIssues().should('contain.text', 'No');
      //Financial deficit
      schoolOverview.changeFinancialDeficit(true);
      schoolOverview.getFinancialDeficit().should('contain.text', 'Yes');
      schoolOverview.changeFinancialDeficit(false);
      schoolOverview.getFinancialDeficit().should('contain.text', 'No');
      //PFI + details
      schoolOverview.changePFI(true, testData.pfiDescription);
      schoolOverview.getPFI().should('contain.text', 'Yes');
      schoolOverview.getPFIDetails().should('contain.text', testData.pfiDescription);
      //Distance plus details
      schoolOverview.changeDistance(testData.distance, testData.distanceDecription);
      schoolOverview.getDistance().should('contain.text', testData.distance);
      schoolOverview.getDistanceDetails().should('contain.text', testData.distanceDecription);
      //MP
      schoolOverview.changeMP(testData.mp);
      schoolOverview.getMP().should('contain.text', testData.mp);
      //Complete
      schoolOverview.markComplete();
      cy.confirmContinueBtn().click();
      projectTaskList.getSchoolOverviewStatus().should('contain.text', 'Completed');

      // ----------
      // - Budget -
      // ----------

      projectTaskList.selectBudget();
      budget.updateBudgetInfomation();
   })
})
