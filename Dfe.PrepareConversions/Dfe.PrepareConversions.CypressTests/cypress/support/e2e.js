// ***********************************************************
// This example support/index.js is processed and
// loaded automatically before your test files.
//
// This is a great place to put global configuration and
// behavior that modifies Cypress.
//
// You can change the location of this file or turn off
// automatically serving support files with the
// 'supportFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/configuration
// ***********************************************************

import './commands'
import registerCypressGrep from '@cypress/grep/src/support'


registerCypressGrep()

Cypress.on('uncaught:exception', (err) => {
	if (
	  err.message.includes('ResizeObserver') ||
	  err.name === 'ResizeObserver loop limit exceeded'
	) {
	  return false;
	}
  });
  
  Cypress.on('fail', (err, runnable) => {
	if (err.message.includes('ResizeObserver loop completed')) {
	  // suppress the error completely
	  return false;
	}
	throw err; // re-throw all other errors
  });
  
// ***********************************************************

beforeEach(() => {
	cy.intercept(
		{ url: Cypress.env('url') + '/**', middleware: true },
		//Add authorization to all Cypress requests
		(req) => req.headers['Authorization'] = 'Bearer ' + Cypress.env('cypressTestSecret'),
		(req) => req.headers['AuthorizationRole'] = 'conversions.create'
	)
})

// ***********************************************************

import 'cypress-axe'

// ***********************************************************
import 'cypress-plugin-api'