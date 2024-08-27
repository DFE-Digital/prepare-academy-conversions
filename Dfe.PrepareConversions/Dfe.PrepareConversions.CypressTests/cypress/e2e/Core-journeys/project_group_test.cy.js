import projectTaskList from "../../pages/projectTaskList";
import { decisionPage } from '../../pages/decisionPage';
import { Logger } from '../../support/logger';

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


  });
});


describe('Group Creation Tests', () => {

  let groupId;

  beforeEach(() => {
    Logger.log("Visit the homepage before each test");
    projectTaskList.getHomePage();
  });

  it('Creating a new group and linking schools to an existing trust', () => {
    Logger.log("Go to the Groups page");
    cy.visit(`${Cypress.env('url')}/groups/project-list`);
    projectTaskList.clickGroupsLink();

    Logger.log("Click on create new group");
    projectTaskList.clickCreateNewGroupBtn();

    Logger.log("Click on create a group");
    projectTaskList.clickCreateGroupBtn();

    Logger.log("enerting which trust will the group join");
    cy.get('.autocomplete__wrapper > #SearchQuery').type('Greater Manchester Academies Trust (10058252)')
    cy.get('.autocomplete__wrapper > #SearchQuery').type('{enter}')

    Logger.log("check urn")
    projectTaskList.checkURNAndContinue('10058252');

    Logger.log("Select conversion")
    projectTaskList.selectConversion();

    Logger.log("click continue");
    projectTaskList.clickContinue();

    Logger.log("remove the school from the group");
    projectTaskList.removeSchoolFromGroup();

    Logger.log("delete the group");
    projectTaskList.deleteGroup();

  });
});

