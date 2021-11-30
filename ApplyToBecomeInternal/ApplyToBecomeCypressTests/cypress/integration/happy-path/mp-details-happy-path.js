describe('Submit and view MP details', () => {
    afterEach(() => {
		cy.storeSessionData();
	});

    before(function () {
        cy.login();
        cy.selectSchoolListing(2)
        cy.url().then(url =>{
            //Changes the current URL
            let modifiedUrl = url + "/confirm-general-information"
            cy.visit(modifiedUrl)
        });
     });
    
    after(function () {
        cy.clearLocalStorage();
    });

    it('Should navigate to MP details page', () => {
        cy.get("[data-test='change-member-of-parliament-party']").click();
        cy.url().then(href => {
            expect(href.endsWith('/confirm-general-information/mp-details')).to.be.true;
        });
    });

    it('Should change the MP details', () => {
        cy.get('#member-of-parliament-name').clear().type('An MP');
        cy.get('#member-of-parliament-party').clear().type('A Party');
        cy.get('#member-of-parliament-name').should('have.value', 'An MP');
        cy.get('#member-of-parliament-party').should('have.value', 'A Party');
    });

    it('Should go back to general information page on confirm', () => {
        cy.get('#confirm-and-continue-button').click();
        cy.url().then(href => {
            expect(href.endsWith('/confirm-general-information')).to.be.true;
          });
    });

    it('Should display the MP details after it is submitted', () => {        
        cy.get('#member-of-parliament-name').should('have.text', 'An MP');
        cy.get('#member-of-parliament-party').should('have.text','A Party');    
    })

    it('Should navigate to MP details page and remove details', () => {
        cy.get("[data-test='change-member-of-parliament-party']").click();
        cy.get('#member-of-parliament-name').clear();
        cy.get('#member-of-parliament-party').clear();
        cy.get('#confirm-and-continue-button').click();
        cy.get('#member-of-parliament-name').should('have.text', 'Empty');
        cy.get('#member-of-parliament-party').should('have.text', 'Empty'); 
    });
})
