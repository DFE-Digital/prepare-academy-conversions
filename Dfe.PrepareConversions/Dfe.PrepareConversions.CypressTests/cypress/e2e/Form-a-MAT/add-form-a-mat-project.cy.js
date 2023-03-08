const FIRST_NAME = 'Brad';
const UPDATED_FIRST_NAME = 'Bradley';
const LAST_NAME = 'Reaney';


describe('Add Form-a-MAT application using cypress-plugin-api', () => {
	it('should perform a health check', () => {
		cy.api('//legacy/project/involuntary-conversion-project').then((response) => {
			expect(response.status).to.equal(201);
			expect(response.body).to.equal('Created');
		});
	});

	it('should create a Form-a-MAT project', () => {
		cy.api({
			method: 'POST',
			url: '/booking',
			body: {
				'firstname': FIRST_NAME,
				'bookingdates': {
					'checkin': DATE,
					'checkout': DATE
				},
			}
		}).then((response) => {
			expect(response.status).to.equal(200);
			expect(response.body).to.have.property('bookingid');
		});
	});
});
