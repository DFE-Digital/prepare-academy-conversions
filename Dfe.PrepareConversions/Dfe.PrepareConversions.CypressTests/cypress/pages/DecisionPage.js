export class DecisionPage {


    searchSchool(schoolName) {
        cy.get('#SearchQuery').type(schoolName);
        cy.get('.autocomplete__input--default').first().click();
        return this;
    }

    clickContinue() {
        cy.get('[data-id="submit"]').click();
        return this;
    }

    selectNoAndContinue() {
        cy.get('[data-cy="select-legal-input-no"]').click();
        cy.get('[data-cy="select-common-submitbutton"]').click();
        return this;
    }

    assertSchoolDetails(schoolName, urn, localAuthority, schoolType) {
        cy.get('[data-cy="school-name"]').should('contain', schoolName);
        cy.get('.govuk-summary-list__value').eq(1).should('contain', urn);
        cy.get('.govuk-summary-list__value').eq(3).should('contain', localAuthority);
        cy.get('.govuk-summary-list__value').eq(5).should('contain', schoolType)
        return this;
    }

    clickOnFirstProject() {
        cy.get('[data-cy="select-projectlist-filter-title"]').type('Manchester Academy');
        cy.get('[data-cy="select-projectlist-filter-apply"]').click();
        cy.get('[data-cy="trust-name-Manchester Academy-0"]').click();
        return this;
    }

    clickRecordDecisionMenu() {
        cy.get('[data-cy="record_decision_menu"]').click();
        return this;
    }

    clickRecordDecision() {
        cy.get('[data-cy="record_decision_btn"]').click();
        return this;
    }

    makeDecisionApproved() {
        cy.get('#approved-radio').click();
        cy.get('#submit-btn').click();
        return this;
    }

    makeGrade6Decision() {
        cy.get('#grade6-radio').click();
        cy.get('#submit-btn').click();
        return this;
    }

    enterDecisionMakerName(name) {
        cy.get('#decision-maker-name').type(name);
        cy.get('#submit-btn').click();
        return this;
    }

    selectNoConditions() {
        cy.get('#no-radio').click();
        cy.get('#submit-btn').click();
        return this;
    }

    enterDecisionDate(day, month, year) {
        cy.get('#decision-date-day').type(day);
        cy.get('#decision-date-month').type(month);
        cy.get('#decision-date-year').type(year);
        cy.get('#submit-btn').click();
        return this;
    }

    dateAOWasSent(day, month, year) {
        cy.get('#academy-order-date-day').type(day);
        cy.get('#academy-order-date-month').type(month);
        cy.get('#academy-order-date-year').type(year);
        cy.get('#submit-btn').click();
        return this;
    }

    verifyDecisionDetails(decision, grade, name, conditions, date) {
        cy.get('#decision').should('contain', decision);
        cy.get('#decision-made-by').eq(0).should('contain', grade);
        cy.get('#decision-maker-name').should('contain', name);
        cy.get('#condition-set').should('contain', conditions);
        cy.get('#decision-date').should('contain', date);
        cy.get('#submit-btn').click();
        return this;
    }

    verifyDecisionDetailsBeforeChanging(decision, name, conditions, date) {
        cy.get('#decision').should('contain', decision);
        cy.get('#decision-maker-name').should('contain', name);
        cy.get('#condition-set').should('contain', conditions);
        cy.get('#decision-date').should('contain', date);
        return this;
    }

    verifyDecisionDetailsAfterChanging(decision, name, date) {
        cy.get('#decision').should('contain', decision);
        cy.get('#decision-made-by').should('contain', name);
        console.log('Date:', date);
        cy.get('#decision-date').should('contain', date);
        return this;
    }

    changeDecisionDetails() {
        cy.get('[data-cy="record_decision_menu"]').click();
        this.verifyDecisionDetailsBeforeChanging('Approved', 'Fahad Darwish', 'No', '12 December 2023');
        cy.get('#record-decision-link').click();
        cy.get('#declined-radio').click();
        cy.get('#submit-btn').click();
        cy.get('#directorgeneral-radio').click();
        cy.get('#submit-btn').click();
        //decision maker name will stay as name Fahad Darwish then click continue
        cy.get('#submit-btn').click();
        cy.get('#declined-reasons-finance').click();
        cy.get('#reason-finance').type('Fahads Cypress Reason is Finance');
        cy.get('#submit-btn').click();

        //change the date to 12th November 2023
        cy.get('#decision-date-day').clear();
        cy.get('#decision-date-day').type('12');
        cy.get('#decision-date-month').clear();
        cy.get('#decision-date-month').type('11');
        cy.get('#decision-date-year').clear();
        cy.get('#decision-date-year').type('2023');
        cy.get('#submit-btn').click();

        this.verifyDecisionDetailsAfterChanging('Declined', 'Director General', '12 November 2023');
        return this;
    }
}

export const decisionPage = new DecisionPage();
