/// <reference types="cypress" />
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
import { AcademisationApiKey, AcademisationApiUrl, CypressApiKey } from '../constants/cypressConstants';

//--Universal

Cypress.Commands.add('getByDataTest', (dataTest: string) => {
    return cy.get(`[data-test="${dataTest}"]`);
});

Cypress.Commands.add('getByDataCy', (dataCy: string) => {
    return cy.get(`[data-cy="${dataCy}"]`);
});

Cypress.Commands.add('getByDataId', (dataId: string) => {
    return cy.get(`[data-id="${dataId}"]`);
});

Cypress.Commands.add('getById', (id: string) => {
    return cy.get(`#${id}`);
});

Cypress.Commands.add('getByDataModule', (dataModule: string) => {
    return cy.get(`[data-module="${dataModule}"]`);
});

Cypress.Commands.add('clickSubmitBtn', () => {
    return cy.get('#submit-btn').click();
});

Cypress.Commands.add('clickContinueBtn', () => {
    cy.get('button').contains('Continue').click();
});

Cypress.Commands.add('clickButton', (buttonText: string) => {
    cy.get('button').contains(buttonText).click();
});

Cypress.Commands.add('containsText', (text: string) => {
    cy.contains(text);
});

Cypress.Commands.add('urlPath', () => cy.location().then((location) => `${location.origin}${location.pathname}`));

Cypress.Commands.add('checkPath', (path: string) => {
    cy.url().should('include', path);
});

Cypress.Commands.add('login', ({ titleFilter }: { titleFilter?: string } = {}) => {
    const filterQuery = titleFilter ? `?Title=${encodeURIComponent(titleFilter)}` : '';
    cy.visit(`/project-list${filterQuery}`);
});

Cypress.Commands.add('acceptCookies', () => {
    cy.setCookie('.ManageAnAcademyConversion.Consent', 'True');
    cy.setCookie('.ManageAnAcademyTransfer.Consent', 'True');
});

// School Listing Summary Page (Universal)
Cypress.Commands.add('selectSchoolListing', (listing: number | string) => {
    cy.getById(`school-name-${listing}`).click();
    cy.get('*[href*="/confirm-school-trust-information-project-dates"]').should('be.visible');
    cy.saveLocalStorage();
});

// Confirm and Continue Button (Universal)
Cypress.Commands.add('confirmContinueBtn', () => {
    return cy.getById('confirm-and-continue-button');
});

// Preview Project Template Button (Universal)
Cypress.Commands.add('previewProjectTempBtn', () => {
    return cy.getById('preview-project-template-button');
});

// Generate Project Template Button (Universal)
Cypress.Commands.add('generateProjectTempBtn', () => {
    return cy.getById('generate-project-template-button');
});

Cypress.Commands.add(
    'enterDate',
    (idSelector: string, day: number | string, month: number | string, year: number | string) => {
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
    return cy.get('[data-cy="select-radio-no" i]');
});

// Yes Radio Btn
Cypress.Commands.add('YesRadioBtn', () => {
    return cy.get('[data-cy="select-radio-yes" i]');
});

// Any radio button
Cypress.Commands.add('RadioBtn', (label: string) => {
    return cy.get(`[data-cy="select-radio-${label}" i]`);
});

Cypress.Commands.add('clearFilters', () => {
    cy.getByDataCy('select-projectlist-filter-clear').should('have.text', 'Clear filters');
    cy.getByDataCy('select-projectlist-filter-clear').click();
});

Cypress.Commands.add('saveContinue', () => {
    return cy.getById('save-and-continue-button');
});

Cypress.Commands.add('executeAccessibilityTests', () => {
    const wcagStandards = ['wcag22aa', 'wcag21aa'];
    const impactLevel = ['critical', 'minor', 'moderate', 'serious'];
    const continueOnFail = false;
    cy.injectAxe();
    cy.checkA11y(
        undefined,
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
    (method: string, url: string, body: object | null = null, failOnStatusCode = true) => {
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
