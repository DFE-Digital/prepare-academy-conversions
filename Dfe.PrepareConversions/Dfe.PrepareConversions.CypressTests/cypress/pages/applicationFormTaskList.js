class ApplicationFormTaskList {
    // Method to interact with the filter search input
    filterSearch(title) {
      cy.get('[data-cy="select-projectlist-filter-title"]').type(title);
      cy.get('[data-cy="select-projectlist-filter-apply"]').click();
    }
  
    // Method to select the first search result
    selectFirstSearchResult() {
        cy.get('#school-name-0').click();
    }

      // Method to select the first search result for Form a MAT
      selectFirstSearchResultFormAMAT() {
        cy.get('#trust-name-0').click();
    }
  
    // Method to check the route
    checkRoute(expectedRoute) {
        cy.get('#type-and-route-0 > span').should('contain', expectedRoute);
    }
  
    // Method to check the page title
    checkTitle(title) {
        cy.get('#school-name-0').should('contain', title);
    }

    //method to click on the first form a mat project 
    selectFirstProjectFormAMAT() {  
        cy.get('#school-name-0').click();
    }
  
    // Method to click on school application form
    clickSchoolApplicationForm() {

        cy.get('[data-cy="school_application_form"]').click();
    }
  // Method to click on school application form for Form a MAT page
    clickSchoolApplicationFormForFormAMAT() {

        cy.get('[data-cy="school_application_form_formamat"]').click();
    }
      // Method to click on school application form First project
      clickSchoolApplicationFormFirstProjet() {

        cy.get('#school-name-0').click();
    }

  // Method to check the url contains school-application-form
    checkUrlContainsSchoolApplicationForm() {
  cy.url().should('contain', 'school-application-form');
    }

     // Method to check the url contains schools-in-this-mat
     checkUrlContainsschoolsinthismat() {
        cy.url().should('contain', 'schools-in-this-mat');
          }

      // Method to check the route in Application Form page
      checkRouteAppForm(expectedRouteAppRoute) {
        
    cy.get('[data-cy="route"]').should('contain', expectedRouteAppRoute);
  
    }
    // Method to check the contents of the table
  checkTableContents(expectedContents) {
    cy.get('[data-cy="content-Table"]').within(() => {
      expectedContents.forEach((content, index) => {
        cy.get('[data-cy="contents_list_items"]').eq(index).should('contain', content);
      });
    });
  }

  // Method to check specific elements for expected text
  checkElementText(selector, expectedText) {
    cy.get(selector).should('contain', expectedText);
  }

  clickFormAMAT() { 
    cy.get('[data-cy="formAMatLink"]').click();
  }
}
  
  module.exports = new ApplicationFormTaskList();
  