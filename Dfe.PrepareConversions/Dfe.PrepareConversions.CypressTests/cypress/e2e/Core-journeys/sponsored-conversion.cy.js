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


describe('Sponsored conversion journey', () => {

   const currentDate = new Date();
   const nextYearDate = new Date();
   nextYearDate.setFullYear(currentDate.getFullYear() + 1);
   const oneMonthAgoDate = new Date();
   oneMonthAgoDate.setMonth(currentDate.getMonth() - 1);
   const nextMonthDate = new Date();
   nextMonthDate.setMonth(currentDate.getMonth() + 1);

   const testData = {
      projectName: 'Sponsored Cypress Project',
      completedText: 'Completed',
      projectAssignment: {
         deliveryOfficer: 'Richika Dogra',
         assignedOfficerMessage: 'Project is assigned',
      },
      schoolOverview: {
         pan: '98765',
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
      keyStages: [4, 5]
   }

   before(() => {
     // Do the spin up a project journey
   })

   beforeEach(() => {
      projectList.selectProject(testData.projectName)
   })

   it('TC01: Assign Project', () => {
      projectTaskList.selectAssignProject();
      projectAssignment.assignProject(testData.projectAssignment.deliveryOfficer)
      projectTaskList.getNotificationMessage().should('contain.text', testData.projectAssignment.assignedOfficerMessage);
      projectTaskList.getAssignedUser().should('contain.text', testData.projectAssignment.deliveryOfficer);
      projectList.filterProjectList(testData.projectName);
      projectList.getNthProjectDeliveryOfficer(testData.projectAssignment.deliveryOfficer).should('contain.value', testData.projectAssignment.deliveryOfficer);
   })

   it('TC02: School Overview', () => {
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
      projectTaskList.getSchoolOverviewStatus().should('contain.text', testData.completedText);
   })

   it('TC03: Budget ', () => {
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
      projectTaskList.getBudgetStatus().should('contain.text', testData.completedText);
   });

   it('TC04: Pupil Forecast ', () => {
      projectTaskList.selectPupilForecast();
      PupilForecast.enterAditionalInfomation(testData.pupilForecast.additionalInfomation);
      PupilForecast.getAdditionalInfomation().should('contain.text', testData.pupilForecast.additionalInfomation);
      cy.confirmContinueBtn().click();
   });

   it('TC05: Conversion Details ', () => {
      projectTaskList.selectConversionDetails();

      // Form 7
      ConversionDetails.setForm7Receivied('Yes');
      ConversionDetails.getForm7Receivied().should('contain.text', 'Yes');
      ConversionDetails.setForm7Date(oneMonthAgoDate);
      ConversionDetails.getForm7Date().should('contain.text', oneMonthAgoDate.getDate());
      ConversionDetails.getForm7Date().should('contain.text', oneMonthAgoDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getForm7Date().should('contain.text', oneMonthAgoDate.getFullYear());

      // DAO
      ConversionDetails.setDAODate(oneMonthAgoDate);
      ConversionDetails.getDAODate().should('contain.text', oneMonthAgoDate.getDate());
      ConversionDetails.getDAODate().should('contain.text', oneMonthAgoDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getDAODate().should('contain.text', oneMonthAgoDate.getFullYear());

      // Funding
      ConversionDetails.setFundingType('Full');
      ConversionDetails.getFundingType().should('contain.text', 'Full');
      ConversionDetails.setFundingAmount(false, 100000);
      ConversionDetails.getFundingAmount().should('contain.text', '£100,000');
      ConversionDetails.setFundingReason();
      ConversionDetails.getFundingReason().should('contain.text', 'Funding Reason');

      //EIG
      ConversionDetails.setEIG(true);
      ConversionDetails.getEIG().should('contain.text', 'Yes');

      // Dates
      ConversionDetails.setAdvisoryBoardDate(nextMonthDate);
      ConversionDetails.getAdvisoryBoardDate().should('contain.text', nextMonthDate.getDate());
      ConversionDetails.getAdvisoryBoardDate().should('contain.text', nextMonthDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getAdvisoryBoardDate().should('contain.text', nextMonthDate.getFullYear());
      ConversionDetails.setProposedAcademyOpening(nextYearDate.toLocaleString('default', { month: '2-digit' }), nextYearDate.getFullYear());
      ConversionDetails.getProposedAcademyOpening().should('contain.text', 1);
      ConversionDetails.getProposedAcademyOpening().should('contain.text', nextYearDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getProposedAcademyOpening().should('contain.text', nextYearDate.getFullYear());
      ConversionDetails.setPreviousAdvisoryBoardDate(true, oneMonthAgoDate);
      ConversionDetails.getPreviousAdvisoryBoardDate().should('contain.text', oneMonthAgoDate.getDate());
      ConversionDetails.getPreviousAdvisoryBoardDate().should('contain.text', oneMonthAgoDate.toLocaleString('default', { month: 'long' }));
      ConversionDetails.getPreviousAdvisoryBoardDate().should('contain.text', oneMonthAgoDate.getFullYear());

      // Author
      ConversionDetails.setAuthor();
      ConversionDetails.getAuthor().should('contain.text', 'Nicholas Warms');
      ConversionDetails.setClearedBy();
      ConversionDetails.getClearedBy().should('contain.text', 'Nicholas Warms');

      // Complete
      ConversionDetails.markComplete();
      cy.confirmContinueBtn().click();
      projectTaskList.getConversionDetailsStatus().should('contain.text', testData.completedText);
   });

   it('TC06: Rationale ', () => {
      projectTaskList.selectRationale();
      Rationale.changeRationale(testData.rationale);
      Rationale.getRationale().should('contain.text', testData.rationale);
      Rationale.markComplete();
      cy.confirmContinueBtn().click();
      projectTaskList.getRationaleStatus().should('contain.text', testData.completedText);
   });

   it('TC07: Risks and issues ', () => {
      projectTaskList.selectRisksAndIssues();
      RisksAndIssues.changeRisksAndIssues(testData.risksAndIssues);
      RisksAndIssues.getRisksAndIssues().should('contain.text', testData.risksAndIssues);
      RisksAndIssues.markComplete();
      cy.confirmContinueBtn().click();
      projectTaskList.getRisksAndIssuesStatus().should('contain.text', testData.completedText);
   });

   it('TC08: Local authority ', () => {
      projectTaskList.selectLA();
      LocalAuthorityInfomation.changeTemplateDates(oneMonthAgoDate, nextMonthDate);
      LocalAuthorityInfomation.getTemplateSentDate().should('contain.text', oneMonthAgoDate.getDate());
      LocalAuthorityInfomation.getTemplateSentDate().should('contain.text', oneMonthAgoDate.toLocaleString('default', { month: 'long' }));
      LocalAuthorityInfomation.getTemplateSentDate().should('contain.text', oneMonthAgoDate.getFullYear());
      LocalAuthorityInfomation.getTemplateReturnedDate().should('contain.text', nextMonthDate.getDate());
      LocalAuthorityInfomation.getTemplateReturnedDate().should('contain.text', nextMonthDate.toLocaleString('default', { month: 'long' }));
      LocalAuthorityInfomation.getTemplateReturnedDate().should('contain.text', nextMonthDate.getFullYear());

      LocalAuthorityInfomation.changeComments(testData.localAuthority.comment);
      LocalAuthorityInfomation.getComments().should('contain.text', testData.localAuthority.comment);

      LocalAuthorityInfomation.changeSharepointLink(testData.localAuthority.sharepointLink);
      LocalAuthorityInfomation.getSharepointLink().should('contain.text', testData.localAuthority.sharepointLink);

      LocalAuthorityInfomation.markComplete();
      cy.confirmContinueBtn().click();
      projectTaskList.getLAStatus().should('contain.text', testData.completedText);

   });

   it('TC09: Performance Info ', () => {
      projectTaskList.selectOfsted();
      Performance.changeOfstedInfo(testData.performanceInfo);
      Performance.getOfstedInfo().should('contain.text', testData.performanceInfo);
      cy.confirmContinueBtn().click();

      for (const keyStage of testData.keyStages) {
         console.log(keyStage)
         projectTaskList.selectKeyStage(keyStage);
         Performance.changeKeyStageInfo(keyStage, testData.performanceInfo);
         Performance.getKeyStageInfo(keyStage).should('contain.text', testData.performanceInfo);
         cy.confirmContinueBtn().click();
      }
   });
})
