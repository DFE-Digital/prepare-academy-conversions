describe('validate advisory board urls', () => {
    let selectedSchool = '';

    afterEach(() => {
		cy.storeSessionData();
	});

    beforeEach(function () {
        cy.login();
        cy.selectSchoolListing(2)
        cy.url().then(url =>{
            selectedSchool = url
        })
     });
    
   after(function () {
        cy.clearLocalStorage();
    });  


    it('should correctly render a page when user visits /confirm-school-trust-information-project-dates/advisory-board-date', () => {
            //Changes the current URL
            cy.visit(selectedSchool + "/confirm-school-trust-information-project-dates/advisory-board-date"            )
            cy.get('h1').contains('Set the Advisory Board date')
        
    });

    /* it('should correctly render a page when user visits /task-list', () => {
        cy.url().then(url =>{
            //Changes the current URL
            let modifiedUrl = url + "task-list"
            cy.visit(modifiedUrl)
            cy.get('h1').contains('Set the Advisory Board dateTask List')
        }); 
    }); */
    it('should correctly render a page when user visits /confirm-school-trust-information-project-dates/previous-advisory-board-date', () => {
            //Changes the current URL
            cy.visit(selectedSchool+"/confirm-school-trust-information-project-dates/previous-advisory-board-date")
            cy.get('h1').contains('Date of previous Advisory Board')
    });
});