/// <reference types ='Cypress'/>

import projectList from "../../pages/projectList";
import projectTaskList from "../../pages/projectTaskList";
import projectAssignment from "../../pages/projectAssignment";
import schoolOverview from "../../pages/schoolOverview";
import budget from "../../pages/budget";
import PupilForecast from "../../pages/pupilForecast";
import ConversionDetails from "../../pages/conversionDetails";
import Rationale from "../../pages/rationale";
import RisksAndIssues from "../../pages/risksAndIssues";
import LocalAuthorityInfomation from "../../pages/localAuthorityInformation";
import Performance from "../../pages/performance";


describe('Sponsored conversion journey', { tags: ['@dev', '@stage'] }, () => {

   const currentDate = new Date();
   const nextYearDate = new Date();
   nextYearDate.setFullYear(currentDate.getFullYear() + 1);
   const oneMonthAgoDate = new Date();
   oneMonthAgoDate.setMonth(currentDate.getMonth() - 1);
   const nextMonthDate = new Date();
   nextMonthDate.setMonth(currentDate.getMonth() + 1);

   const testData = {
      projectName: 'Deanshanger Primary School',
      completedText: 'Completed',
      projectAssignment: {
         deliveryOfficer: 'Chris Sherlock',
         assignedOfficerMessage: 'Project is assigned',
      },
      schoolOverview: {
         pan: '999',
         pfiDescription: 'PFI Description',
         distance: '15',
         distanceDecription: 'Distance description',
         mp: 'Important Politician, Independent',
      },
      budget: {
         endOfFinanicalYear: currentDate,
         forecastedRevenueCurrentYear: 20,
         forecastedCapitalCurrentYear: 10,
         endOfNextFinancialYear: nextYearDate,
         forecastedRevenueNextYear: 15,
         forecastedCapitalNextYear: 12
      },
      pupilForecast: {
         additionalInfomation: 'Pupil Forecast Additional Information'
      },
      rationale: 'This is why this school should become an academy',
      risksAndIssues: 'Here are the risks and issues for this conversion',
      localAuthority: {
         comment: 'Comment',
         sharepointLink: 'https://sharepoint.com'
      },
      performanceInfo: 'Additional Information',
      keyStages: [2]
   }

   before(() => {
     // Do the spin up a project journey
   })

   beforeEach(() => {
      projectList.selectProject(testData.projectName)
   })

   it('TC01: Assign Project', () => {
      projectTaskList.selectAssignProject();
      cy.excuteAccessibilityTests();
      projectAssignment.assignProject(testData.projectAssignment.deliveryOfficer)
      cy.excuteAccessibilityTests();
      projectTaskList.getNotificationMessage().should('contain.text', testData.projectAssignment.assignedOfficerMessage);
      cy.excuteAccessibilityTests();
      projectTaskList.getAssignedUser().should('contain.text', testData.projectAssignment.deliveryOfficer);
      cy.excuteAccessibilityTests();
      projectList.filterProjectList(testData.projectName);
      cy.excuteAccessibilityTests();
      projectList.getNthProjectDeliveryOfficer().should('contain.text', testData.projectAssignment.deliveryOfficer);
   })

   it('TC02: School Overview', () => {
      projectList.selectProject(testData.projectName);
      cy.excuteAccessibilityTests();
      projectTaskList.selectSchoolOverview();
      cy.excuteAccessibilityTests();
      //PAN
      schoolOverview.changePan(testData.schoolOverview.pan);
      cy.excuteAccessibilityTests();
      schoolOverview.getPan().should('contain.text', testData.schoolOverview.pan);
      cy.excuteAccessibilityTests();
      //Viability issues
      schoolOverview.changeViabilityIssues(true);
      cy.excuteAccessibilityTests();
      schoolOverview.getViabilityIssues().should('contain.text', 'Yes');
      cy.excuteAccessibilityTests();
      schoolOverview.changeViabilityIssues(false);
      schoolOverview.getViabilityIssues().should('contain.text', 'No');
      //Financial deficit
      schoolOverview.changeFinancialDeficit(true);
      cy.excuteAccessibilityTests();
      schoolOverview.getFinancialDeficit().should('contain.text', 'Yes');
      cy.excuteAccessibilityTests();
      schoolOverview.changeFinancialDeficit(false);
      schoolOverview.getFinancialDeficit().should('contain.text', 'No');
      cy.excuteAccessibilityTests();
      //PFI + details
      cy.excuteAccessibilityTests();
      schoolOverview.changePFI(true, testData.schoolOverview.pfiDescription);
      schoolOverview.getPFI().should('contain.text', 'Yes');
      cy.excuteAccessibilityTests();
      schoolOverview.getPFIDetails().should('contain.text', testData.schoolOverview.pfiDescription);
      //Distance plus details
      schoolOverview.changeDistance(testData.schoolOverview.distance, testData.schoolOverview.distanceDecription);
      cy.excuteAccessibilityTests();
      schoolOverview.getDistance().should('contain.text', testData.schoolOverview.distance);
      cy.excuteAccessibilityTests();
      schoolOverview.getDistanceDetails().should('contain.text', testData.schoolOverview.distanceDecription);
      //MP
      schoolOverview.changeMP(testData.schoolOverview.mp);
      cy.excuteAccessibilityTests();
      schoolOverview.getMP().should('contain.text', testData.schoolOverview.mp);
      cy.excuteAccessibilityTests();
      //Complete
      schoolOverview.markComplete();
      cy.excuteAccessibilityTests();
      cy.confirmContinueBtn().click();
      projectTaskList.getSchoolOverviewStatus().should('contain.text', testData.completedText);
      cy.excuteAccessibilityTests();
   })

   it('TC03: Budget ', () => {
      projectTaskList.selectBudget();
      cy.excuteAccessibilityTests();
      budget.updateBudgetInfomation(testData.budget);

      budget.getCurrentFinancialYear().should('contain.text', testData.budget.endOfFinanicalYear.getDate());
      cy.excuteAccessibilityTests();
      budget.getCurrentFinancialYear().should('contain.text', testData.budget.endOfFinanicalYear.toLocaleString('default', { month: 'long' }));
      cy.excuteAccessibilityTests();
      budget.getCurrentFinancialYear().should('contain.text', testData.budget.endOfFinanicalYear.getFullYear());
      cy.excuteAccessibilityTests();
      budget.getCurrentRevenue().should('contain.text', `£${testData.budget.forecastedRevenueCurrentYear}`);
      budget.getCurrentCapital().should('contain.text', `£${testData.budget.forecastedCapitalCurrentYear}`);

      budget.getNextFinancialYear().should('contain.text', testData.budget.endOfNextFinancialYear.getDate());
      cy.excuteAccessibilityTests();
      budget.getNextFinancialYear().should('contain.text', testData.budget.endOfNextFinancialYear.toLocaleString('default', { month: 'long' }));
      cy.excuteAccessibilityTests();
      budget.getNextFinancialYear().should('contain.text', testData.budget.endOfNextFinancialYear.getFullYear());
      budget.getNextRevenue().should('contain.text', `£${testData.budget.forecastedRevenueNextYear}`);
      cy.excuteAccessibilityTests();
      budget.getNextCapital().should('contain.text', `£${testData.budget.forecastedCapitalNextYear}`);

      budget.markComplete();
      cy.excuteAccessibilityTests();
      cy.confirmContinueBtn().click();
      cy.excuteAccessibilityTests();
      projectTaskList.getBudgetStatus().should('contain.text', testData.completedText);
   });

   it('TC04: Pupil Forecast ', () => {
      projectTaskList.selectPupilForecast();
      cy.excuteAccessibilityTests();
      PupilForecast.enterAditionalInfomation(testData.pupilForecast.additionalInfomation);
      cy.excuteAccessibilityTests();
      PupilForecast.getAdditionalInfomation().should('contain.text', testData.pupilForecast.additionalInfomation);
      cy.confirmContinueBtn().click();
      cy.excuteAccessibilityTests();
   });

   it('TC05: Conversion Details ', () => {
      projectTaskList.selectConversionDetails();
      cy.excuteAccessibilityTests();

      // Form 7
      ConversionDetails.setForm7Receivied('Yes');
      cy.excuteAccessibilityTests();
      ConversionDetails.getForm7Receivied().should('contain.text', 'Yes');
      ConversionDetails.setForm7Date(oneMonthAgoDate);
      ConversionDetails.getForm7Date().should('contain.text', oneMonthAgoDate.getDate());
      cy.excuteAccessibilityTests();
      ConversionDetails.getForm7Date().should('contain.text', oneMonthAgoDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getForm7Date().should('contain.text', oneMonthAgoDate.getFullYear());
      cy.excuteAccessibilityTests();

      // DAO
      ConversionDetails.setDAODate(oneMonthAgoDate);
      ConversionDetails.getDAODate().should('contain.text', oneMonthAgoDate.getDate());
      cy.excuteAccessibilityTests();
      ConversionDetails.getDAODate().should('contain.text', oneMonthAgoDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getDAODate().should('contain.text', oneMonthAgoDate.getFullYear());

      // Funding
      ConversionDetails.setFundingType('Full');
      cy.excuteAccessibilityTests();
      ConversionDetails.getFundingType().should('contain.text', 'Full');
      ConversionDetails.setFundingAmount(false, 100000);
      ConversionDetails.getFundingAmount().should('contain.text', '£100,000');
      ConversionDetails.setFundingReason();
      cy.excuteAccessibilityTests();
      ConversionDetails.getFundingReason().should('contain.text', 'Funding Reason');

      //EIG
      ConversionDetails.setEIG(true);
      cy.excuteAccessibilityTests();
      ConversionDetails.getEIG().should('contain.text', 'Yes');

      // Dates
      ConversionDetails.setAdvisoryBoardDate(nextMonthDate);
      cy.excuteAccessibilityTests();
      ConversionDetails.getAdvisoryBoardDate().should('contain.text', nextMonthDate.getDate());
      cy.excuteAccessibilityTests();
      ConversionDetails.getAdvisoryBoardDate().should('contain.text', nextMonthDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getAdvisoryBoardDate().should('contain.text', nextMonthDate.getFullYear());
      ConversionDetails.setProposedAcademyOpening(nextYearDate.toLocaleString('default', { month: '2-digit' }), nextYearDate.getFullYear());
      ConversionDetails.getProposedAcademyOpening().should('contain.text', 1);
      ConversionDetails.getProposedAcademyOpening().should('contain.text', nextYearDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getProposedAcademyOpening().should('contain.text', nextYearDate.getFullYear());
      cy.excuteAccessibilityTests();
      ConversionDetails.setPreviousAdvisoryBoardDate(true, oneMonthAgoDate);
      ConversionDetails.getPreviousAdvisoryBoardDate().should('contain.text', oneMonthAgoDate.getDate());
      ConversionDetails.getPreviousAdvisoryBoardDate().should('contain.text', oneMonthAgoDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getPreviousAdvisoryBoardDate().should('contain.text', oneMonthAgoDate.getFullYear());
      cy.excuteAccessibilityTests();

      // Author
      ConversionDetails.setAuthor();
      cy.excuteAccessibilityTests();
      ConversionDetails.getAuthor().should('contain.text', 'Nicholas Warms');
      ConversionDetails.setClearedBy();
      ConversionDetails.getClearedBy().should('contain.text', 'Nicholas Warms');
      cy.excuteAccessibilityTests();
      // Complete
      ConversionDetails.markComplete();
      cy.excuteAccessibilityTests();
      cy.confirmContinueBtn().click();
      cy.excuteAccessibilityTests();
      projectTaskList.getConversionDetailsStatus().should('contain.text', testData.completedText);
   });

   it('TC06: Rationale ', () => {
      projectTaskList.selectRationale();
      Rationale.changeRationale(testData.rationale);
      cy.excuteAccessibilityTests();
      Rationale.getRationale().should('contain.text', testData.rationale);
      Rationale.markComplete();
      cy.confirmContinueBtn().click();
      cy.excuteAccessibilityTests();
      projectTaskList.getRationaleStatus().should('contain.text', testData.completedText);
   });

   it('TC07: Risks and issues ', () => {
      projectTaskList.selectRisksAndIssues();
      cy.excuteAccessibilityTests();
      RisksAndIssues.changeRisksAndIssues(testData.risksAndIssues);
      RisksAndIssues.getRisksAndIssues().should('contain.text', testData.risksAndIssues);
      RisksAndIssues.markComplete();
      cy.excuteAccessibilityTests();
      cy.confirmContinueBtn().click();
      cy.excuteAccessibilityTests();
      projectTaskList.getRisksAndIssuesStatus().should('contain.text', testData.completedText);
   });

   it('TC08: Local authority ', () => {
      projectTaskList.selectLA();
      LocalAuthorityInfomation.changeTemplateDates(oneMonthAgoDate, nextMonthDate);
      cy.excuteAccessibilityTests();
      LocalAuthorityInfomation.getTemplateSentDate().should('contain.text', oneMonthAgoDate.getDate());
      LocalAuthorityInfomation.getTemplateSentDate().should('contain.text', oneMonthAgoDate.toLocaleString('default', { month: 'long' }));
      cy.excuteAccessibilityTests();
      LocalAuthorityInfomation.getTemplateSentDate().should('contain.text', oneMonthAgoDate.getFullYear());
      LocalAuthorityInfomation.getTemplateReturnedDate().should('contain.text', nextMonthDate.getDate());
      cy.excuteAccessibilityTests();
      LocalAuthorityInfomation.getTemplateReturnedDate().should('contain.text', nextMonthDate.toLocaleString('default', { month: 'long' }));
      LocalAuthorityInfomation.getTemplateReturnedDate().should('contain.text', nextMonthDate.getFullYear());
      cy.excuteAccessibilityTests();
      LocalAuthorityInfomation.changeComments(testData.localAuthority.comment);
      LocalAuthorityInfomation.getComments().should('contain.text', testData.localAuthority.comment);
      cy.excuteAccessibilityTests();
      LocalAuthorityInfomation.changeSharepointLink(testData.localAuthority.sharepointLink);
      LocalAuthorityInfomation.getSharepointLink().should('contain.text', testData.localAuthority.sharepointLink);

      LocalAuthorityInfomation.markComplete();
      cy.confirmContinueBtn().click();
      cy.excuteAccessibilityTests();
      projectTaskList.getLAStatus().should('contain.text', testData.completedText);

   });

   it('TC09: Performance Info ', () => {
      projectTaskList.selectOfsted();
      Performance.changeOfstedInfo(testData.performanceInfo);
      cy.excuteAccessibilityTests();
      Performance.getOfstedInfo().should('contain.text', testData.performanceInfo);
      cy.confirmContinueBtn().click();
      cy.excuteAccessibilityTests();

      for (const keyStage of testData.keyStages) {
         console.log(keyStage)
         projectTaskList.selectKeyStage(keyStage);
         cy.excuteAccessibilityTests();
         Performance.changeKeyStageInfo(keyStage, testData.performanceInfo);
         Performance.getKeyStageInfo(keyStage).should('contain.text', testData.performanceInfo);
         cy.confirmContinueBtn().click();
         cy.excuteAccessibilityTests();
      }
   });

 
});
