/// <reference types ='Cypress'/>

import projectList from "../../pages/projectList";
import projectTaskList from "../../pages/projectTaskList";
import projectAssignment from "../../pages/projectAssignment";
import schoolOverview from "../../pages/schoolOverview";
import budget from "../../pages/budget";
import ProjectTaskList from "../../pages/projectTaskList";
import PupilForecast from "../../pages/pupilForecast";

const currentYear = new Date();
const nextYear = new Date();
nextYear.setFullYear(new Date().getFullYear() + 1);


const testData = {
   projectName: 'Sponsored Cypress Project',
   projectAssignment: {
      deliveryOfficer: 'Richika Dogra',
      assignedOfficerMessage: 'Project is assigned',
   },
   schoolOverview: {
      pan: '98765',
      pfiDescription: 'PFI Description',
      distance: '15',
      distanceDecription: 'Distance description',
      mp: 'Important Politician, Indepentent',
   },
   budget: {
      endOfFinanicalYear: currentYear,
      forecastedRevenueCurrentYear: 20,
      forecastedCapitalCurrentYear: 10,
      endOfNextFinancialYear: nextYear,
      forecastedRevenueNextYear: 15,
      forecastedCapitalNextYear: 12
   },
   pupilForecast: {
      additionalInfomation: 'Pupil Forecast Additional Infomation'
   }
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
      projectAssignment.assignProject(testData.projectAssignment.deliveryOfficer)
      projectTaskList.getNotificationMessage().should('contain.text', testData.projectAssignment.assignedOfficerMessage);
      projectTaskList.getAssignedUser().should('contain.text', testData.projectAssignment.deliveryOfficer);
      projectList.filterProjectList(testData.projectName);
      projectList.getNthProjectDeliveryOfficer().should('contain.text', testData.projectAssignment.deliveryOfficer);

      // -------------------
      // - School Overview -
      // -------------------

      projectList.selectProject(testData.projectName);
      projectTaskList.selectSchoolOverview();
      //PAN
      schoolOverview.changePan(testData.schoolOverview.pan);
      schoolOverview.getPan().should('contain.text', testData.schoolOverview.pan);
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
      schoolOverview.changePFI(true, testData.schoolOverview.pfiDescription);
      schoolOverview.getPFI().should('contain.text', 'Yes');
      schoolOverview.getPFIDetails().should('contain.text', testData.schoolOverview.pfiDescription);
      //Distance plus details
      schoolOverview.changeDistance(testData.schoolOverview.distance, testData.schoolOverview.distanceDecription);
      schoolOverview.getDistance().should('contain.text', testData.schoolOverview.distance);
      schoolOverview.getDistanceDetails().should('contain.text', testData.schoolOverview.distanceDecription);
      //MP
      schoolOverview.changeMP(testData.schoolOverview.mp);
      schoolOverview.getMP().should('contain.text', testData.schoolOverview.mp);
      //Complete
      schoolOverview.markComplete();
      cy.confirmContinueBtn().click();
      projectTaskList.getSchoolOverviewStatus().should('contain.text', 'Completed');

      // ----------
      // - Budget -
      // ----------

      projectTaskList.selectBudget();
      budget.updateBudgetInfomation(testData.budget);

      budget.getCurrentFinancialYear().should('contain.text', testData.budget.endOfFinanicalYear.getDate());
      budget.getCurrentFinancialYear().should('contain.text', testData.budget.endOfFinanicalYear.toLocaleString('default', { month: 'long' }));
      budget.getCurrentFinancialYear().should('contain.text', testData.budget.endOfFinanicalYear.getFullYear());
      budget.getCurrentRevenue().should('contain.text', `£${testData.budget.forecastedRevenueCurrentYear}`);
      budget.getCurrentCapital().should('contain.text', `£${testData.budget.forecastedCapitalCurrentYear}`);

      budget.getNextFinancialYear().should('contain.text', testData.budget.endOfNextFinancialYear.getDate());
      budget.getNextFinancialYear().should('contain.text', testData.budget.endOfNextFinancialYear.toLocaleString('default', { month: 'long' }));
      budget.getNextFinancialYear().should('contain.text', testData.budget.endOfNextFinancialYear.getFullYear());
      budget.getNextRevenue().should('contain.text', `£${testData.budget.forecastedRevenueNextYear}`);
      budget.getNextCapital().should('contain.text', `£${testData.budget.forecastedCapitalNextYear}`);

      budget.markComplete();
      cy.confirmContinueBtn().click();
      projectTaskList.getBudgetStatus().should('contain.text', 'Completed');


      // ------------------
      // - Pupil Forecast -
      // ------------------

      projectTaskList.selectPupilForecast();
      PupilForecast.enterAditionalInfomation(testData.pupilForecast.additionalInfomation);
      PupilForecast.getAdditionalInfomation().should('contain.text', testData.pupilForecast.additionalInfomation);
      cy.confirmContinueBtn().click();

   })
})
