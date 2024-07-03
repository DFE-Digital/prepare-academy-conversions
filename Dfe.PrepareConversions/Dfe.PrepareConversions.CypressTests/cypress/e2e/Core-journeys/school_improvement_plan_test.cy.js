import projectTaskList from "../../pages/projectTaskList";
import { schoolImprovementPage } from '../../pages/SchoolImprovementPage';
import { Logger } from '../../support/logger';

describe('School Improvement Plan Tests', () => {

    let projectId;

    beforeEach(() => {
        Logger.log("Visit the homepage before each test");
        projectTaskList.getHomePage();
    });

    it('Creating a conversion project and recording a new interim school improvement plan', () => {
        Logger.log("Go to the home page then click create new conversion");
        projectTaskList.clickCreateNewConversionBtn();

        Logger.log("Click on create new conversion button on the next page");
        projectTaskList.clickCreateNewConversionBtn();

        Logger.log("Search and select the school, then click continue");
        schoolImprovementPage.searchSchool('Manchester Academy (134224)').clickContinue();

        Logger.log("Select no and continue on the next 3 pages regarding the school");
        schoolImprovementPage.selectNoAndContinue();
        schoolImprovementPage.selectNoAndContinue();
        schoolImprovementPage.selectNoAndContinue();

        Logger.log("Verify the selected school details");
        schoolImprovementPage.assertSchoolDetails(
            'Manchester Academy',
            '134224',
            'Manchester',
            'Academy'
        );

        Logger.log("Click school details are correct, click continue");
        schoolImprovementPage.clickContinue();

        Logger.log("Search for the created project then select first one on the list");
        schoolImprovementPage.clickOnFirstProject();

        Logger.log("Capture the projectId dynamically from the URL");
        cy.url().then((url) => {
            projectId = url.match(/task-list\/(\d+)/)[1];
            Logger.log(`Project ID: ${projectId}`);
        }).then(() => {
            Logger.log("Navigate to the section to record interim school improvement plan");
            schoolImprovementPage.navigateToSchoolImprovementSection();

            Logger.log("Click on 'Add school improvement plan'");
            schoolImprovementPage.clickAddSchoolImprovementPlan();

            Logger.log("Select who arranged the improvement plan");
            schoolImprovementPage.selectImprovementDetails();

            Logger.log("Save the who arranged the school improvement plan");
            schoolImprovementPage.saveImprovementDetails();

            Logger.log("Enter the who is providing the interim school improvement plan details");
            schoolImprovementPage.enterImprovementDetails("Fahad-Cypress Test");

            Logger.log("Enter the imprvoment plan's start date");
            schoolImprovementPage.enterImprovementEndDate('12', '12', '2023');

            Logger.log("Entering expected end date of the improvement plan");
            schoolImprovementPage.enterExpectedEndDate('12', '11', '2024');

            Logger.log("Select the confidence level of the improvement plan");
            schoolImprovementPage.selectHighConfidenceLevel();

            Logger.log("Entering comments on the interim school improvement plan");
            schoolImprovementPage.enterComments('This is a test comment');

            Logger.log("Verify the interim school improvement plan details are correct");
            schoolImprovementPage.verifyImprovementDetails("Local Authority", "Fahad-Cypress Test", "12 December 2023", "12 November 2024", "High", "This is a test comment");
            Logger.log("Verify the submitted interim school improvement plan details are saved correctly");
            schoolImprovementPage.verifyTheFinalImprovementDetails("Local Authority", "Fahad-Cypress Test", "12 December 2023", "12 November 2024", "High", "This is a test comment");
            Logger.log("change the details");
            schoolImprovementPage.changeImprovementDetails("Cypress Test Interim schoool plan","Test comment");
            Logger.log("Verify the changed interim school improvement plan details are saved correctly");
            schoolImprovementPage.verifyChangedImprovementDetails("Regional Director, Local Authority", "Cypress Test Interim schoool plan", "12 December 2023", "Unknown", "Medium", "Test comment");
           
            Logger.log("Delete the project and verify that it was deleted successfully - Project ID: " + projectId);
            schoolImprovementPage.deleteProject(projectId);
        });
    });
});
