/// <reference types="cypress" />

declare namespace Cypress {
    interface Chainable {
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
        ): Chainable<Cypress.Response<unknown>>;
    }
}
