/// <reference types="cypress" />
/// <reference path="./index.d.ts" />
// ***********************************************
// This example commands.ts shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })
import 'cypress-localstorage-commands';
import {
  AcademisationApiKey,
  AcademisationApiUrl,
  CypressApiKey,
  EnvUrl,
} from '../constants/cypressConstants';

//--Universal

Cypress.Commands.add('urlPath', () =>
  cy.location().then((location) => `${location.origin}${location.pathname}`)
);

Cypress.Commands.add('checkPath', (path: string) =>
  cy.url().should('include', path)
);

Cypress.Commands.add('login', ({ titleFilter }: { titleFilter?: string } = {}) => {
  const filterQuery = titleFilter
    ? `?Title=${encodeURIComponent(titleFilter)}`
    : '';
  cy.visit(`${Cypress.env(EnvUrl)}/project-list${filterQuery}`);
});

// Preserving Session Data (Universal)
Cypress.Commands.add('storeSessionData', () => {
  Cypress.Cookies.preserveOnce('.ManageAnAcademyConversion.Login');
  const str: string[] = [];
  cy.getCookies().then((cookie) => {
    cy.log(cookie as unknown as string);
    for (let l = 0; l < cookie.length; l++) {
      if (cookie.length > 0 && l == 0) {
        str[l] = cookie[l].name;
        Cypress.Cookies.preserveOnce(str[l]);
      } else if (cookie.length > 1 && l > 1) {
        str[l] = cookie[l].name;
        Cypress.Cookies.preserveOnce(str[l]);
      }
    }
  });
});

// School Listing Summary Page (Universal)
Cypress.Commands.add('selectSchoolListing', (listing: number | string) => {
  cy.get('#school-name-' + listing).click();
  cy.get('*[href*="/confirm-school-trust-information-project-dates"]').should(
    'be.visible'
  );
  cy.saveLocalStorage();
});

// Confirm and Continue Button (Universal)
Cypress.Commands.add('confirmContinueBtn', () => {
  cy.get('[id="confirm-and-continue-button"]');
});

// Preview Project Template Button (Universal)
Cypress.Commands.add('previewProjectTempBtn', () => {
  cy.get('[id="preview-project-template-button"]');
});

// Generate Project Template Button (Universal)
Cypress.Commands.add('generateProjectTempBtn', () => {
  cy.get('[id="generate-project-template-button"]');
});

Cypress.Commands.add(
  'enterDate',
  (
    idSelector: string,
    day: number | string,
    month: number | string,
    year: number | string
  ) => {
    cy.get(`[id*="${idSelector}-day"]`).as('day');
    cy.get(`[id*="${idSelector}-month"]`).as('month');
    cy.get(`[id*="${idSelector}-year"]`).as('year');

    cy.get('@day').should('be.visible').invoke('val', '').type(String(day));
    cy.get('@month').should('be.visible').invoke('val', '').type(String(month));
    cy.get('@year').should('be.visible').invoke('val', '').type(String(year));
  }
);

// No Radio Btn
Cypress.Commands.add('NoRadioBtn', () => {
  cy.get('[data-cy="select-radio-no" i]');
});

// Yes Radio Btn
Cypress.Commands.add('YesRadioBtn', () => {
  cy.get('[data-cy="select-radio-yes" i]');
});

// Any radio button
Cypress.Commands.add('RadioBtn', (label: string) => {
  cy.get(`[data-cy="select-radio-${label}" i]`);
});

Cypress.Commands.add('clearFilters', () => {
  cy.get('[data-cy="select-projectlist-filter-clear"]').should(
    'have.text',
    'Clear filters'
  );
  cy.get('[data-cy="select-projectlist-filter-clear"]').click();
});

Cypress.Commands.add('saveContinue', () => {
  cy.get('[id="save-and-continue-button"]');
});

Cypress.Commands.add('excuteAccessibilityTests', () => {
  const wcagStandards = ['wcag22aa', 'wcag21aa'];
  const impactLevel = ['critical', 'minor', 'moderate', 'serious'];
  const continueOnFail = false;
  cy.injectAxe();
  cy.checkA11y(
    null,
    {
      runOnly: {
        type: 'tag',
        values: wcagStandards,
      },
      includedImpacts: impactLevel,
    },
    undefined,
    continueOnFail
  );
});

// Interceptors do not run for cy.request or cy.Api. Therefore use a command to make the request instead, an include the required headers etc.
Cypress.Commands.add(
  'callAcademisationApi',
  (
    method: string,
    url: string,
    body: object | null = null,
    failOnStatusCode = true
  ) => {
    const requestDefinition: Partial<Cypress.RequestOptions> = {
      method: method,
      url: `${Cypress.env(AcademisationApiUrl)}/${url}`,
      headers: {
        'x-api-key': Cypress.env(AcademisationApiKey),
        'x-api-cypress-endpoints-key': Cypress.env(CypressApiKey),
        'Content-Type': 'application/json',
      },
      failOnStatusCode: failOnStatusCode,
    };

    // add body to a post/put/patch request, otherwise leave as not supplied
    switch (method.toUpperCase()) {
      case 'POST':
      case 'PUT':
      case 'PATCH':
        requestDefinition.body = body;
        break;
    }

    return cy.request(requestDefinition);
  }
);
