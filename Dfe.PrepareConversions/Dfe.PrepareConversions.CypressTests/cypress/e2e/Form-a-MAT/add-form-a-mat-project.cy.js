let APPLICATION_ID = 31;
let URN_ID = null;

describe('Add Form-a-MAT application through API request and verify on the frontend', () => {
    beforeEach(() => {
		cy.sqlServer(`UPDATE [academisation].[ConversionApplication]
		SET [academisation].[ConversionApplication].ApplicationStatus = 'InProgress'
		SELECT TOP (1000) [Id]
			  ,[ApplicationType]
			  ,[CreatedOn]
			  ,[LastModifiedOn]
			  ,[ApplicationStatus]
			  ,[FormTrustId]
			  ,[JoinTrustId]
			  ,[ApplicationSubmittedDate]
			  ,[DynamicsApplicationId]
			  ,[ApplicationReference]
		  FROM [academisation].[ConversionApplication]
		  WHERE [academisation].[ConversionApplication].ApplicationReference = 'A2B_31'`);
	  });

	it('TC01: should create a Form-a-MAT project using Api', () => {
		cy.request({
			method: 'POST',
			url: `application/${APPLICATION_ID}/submit`,
		response: [],
		}).then((response) => {
			console.log(response.body)
            assert.equal(response.status, 201)
            expect(response.body).to.not.be.null
            console.log(response)
            expect(response.body).to.be.a('array')
            expect(response.status).to.equal(201);
			expect(response.body[0]).to.have.property('academyTypeAndRoute');
			expect(response.body[0].academyTypeAndRoute).to.equal('Form a Mat');
			expect(response.body[0]).to.have.property('schoolName');
            expect(response.body[0]).to.have.property('urn');
            URN_ID = response.body[0].urn;
			console.log(URN_ID)
            expect(response.body[0]).to.have.property('projectStatus');
           // expect(response.body[0]).to.have.property('nameOfTrust');
		   expect(response.body[1]).to.have.property('academyTypeAndRoute');
		   expect(response.body[1].academyTypeAndRoute).to.equal('Form a Mat');
		   expect(response.body[1]).to.have.property('schoolName');
		   expect(response.body[1]).to.have.property('urn');
		});

        //TC02: should NOT create a Form a MAT project if it's NOT in Progress to submit
		cy.api({
			method: 'POST',
			url: `application/${APPLICATION_ID}/submit`, failOnStatusCode: false,
			response: [],
		}).then((response) => {
            expect(response.status).to.equal(400);
			expect(response.body[0]).to.have.property('errorMessage');
			expect(response.body[0].errorMessage).to.equal('Application must be In Progress to submit');
		});
	});

	it('TC03: should create a Form a MAT project on the FE', () => {
        cy.login()
        cy.get('[data-cy="select-projectlist-filter-row"]').first().should('be.visible')
        .invoke('text')
        .then((text) => {
          if (text.includes('Route: Form a MAT')) {
		   cy.get('#urn-0').invoke('text')
			 .should('match', /\D+\d+/);
		   cy.get('#urn-0').contains('URN: ')
		   cy.get('#application-to-join-trust-0').contains('Application to join a trust: ')
		   cy.get('#application-received-date-0').contains('Project created date: ')
		   cy.get('#opening-date-0').contains('Opening date: ')
		   cy.get('#school-name-0').click()
		   cy.url().should('include', 'form-a-mat')
		   //check school application form
		   cy.get('[class="moj-sub-navigation__link"]').contains('School application form')
			 . click()
		   //check Other schools in this MAT
		   cy.get('[class="moj-sub-navigation__link"]').contains('Other schools in this MAT')
		   . click()
          }
          else {
            cy.log('this is not Form a MAT project')
          }
        });
	});
});
