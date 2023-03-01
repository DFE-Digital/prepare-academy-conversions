/// <reference types ='Cypress'/>

Cypress._.each(['ipad-mini'], (viewport) => {
  describe(`121477: Add Form 7 for Confirm school and trust information and project dates page on ${viewport}`, () => {
    beforeEach(() => {
      cy.login()
      cy.viewport(viewport)
      cy.get('[data-cy="select-projectlist-filter-row"]').first().should('be.visible')
        .invoke('text')
        .then((text) => {
          if (text.includes('Route: Involuntary conversion')) {
            cy.get('[id="school-name-0"]').click();
            cy.url().should('include', '/task-list')
            cy.get('[aria-describedby="school-and-trust-information-status"]').click()
            cy.url().should('include', '/confirm-school-trust-information-project-dates')
            cy.get('[class="govuk-summary-list"]').should('be.visible')
              .invoke('text')
              .then((text) => {
                text.includes('Has the Schools Notification Mailbox (SNM) received a Form 7?')
                cy.get('[data-test="change-form-7-received"]').should('contain', 'Change')
                  .click()
              })
          }
          else {
            cy.log('this is not involuntary project')
          }
        });
    });

    afterEach(() => {
      cy.clearCookies();
    });

    it('TC01: Form 7 lables should display when user opted for Yes option ', () => {
      cy.get('h1').should('contain', 'Has the Schools Notification Mailbox (SNM) received a Form 7?')
      cy.get('[for="form-7-received"]').first().click()
      cy.get('[id="save-and-continue-button"]').click()
      cy.contains('Date SNM received Form 7')
      cy.get('[data-test="change-form-7-received-date"]').click()
      cy.get('h1').should('contain', 'Date SNM received Form 7')
      cy.submitDateSNMReceivedForm(11, 2, 2023)
      cy.get('[data-cy="select-common-submitbutton"]').click()
      cy.get('[id="form-7-received-date"]').should('contain', '11 February 2023')
    });

    it('TC02: Date SNM should not display when user opted for No option ', () => {
      cy.get('h1').should('contain', 'Has the Schools Notification Mailbox (SNM) received a Form 7?')
      cy.get('#form-7-received-2').click()
      cy.get('[id="save-and-continue-button"]').click()
      cy.get('#form-7-received').should('contain', 'No')
    });

    it('TC03: Validation error messages should show on form 7 page ', () => {
      cy.get('h1').should('contain', 'Has the Schools Notification Mailbox (SNM) received a Form 7?')
      cy.get('[for="form-7-received"]').first().click()
      cy.get('[id="save-and-continue-button"]').click()
      cy.contains('Date SNM received Form 7')
      cy.get('[data-test="change-form-7-received-date"]').click()
      cy.get('h1').should('contain', 'Date SNM received Form 7')
      //incorrect year
      cy.submitDateSNMReceivedForm(11, 2, 2025)
      cy.get('[data-cy="select-common-submitbutton"]').click()
      cy.get('#form-7-received-date-error-link').should('contain', 'Form 7 Received Date date must be in the past')
      //incorrect month
      cy.submitDateSNMReceivedForm(11, 13, 2023)
      cy.get('[data-cy="select-common-submitbutton"]').click()
      cy.get('#form-7-received-date-error-link').should('contain', 'Month must be between 1 and 12')
      //incorrect day
      cy.submitDateSNMReceivedForm(32, 1, 2023)
      cy.get('[data-cy="select-common-submitbutton"]').click()
      cy.get('#form-7-received-date-error-link').should('contain', 'Day must be between 1 and 31')
    });
  });
});