import projectTaskList from "../../pages/projectTaskList";
import { conversionDateChangePage } from '../../pages/conversionDateChangePage';
import { Logger } from '../../support/logger';

describe('Conversion Date Change Tests', () => {

    let projectId;

    beforeEach(() => {
        Logger.log("Visit the homepage before each test");
        projectTaskList.getHomePage();
    });

    it('Creating a conversion project', () => {
        Logger.log("Go to the home page then click create new conversion");
        projectTaskList.clickCreateNewConversionBtn();

        Logger.log("Click on create new conversion button on the next page");
        projectTaskList.clickCreateNewConversionBtn();

        Logger.log("Search and select the school, then click continue");
        conversionDateChangePage.searchSchool('Manchester Academy (134224)').clickContinue();

        Logger.log("Select no and continue on the next 3 pages regarding the school");
        conversionDateChangePage.selectNoAndContinue();
        conversionDateChangePage.selectNoAndContinue();
        conversionDateChangePage.selectNoAndContinue();

        Logger.log("Verify the selected school details");
        conversionDateChangePage.assertSchoolDetails(
            'Manchester Academy',
            '134224',
            'Manchester',
            'Academy'
        );

        Logger.log("Click school details are correct, click continue");
        conversionDateChangePage.clickContinue();

        Logger.log("Search for the created project then select first one on the list");
        conversionDateChangePage.clickOnFirstProject();

        Logger.log("Capture the projectId dynamically from the URL");
        cy.url().then((url) => {
            projectId = url.match(/task-list\/(\d+)/)[1];
            Logger.log(`Project ID: ${projectId}`);
        }).then(() => {
            Logger.log("Navigate to the section to record conversion date change");
            conversionDateChangePage.navigateToConversionDateChangeSection();

            Logger.log("Click and update 'Advisory board date change'");
             conversionDateChangePage.updateAdvisoryBoardDate();
             conversionDateChangePage.checkAdvisoryBoardDateChange();
            
            Logger.log("Click and update 'Previous advisory board date change'");
            conversionDateChangePage.updatePreviousAdvisoryBoardDate();
            conversionDateChangePage.checkPreviousAdvisoryBoardDateChange();

            Logger.log("Click and update 'Proposed advisory board date'");
            conversionDateChangePage.updateProposedConversionDate();
            conversionDateChangePage.checkProposedConversionDateChange();

            Logger.log("Confirm the Conversion Date Change");
            conversionDateChangePage.confirmConversionDateChange();
        
            Logger.log("Delete the project and verify that it was deleted successfully - Project ID: " + projectId);
            conversionDateChangePage.deleteProject(projectId);
        });
    });
});
