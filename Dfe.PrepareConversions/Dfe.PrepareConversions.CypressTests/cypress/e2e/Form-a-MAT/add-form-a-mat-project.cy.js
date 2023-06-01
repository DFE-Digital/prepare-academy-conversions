/// <reference types ='Cypress'/>
let APPLICATION_ID = 31;
let URN_ID = null;
let apiKey =  Cypress.env('academisationApiKey');
describe.skip('Add Form-a-MAT application through API request and verify on the frontend', () => {
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
		cy.callAcademisationApi('POST', `application/${APPLICATION_ID}/submit`)
		.then((response) => {
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
		cy.callAcademisationApi('POST', `application/${APPLICATION_ID}/submit`, null, false)
		.then((response) => {
			expect(response.status).to.equal(400);
			expect(response.body[0]).to.have.property('errorMessage');
			expect(response.body[0].errorMessage).to.equal('Application must be In Progress to submit');
		});
	});

	it('TC03: should create a Form a MAT project on the FE with correct details', () => {
		cy.login({ titleFilter: 'Redruth School' })
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

					//verify school application form tab info
					cy.get('[class="moj-sub-navigation__link"]').contains('School application form')
						.click()
					cy.schoolApplicationForm();

					//verify Other schools in this MAT tab info
					cy.get('[class="moj-sub-navigation__link"]').contains('Other schools in this MAT')
						.click()
					cy.schoolsInThisMAT();

					//navigate to other school
					cy.get('#school-name-0').click()
					cy.title().should('contain', 'School Application Form')
					cy.get('h1').should('not.contain', 'Page not found')
					cy.get('[class="moj-sub-navigation__link"]').contains('Other schools in this MAT')
						.click()
					cy.get('h1').should('not.contain', 'Page not found')
				}
				else {
					cy.log('this is not Form a MAT project')
					Cypress.runner.stop()
				}
			});
	});
});
