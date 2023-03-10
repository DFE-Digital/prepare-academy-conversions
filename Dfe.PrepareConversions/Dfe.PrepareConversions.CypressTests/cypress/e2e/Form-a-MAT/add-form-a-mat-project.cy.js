let APPLICATION_ID = 105;
let URN_ID = null;

describe('Add Form-a-MAT application using cypress-plugin-api', () => {
	it('TC01: should create a Form-a-MAT project using Api', () => {
		cy.api({
			method: 'POST',
			url: `application/${APPLICATION_ID}/submit`,
		}).then((response) => {
            expect(response.status).to.equal(201);
			expect(response.body.academyTypeAndRoute).to.equal('Form a Mat');
			expect(response.body).to.have.property('schoolName');
            expect(response.body).to.have.property('urn');
            URN_ID = response.body.urn;
            expect(response.body).to.have.property('projectStatus');
            expect(response.body).to.have.property('nameOfTrust');
		});
	});

	it('TC02: should create a Form a MAT project on the FE', () => {
        cy.login()
        cy.get('[data-cy="select-projectlist-filter-row"]').first().should('be.visible')
        .invoke('text')
        .then((text) => {
          if (text.includes('Route: Form a MAT')) {
            cy.get('#urn-0').should('contain', `URN: '${URN_ID}'`)
          }
          else {
            cy.log('this is not involuntary project')
          }
        });
	});

	it('TC03: should give an error when submitted same application twice', () => {
		cy.api({
			method: 'POST',
			url: `application/${APPLICATION_ID}/submit`,
		}).then((response) => {
            expect(response.status).to.equal(400);
			expect(response.body.errorMessage).to.equal('Form Application must be In Progress to submit Mat');
		});
	});
});
