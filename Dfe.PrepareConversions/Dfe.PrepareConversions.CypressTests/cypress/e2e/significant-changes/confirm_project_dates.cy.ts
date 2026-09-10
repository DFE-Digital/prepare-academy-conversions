/// <reference types="cypress" />

const projectId = 1;
const confirmProjectDatesPath = `/significant-change/task-list/${projectId}/confirm-project-dates`;

const clearProjectDates = () => {
    cy.visit(confirmProjectDatesPath);
    cy.getById('proposed-decision-date-day').clear();
    cy.getById('proposed-decision-date-month').clear();
    cy.getById('proposed-decision-date-year').clear();
    cy.getById('proposed-change-date-day').clear();
    cy.getById('proposed-change-date-month').clear();
    cy.getById('proposed-change-date-year').clear();
    cy.getByDataCy('select-common-submitbutton').click();
};

const assertProjectDatesStatus = (status: string) => {
    cy.contains('a', 'Confirm project dates').parents('li').find('strong').should('have.text', status);
};

describe('Significant change confirm project dates', () => {
    beforeEach(() => {
        cy.login();
        cy.visit('/');
        cy.acceptCookies();
    });

    it('shows Not started when neither date is entered', () => {
        clearProjectDates();

        assertProjectDatesStatus('Not started');
    });

    it('shows In progress when only the proposed decision date is entered', () => {
        clearProjectDates();
        cy.contains('a', 'Confirm project dates').click();
        cy.getById('proposed-decision-date-day').type('21');
        cy.getById('proposed-decision-date-month').type('11');
        cy.getById('proposed-decision-date-year').type('2026');
        cy.getByDataCy('select-common-submitbutton').click();

        assertProjectDatesStatus('In progress');
    });

    it('shows In progress when only the proposed change date is entered', () => {
        clearProjectDates();
        cy.contains('a', 'Confirm project dates').click();
        cy.getById('proposed-change-date-day').type('22');
        cy.getById('proposed-change-date-month').type('12');
        cy.getById('proposed-change-date-year').type('2026');
        cy.getByDataCy('select-common-submitbutton').click();

        assertProjectDatesStatus('In progress');
    });

    it('shows Completed when both dates are entered', () => {
        clearProjectDates();
        cy.contains('a', 'Confirm project dates').click();
        cy.getById('proposed-decision-date-day').type('21');
        cy.getById('proposed-decision-date-month').type('11');
        cy.getById('proposed-decision-date-year').type('2026');
        cy.getById('proposed-change-date-day').type('22');
        cy.getById('proposed-change-date-month').type('12');
        cy.getById('proposed-change-date-year').type('2026');
        cy.getByDataCy('select-common-submitbutton').click();

        assertProjectDatesStatus('Completed');
    });

    const partialDateCases = [
        { part: 'day', missingParts: 'a month and year' },
        { part: 'month', missingParts: 'a day and year' },
        { part: 'year', missingParts: 'a day and month' },
    ];

    partialDateCases.forEach(({ part, missingParts }) => {
        it(`shows validation when the proposed decision date only includes a ${part}`, () => {
            clearProjectDates();
            cy.contains('a', 'Confirm project dates').click();
            cy.getById(`proposed-decision-date-${part}`).type(part === 'year' ? '2026' : '21');
            cy.getByDataCy('select-common-submitbutton').click();

            cy.get('.govuk-error-summary').should('contain.text', `must include ${missingParts}`);
        });
    });

    partialDateCases.forEach(({ part, missingParts }) => {
        it(`shows validation when the proposed change date only includes a ${part}`, () => {
            clearProjectDates();
            cy.contains('a', 'Confirm project dates').click();
            cy.getById(`proposed-change-date-${part}`).type(part === 'year' ? '2026' : '21');
            cy.getByDataCy('select-common-submitbutton').click();

            cy.get('.govuk-error-summary').should('contain.text', `must include ${missingParts}`);
        });
    });
});
