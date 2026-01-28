/// <reference types="cypress" />

import projectList from '../../pages/projectList';
import projectTaskList from '../../pages/projectTaskList';
import projectAssignmentConversion from '../../pages/projectAssignmentConversion';
import schoolOverview from '../../pages/schoolOverview';
import budget from '../../pages/budget';
import PupilForecast from '../../pages/pupilForecast';
import ConversionDetails from '../../pages/conversionDetails';
import Rationale from '../../pages/rationaleConversion';
import RisksAndIssues from '../../pages/risksAndIssues';
import LocalAuthorityInfomation from '../../pages/localAuthorityInformation';
import Performance from '../../pages/performance';
import PublicSectorEqualityDutyImpact from '../../pages/publicSectorEqualityDutyImpact';
import { currentDate, nextMonthDate, nextYearDate, oneMonthAgoDate } from '../../constants/testConstants';

describe('Sponsored conversion journey', {}, () => {
    const testData = {
        projectName: 'Deanshanger Primary School',
        completedText: 'Completed',
        projectAssignment: {
            deliveryOfficer: 'Richika Dogra',
            assignedOfficerMessage: 'Project is assigned',
        },
        schoolOverview: {
            pan: '999',
            pfiDescription: 'PFI Description',
            distance: '15',
            distanceDecription: 'Distance description',
            mp: 'Sarah Bool',
        },
        budget: {
            endOfFinanicalYear: currentDate,
            forecastedRevenueCurrentYear: 20,
            forecastedCapitalCurrentYear: 10,
            endOfNextFinancialYear: nextYearDate,
            forecastedRevenueNextYear: 15,
            forecastedCapitalNextYear: 12,
        },
        pupilForecast: {
            additionalInfomation: 'Pupil Forecast Additional Information',
        },
        psed: {
            unlikely: 'The decision is unlikely to disproportionately affect any particular person ',
            someImpact: 'There are some impacts but on balance the analysis indicates these changes',
            likely: 'The decision is likely to disproportionately affect',
            reason: 'Test PSED reason',
            errorMessage: 'Consider the Public Sector Equality Duty',
            reasonErrorMessage: 'Decide what will be done to reduce the impact',
        },
        rationale: 'This is why this school should become an academy',
        risksAndIssues: 'Here are the risks and issues for this conversion',
        localAuthority: {
            comment: 'Comment',
            sharepointLink: 'https://sharepoint.com',
        },
        keyStages: [2],
    };

    before(() => {
        // Do the spin up a project journey
    });

    beforeEach(() => {
        cy.visit('/');
        cy.acceptCookies();
        projectList.selectProject(testData.projectName);
    });

    it('User could Assign Project', () => {
        projectTaskList.selectAssignProject();
        projectAssignmentConversion.assignProject(testData.projectAssignment.deliveryOfficer);
        projectTaskList
            .getNotificationMessage()
            .should('contain.text', testData.projectAssignment.assignedOfficerMessage);
        projectTaskList.getAssignedUser().should('contain.text', testData.projectAssignment.deliveryOfficer);
        projectList.filterProjectList(testData.projectName);
        projectList.getNthProjectDeliveryOfficer().should('contain.text', testData.projectAssignment.deliveryOfficer);
    });

    it('User could complete the School Overview task ', () => {
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
        schoolOverview.getFinancialDeficit().should('contain.text', 'No');
        //PFI + details
        schoolOverview.changePFI(true, testData.schoolOverview.pfiDescription);
        schoolOverview.getPFI().should('contain.text', 'Yes');
        schoolOverview.getPFIDetails().should('contain.text', testData.schoolOverview.pfiDescription);
        //Distance plus details
        schoolOverview.changeDistance(testData.schoolOverview.distance, testData.schoolOverview.distanceDecription);
        schoolOverview.getDistance().should('contain.text', testData.schoolOverview.distance);
        schoolOverview.getDistanceDetails().should('contain.text', testData.schoolOverview.distanceDecription);
        //Complete
        schoolOverview.markComplete();
        cy.confirmContinueBtn().click();
        projectTaskList.getSchoolOverviewStatus().should('contain.text', testData.completedText);
    });

    it('User could complete Budget task ', () => {
        projectTaskList.selectBudget();
        budget.updateBudgetInfomation(testData.budget);

        budget.getCurrentFinancialYear().should('contain.text', testData.budget.endOfFinanicalYear.getDate());
        budget.getCurrentFinancialYear().should(
            'contain.text',
            testData.budget.endOfFinanicalYear.toLocaleString('default', {
                month: 'long',
            })
        );
        budget.getCurrentFinancialYear().should('contain.text', testData.budget.endOfFinanicalYear.getFullYear());
        budget.getCurrentRevenue().should('contain.text', `£${testData.budget.forecastedRevenueCurrentYear}`);
        budget.getCurrentCapital().should('contain.text', `£${testData.budget.forecastedCapitalCurrentYear}`);

        budget.getNextFinancialYear().should('contain.text', testData.budget.endOfNextFinancialYear.getDate());
        budget.getNextFinancialYear().should(
            'contain.text',
            testData.budget.endOfNextFinancialYear.toLocaleString('default', {
                month: 'long',
            })
        );
        budget.getNextFinancialYear().should('contain.text', testData.budget.endOfNextFinancialYear.getFullYear());
        budget.getNextRevenue().should('contain.text', `£${testData.budget.forecastedRevenueNextYear}`);
        budget.getNextCapital().should('contain.text', `£${testData.budget.forecastedCapitalNextYear}`);

        budget.markComplete();
        cy.confirmContinueBtn().click();
        projectTaskList.getBudgetStatus().should('contain.text', testData.completedText);
    });

    it('User could complete Pupil Forecast task', () => {
        projectTaskList.selectPupilForecast();
        PupilForecast.enterAditionalInfomation(testData.pupilForecast.additionalInfomation);
        PupilForecast.getAdditionalInfomation().should('contain.text', testData.pupilForecast.additionalInfomation);
        cy.confirmContinueBtn().click();
    });

    it('User could complete Conversion Details task', () => {
        projectTaskList.selectConversionDetails();

        // Form 7
        ConversionDetails.setForm7Receivied('Yes');
        ConversionDetails.getForm7Receivied().should('contain.text', 'Yes');
        ConversionDetails.setForm7Date(oneMonthAgoDate);
        ConversionDetails.getForm7Date().should('contain.text', oneMonthAgoDate.getDate());
        ConversionDetails.getForm7Date().should(
            'contain.text',
            oneMonthAgoDate.toLocaleString('default', { month: 'long' })
        );
        ConversionDetails.getForm7Date().should('contain.text', oneMonthAgoDate.getFullYear());

        // DAO
        ConversionDetails.setDAODate(oneMonthAgoDate);
        ConversionDetails.getDAODate().should('contain.text', oneMonthAgoDate.getDate());
        ConversionDetails.getDAODate().should(
            'contain.text',
            oneMonthAgoDate.toLocaleString('default', { month: 'long' })
        );
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

    it('User gets an error message whilst previewing or creating project document for incomplete PSED task', () => {
        projectTaskList.selectPublicSectorEqualityDuty();
        PublicSectorEqualityDutyImpact.markIncomplete();
        cy.confirmContinueBtn().click();
        projectTaskList.clickPreviewProjectDocumentButton();
        projectTaskList.getErrorMessage().should('include.text', testData.psed.errorMessage);
        projectTaskList.clickCreateProjectDocumentButton();
        projectTaskList.getErrorMessage().should('include.text', testData.psed.errorMessage);
    });

    it('User could complete Public Sector Equality Duty task for Unlikely Impact', () => {
        projectTaskList.selectPublicSectorEqualityDuty();
        PublicSectorEqualityDutyImpact.changePsed('Unlikely');
        PublicSectorEqualityDutyImpact.getPsed().should('include.text', testData.psed.unlikely);
        PublicSectorEqualityDutyImpact.markComplete();
        cy.confirmContinueBtn().click();
        projectTaskList.publicSectorEqualityDutyStatus().should('contain.text', testData.completedText);
    });

    it('User could complete Public Sector Equality Duty task for Some Impact', () => {
        projectTaskList.selectPublicSectorEqualityDuty();
        PublicSectorEqualityDutyImpact.changePsed('Some impact');
        //should get an error if impact reason not provided
        PublicSectorEqualityDutyImpact.clickSaveBtnWithoutReason();
        PublicSectorEqualityDutyImpact.getErrorMessage().should('include.text', testData.psed.reasonErrorMessage);
        //should be able to save with impact reason
        PublicSectorEqualityDutyImpact.enterImpactReason();
        PublicSectorEqualityDutyImpact.getPsed().should('include.text', testData.psed.someImpact);
        PublicSectorEqualityDutyImpact.getImpactReason().should('include.text', testData.psed.reason);
        PublicSectorEqualityDutyImpact.markComplete();
        cy.confirmContinueBtn().click();
        projectTaskList.publicSectorEqualityDutyStatus().should('contain.text', testData.completedText);
    });

    it('User could complete Public Sector Equality Duty task for Likely Impact', () => {
        projectTaskList.selectPublicSectorEqualityDuty();
        PublicSectorEqualityDutyImpact.changePsed('Likely');
        PublicSectorEqualityDutyImpact.enterImpactReason();
        PublicSectorEqualityDutyImpact.getPsed().should('include.text', testData.psed.likely);
        PublicSectorEqualityDutyImpact.getImpactReason().should('include.text', testData.psed.reason);
        PublicSectorEqualityDutyImpact.markComplete();
        cy.confirmContinueBtn().click();
        projectTaskList.publicSectorEqualityDutyStatus().should('contain.text', testData.completedText);
    });

    it('User could complete Rationale task', () => {
        projectTaskList.selectRationale();
        Rationale.changeRationale(testData.rationale);
        Rationale.getRationale().should('contain.text', testData.rationale);
        Rationale.markComplete();
        cy.confirmContinueBtn().click();
        projectTaskList.getRationaleStatus().should('contain.text', testData.completedText);
    });

    it('User could complete Risks and issues task', () => {
        projectTaskList.selectRisksAndIssues();
        RisksAndIssues.changeRisksAndIssues(testData.risksAndIssues);
        RisksAndIssues.getRisksAndIssues().should('contain.text', testData.risksAndIssues);
        RisksAndIssues.markComplete();
        cy.confirmContinueBtn().click();
        projectTaskList.getRisksAndIssuesStatus().should('contain.text', testData.completedText);
    });

    it('User could complete Local authority task ', () => {
        projectTaskList.selectLA();
        LocalAuthorityInfomation.changeTemplateDates(oneMonthAgoDate, nextMonthDate);
        LocalAuthorityInfomation.getTemplateSentDate().should('contain.text', oneMonthAgoDate.getDate());
        LocalAuthorityInfomation.getTemplateSentDate().should(
            'contain.text',
            oneMonthAgoDate.toLocaleString('default', { month: 'long' })
        );
        LocalAuthorityInfomation.getTemplateSentDate().should('contain.text', oneMonthAgoDate.getFullYear());
        LocalAuthorityInfomation.getTemplateReturnedDate().should('contain.text', nextMonthDate.getDate());
        LocalAuthorityInfomation.getTemplateReturnedDate().should(
            'contain.text',
            nextMonthDate.toLocaleString('default', { month: 'long' })
        );
        LocalAuthorityInfomation.getTemplateReturnedDate().should('contain.text', nextMonthDate.getFullYear());

        LocalAuthorityInfomation.changeComments(testData.localAuthority.comment);
        LocalAuthorityInfomation.getComments().should('contain.text', testData.localAuthority.comment);

        LocalAuthorityInfomation.changeSharepointLink(testData.localAuthority.sharepointLink);
        LocalAuthorityInfomation.getSharepointLink().should('contain.text', testData.localAuthority.sharepointLink);

        LocalAuthorityInfomation.markComplete();
        cy.confirmContinueBtn().click();
        projectTaskList.getLAStatus().should('contain.text', testData.completedText);
    });

    it('Performance Info ', () => {
        projectTaskList.selectOfsted();
        Performance.verifyOfsteadScreenText();
        projectTaskList.clickOfStedINfoBackButton();

        for (const keyStage of testData.keyStages) {
            console.log(keyStage);
            projectTaskList.getProjectUrn().then((urn) => {
                projectTaskList.selectKeyStage(keyStage);
                Performance.verifyKeyStageScreenText(urn);
                Performance.changeKeyStageInfo(keyStage);
            });
        }
    });

    it('Check accessibility across pages', () => {
        cy.checkAccessibilityAcrossPages();
    });
});
