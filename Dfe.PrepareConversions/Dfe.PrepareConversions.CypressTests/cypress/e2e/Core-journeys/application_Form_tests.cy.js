import '../../support/commands';
import projectTaskList from "../../pages/projectTaskList";
const applicationFormTaskList = require('../../pages/applicationFormTaskList');

describe('Test Application Form for Voluntary and Form a MAT', { tags: ['@dev', '@stage'] }, () => {

    beforeEach(() => {
        projectTaskList.getHomePage();
    });

   it('Should filter and select the project, then verify details of Voluntary', () => {

    // Step 1: Filter search
    applicationFormTaskList.filterSearch('Fahads Cypress Trust');

    // Step 2: Verify the title and route
    applicationFormTaskList.checkTitle('Fahads Cypress Trust');
    applicationFormTaskList.checkRoute('Voluntary conversion');

    // Step 3: Select the first search result
     applicationFormTaskList.selectFirstSearchResult();

    // Step 4: Click on school application form
    applicationFormTaskList.clickSchoolApplicationForm();

    //Step 4.1: Check URL contains to contain school-application-form
    applicationFormTaskList.checkUrlContainsSchoolApplicationForm();

    //Step 4.2: Check the route 
    applicationFormTaskList.checkRouteAppForm('Voluntary conversion');

    // Step 5: Check the table contents
    const expectedContents = [
      'Overview',
      'About the conversion',
      'Further information',
      'Finances',
      'Future pupil numbers',
      'Land and buildings',
      'Pre-opening support grant',
      'Consultation',
      'Declaration'
    ];
    applicationFormTaskList.checkTableContents(expectedContents);

    // Step 6: Check Overview1_value
    applicationFormTaskList.checkElementText('[test-id="Overview1_value"]', 'PLYMOUTH CAST with Plymstock School');

    // Step 7: Check Application reference
    applicationFormTaskList.checkElementText('[test-id="Overview2"]', 'A2B_124378');
  });
  
  it('Should filter and select the project, then verify details of a Form a MAT project', () => {

    // Step 1: Filter search
    applicationFormTaskList.clickFormAMAT();
    applicationFormTaskList.filterSearch('Fahads Cypress Trust');


    // Step 2: Select the first search result
     applicationFormTaskList.selectFirstSearchResultFormAMAT();

    //Step 2.1: Check URL contains to contain school-application-form
    applicationFormTaskList.checkUrlContainsschoolsinthismat();
//step 2.2: Click on the first project
    applicationFormTaskList.selectFirstProjectFormAMAT();
    //Step 2.3: Check the route 
    applicationFormTaskList.checkRouteAppForm('Form a MAT Voluntary conversion');
    // Step 3: Click on school application form
    applicationFormTaskList.clickSchoolApplicationFormForFormAMAT();

    // Step 4: Check the table contents
    
    const expectedContents = [
      'Overview',
      'Trust information',
      'Key people within the trust',
      'About the conversion',
      'Further information',
      'Finances',
      'Future pupil numbers',
      'Land and buildings',
      'Pre-opening support grant',
      'Consultation',
      'Declaration'
    ];
    applicationFormTaskList.checkTableContents(expectedContents);

    // Step 5: Check Overview1_value
    applicationFormTaskList.checkElementText('[test-id="Overview1_value"]', 'Plymouth with Fahads Cypress Trust');

    // Step 6: Check Application reference
    applicationFormTaskList.checkElementText('[test-id="Overview2"]', 'A2B_124335');
  });
});
