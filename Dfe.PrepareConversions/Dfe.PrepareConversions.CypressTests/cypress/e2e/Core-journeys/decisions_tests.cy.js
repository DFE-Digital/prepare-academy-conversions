import projectTaskList from "../../pages/projectTaskList";
import { decisionPage } from '../../pages/decisionPage';
import { Logger } from '../../support/logger'

describe('Decisions Tests', () => {

  let projectId;

  beforeEach(() => {
    Logger.log("Visit the homepage before each test");
    projectTaskList.getHomePage();
  });

  it('Creating a conversion project and recording a new decision then editing the decision', () => {
    Logger.log("Go to the home page then click create new conversion");
    projectTaskList.clickCreateNewConversionBtn();

    Logger.log("Click on create new conversion button on the next page");
    projectTaskList.clickCreateNewConversionBtn();

    Logger.log("Search and select the school, then click continue");
    decisionPage.searchSchool('Manchester Academy (134224)').clickContinue();

    Logger.log("Select no and continue on the next 3 pages regarding the school");
    decisionPage.selectNoAndContinue();
    decisionPage.selectNoAndContinue();
    decisionPage.selectNoAndContinue();

    Logger.log("Verify the selected school details");
    decisionPage.assertSchoolDetails(
      'Manchester Academy',
      '134224',
      'Manchester',
      'Academy'
    );

    Logger.log("Click school details are correct, click continue");
    decisionPage.clickContinue();

    Logger.log("Search for the created project then select first one on the list");
    decisionPage.clickOnFirstProject();

    Logger.log("Capture the projectId dynamically from the URL");
    cy.url().then((url) => {
      projectId = url.match(/task-list\/(\d+)/)[1];
      Logger.log(`Project ID: ${projectId}`);
    }).then(() => {
      Logger.log("Click on record a decision menubar button");
      decisionPage.clickRecordDecisionMenu();

      Logger.log("Click on record a decision button and add a decision to check error handling");
      decisionPage.clickRecordDecision();

      decisionPage.makeDecision("approved")


      Logger.log("Check error and add necessary details to record the decision");
      decisionPage.checkErrorAndAddDetails('15', '10', '2024', 'Paul Lockwood');

      Logger.log("Click on record a decision menubar button");
      decisionPage.clickRecordDecisionMenu();

      

      Logger.log("Record the decision with the necessary details");
      decisionPage.clickRecordDecisionWithoutError()
      .makeDecision("deferred")
        .decsionMaker("grade6")
        .selectReasonWhyDeferred()
        .enterDecisionMakerName('Fahad Darwish')
        .enterDecisionDate('12', '12', '2023')
        .verifyDecisionDetails('Deferred', 'Grade 6', 'Fahad Darwish', '12 December 2023');

      Logger.log("Verify that decision was recorded successfully then change the decision details, verify the changes");
      decisionPage.changeDecisionDetails();

      Logger.log("Change the current decision to DAO (Directive Academy Order) revoked and verify the changes");
      decisionPage.changeDecisionDAODetails();

      Logger.log("Change the current decision to Approved, verify the changes then check if the project is readonly");
      decisionPage.changeDecisionApproved();

      Logger.log("Check if this project is readonly after adding a decision");
      decisionPage.clickToGoBackandCheckReadOnly();

      Logger.log("Delete the project and verify that it was deleted successfully - Project ID: " + projectId);
      decisionPage.deleteProject(projectId);
    });
  });
});
