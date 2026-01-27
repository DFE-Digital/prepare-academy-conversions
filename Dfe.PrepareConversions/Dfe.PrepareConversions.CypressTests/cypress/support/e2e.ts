// ***********************************************************
// This example support/e2e.ts is processed and
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

import './commands';
import registerCypressGrep from '@cypress/grep/src/support';
import { CypressTestSecret, EnvUrl } from '../constants/cypressConstants';
import 'cypress-axe';

// ***********************************************************
import 'cypress-plugin-api';

registerCypressGrep();

Cypress.on('uncaught:exception', (err) => {
    if (err.message.includes('ResizeObserver') || err.name === 'ResizeObserver loop limit exceeded') {
        return false;
    }
});

Cypress.on('fail', (err) => {
    if (err.message.includes('ResizeObserver loop completed')) {
        // suppress the error completely
        return false;
    }
    throw err; // re-throw all other errors
});

// ***********************************************************

beforeEach(() => {
    cy.intercept(
        { url: Cypress.env(EnvUrl) + '/**', middleware: true },
        //Add authorization to all Cypress requests
        (req) => {
            req.headers['Authorization'] = 'Bearer ' + Cypress.env(CypressTestSecret);
            req.headers['AuthorizationRole'] = 'conversions.create';
        }
    );
});

// ***********************************************************

declare global {
    namespace Cypress {
        interface Chainable {
            /**
             * Get an element by its data-test attribute
             * @param dataTest - The value of the data-test attribute
             */
            getByDataTest(dataTest: string): Chainable<JQuery<HTMLElement>>;

            /**
             * Get an element by its data-cy attribute
             * @param dataCy - The value of the data-cy attribute
             */
            getByDataCy(dataCy: string): Chainable<JQuery<HTMLElement>>;

            /**
             * Get an element by its data-id attribute
             * @param dataId - The value of the data-id attribute
             */
            getByDataId(dataId: string): Chainable<JQuery<HTMLElement>>;

            /**
             * Get an element by its id attribute (shorthand for cy.get('#id'))
             * @param id - The id of the element (without #)
             */
            getById(id: string): Chainable<JQuery<HTMLElement>>;

            /**
             * Get an element by its data-module attribute
             * @param dataModule - The value of the data-module attribute
             */
            getByDataModule(dataModule: string): Chainable<JQuery<HTMLElement>>;

            /**
             * Click the submit button (#submit-btn)
             */
            clickSubmitBtn(): Chainable<JQuery<HTMLElement>>;

            /**
             * Click a button containing the text 'Continue'
             */
            clickContinueBtn(): Chainable<void>;

            /**
             * Find an element containing the specified text
             * @param text - The text to search for
             */
            containsText(text: string): Chainable<void>;

            /**
             * Get the current URL path (origin + pathname)
             */
            urlPath(): Chainable<string>;

            /**
             * Check that the current URL includes the specified path
             * @param path - The path to check for in the URL
             */
            checkPath(path: string): Chainable<void>;

            /**
             * Login and visit the project list page
             * @param options - Optional login options
             * @param options.titleFilter - Optional title filter to apply
             */
            login(options?: { titleFilter?: string }): Chainable<void>;

            /**
             * Select a school listing by its index
             * @param listing - The listing index
             */
            selectSchoolListing(listing: number | string): Chainable<void>;

            /**
             * Get the confirm and continue button element
             */
            confirmContinueBtn(): Chainable<JQuery<HTMLElement>>;

            /**
             * Get the preview project template button element
             */
            previewProjectTempBtn(): Chainable<JQuery<HTMLElement>>;

            /**
             * Get the generate project template button element
             */
            generateProjectTempBtn(): Chainable<JQuery<HTMLElement>>;

            /**
             * Enter a date into day/month/year input fields
             * @param idSelector - The base ID selector for the date inputs
             * @param day - The day value
             * @param month - The month value
             * @param year - The year value
             */
            enterDate(
                idSelector: string,
                day: number | string,
                month: number | string,
                year: number | string
            ): Chainable<void>;

            /**
             * Get the No radio button element
             */
            NoRadioBtn(): Chainable<JQuery<HTMLElement>>;

            /**
             * Get the Yes radio button element
             */
            YesRadioBtn(): Chainable<JQuery<HTMLElement>>;

            /**
             * Get a radio button by its label
             * @param label - The radio button label
             */
            RadioBtn(label: string): Chainable<JQuery<HTMLElement>>;

            /**
             * Clear all filters on the project list
             */
            clearFilters(): Chainable<void>;

            /**
             * Get the save and continue button element
             */
            saveContinue(): Chainable<JQuery<HTMLElement>>;

            /**
             * Execute accessibility tests using axe-core
             */
            executeAccessibilityTests(): Chainable<void>;

            /**
             * Call the Academisation API
             * @param method - HTTP method (GET, POST, PUT, PATCH, DELETE)
             * @param url - API endpoint URL (relative to base URL)
             * @param body - Optional request body
             * @param failOnStatusCode - Whether to fail on non-2xx status codes (default: true)
             */
            callAcademisationApi(
                method: string,
                url: string,
                body?: object | null,
                failOnStatusCode?: boolean
            ): Chainable<Response<unknown>>;
        }
    }
}
