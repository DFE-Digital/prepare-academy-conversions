// Purpose: To test the correct headings are showing on the page for the different types of applications
import projectList from "../../pages/projectList";
import projectTaskList from "../../pages/projectTaskList";

describe('Filteration Tests', { tags: ['@dev', '@stage'] }, () => {

    beforeEach(() => {
        projectList.checkProjectListPage();
      })
it.only('Correct Application Form showing for Standard Voluntary', () => {
    projectTaskList.getHomePage();
    cy.get('[data-cy="Conversions_tab"]').click();      
    projectList.verifyCorrectHeaders();
    projectList.verifyApplicationToJoin('Voluntary conversion');
 });

 it.only('Correct Application Form showing for Form a MAT', () => {
    projectTaskList.getHomePage();
    projectList.verifyCorrectHeaders();
    projectList.verifyApplicationToJoin('Form a MAT Voluntary conversion');
 });  
});
