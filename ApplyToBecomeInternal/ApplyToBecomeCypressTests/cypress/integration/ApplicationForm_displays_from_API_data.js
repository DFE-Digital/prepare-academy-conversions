/// <reference types ='Cypress'/>

describe('91489: Apply-to-become GET application types', () => {
    let url = Cypress.env('url')

    afterEach(() => {
        cy.storeSessionData()
    });
    
    before(function () {
        cy.login()
        cy.get('[id="school-name-46"]').click()
    });

    after(() => {
        cy.clearLocalStorage()
    });


    it('TC01: Application form page displays data from API', () => {
        var urlAPI = 'https://trams-external-api.azurewebsites.net'
        var endpoint = '/v2/apply-to-become/application/'
        var fullUrl = urlAPI + endpoint
        var key = Cypress.env('key')

        cy.request({
            method: 'GET',
            url: fullUrl+'Cath102', 
            headers: {'ApiKey' : key}
            }).then(
                (response) => {
                // response.body is automatically serialized into JSON
                var target = response.body.data;
                expect(response.body.data).to.have.property('applicationId', 'Cath102')
                expect(response.body.data).to.have.property('applicationType', 'JoinMat')
                cy.visit(url+'/school-application-form/555')
        
                cy.get('#Overview2').within(() => {
                    cy.get('dt').should('contain', 'Application reference')            
                    cy.get('dd').should('contain', target['applicationId'])
                })
            })
        });
})