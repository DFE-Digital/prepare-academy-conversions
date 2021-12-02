describe('validate advisory board urls', () => {
    afterEach(() => {
		cy.storeSessionData();
        //cy.visit(Cypress.env('url')+ '/task-list/4')
	});

    before(function () {
        cy.login();
        cy.selectSchoolListing(2)
     });
    
   after(function () {
        cy.clearLocalStorage();
    });  

    it('should correctly render a page when user visits /confirm-school-trust-information-project-dates/advisory-board-date', () => {
        cy.url().then(url =>{
            //Changes the current URL
            let modifiedUrl = url + "/confirm-school-trust-information-project-dates/advisory-board-date"
            cy.visit(modifiedUrl)
            cy.get('h1').contains('Set the Advisory Board date')
        });
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
        cy.url().then(url =>{
            //Changes the current URL
            let modifiedUrl = url.replace(url,"/confirm-school-trust-information-project-date/previous-advisory-board-date")
            cy.visit(modifiedUrl)
            cy.get('h1').contains('Set the Advisory Board date')
        });
    });

    it('', () => {
        
    });
});