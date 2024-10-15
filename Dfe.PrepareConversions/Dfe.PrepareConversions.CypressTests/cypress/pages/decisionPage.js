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

    makeDecision(decision) {
        cy.get(`#${decision}-radio`).click();
        cy.get('#submit-btn').click();
        return this;
    }

    decsionMaker(grade) {
        cy.get(`#${grade}-radio`).click();
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
        cy.get('#submit-btn').click();
        cy.get('#declined-reasons-finance').click();
        cy.get('#reason-finance').type('Fahads Cypress Reason is Finance');
        cy.get('#submit-btn').click();
        //decision maker name will stay as name Fahad Darwish then click continue
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
        cy.get('#submit-btn').click();
        return this;
    }
    changeDecisionDAODetails() {
        cy.get('[data-cy="record_decision_menu"]').click();
        cy.get('#record-decision-link').click();
        cy.get('#daorevoked-radio').click();
        cy.get('#submit-btn').click();
        cy.get('#submit-btn').click();
        cy.get('#submit-btn').click();
        cy.get('#MinisterApproval').click();
        cy.get('#LetterSent').click();
        cy.get('#LetterSaved').click();
        cy.get('#submit-btn').click();
        cy.get('#schoolclosedorclosing-checkbox').click();
        cy.get('#schoolclosedorclosing-txtarea').type('Cypress Test Fahad - Reason school is closing');
        cy.get('#submit-btn').click();
        cy.get('#decision-maker-name').clear();
        cy.get('#decision-maker-name').type('Fahad Darwish');
        cy.get('#submit-btn').click();
        cy.get('#submit-btn').click();
        this.verifyDecisionDetailsAfterChanging('DAO', 'Minister', '12 November 2023');
        cy.get('#submit-btn').click();
        return this;
    }
    deleteProject(projectId) {
        cy.callAcademisationApi("DELETE", `/conversion-project/${projectId}/Delete`).then((response) => {
            expect(response.status).to.eq(200);
        });
    }

    clickConfirmProjectDates() {
        cy.get('[data-cy="confirm_project_dates"]').click();
        return this;    
    }

    // verifyReadOnlyAfterDecision() {
    //     // Check advisory board and conversion dates to make sure the change option no longer exists
    //     cy.get('[data-test="change-advisory-board-date"]').should('not.be.visible');
    //     return this;
    // }

    goBackToTaskList() {
        cy.get('[data-cy="back-link"]').click();
        return this;
    }

    checkReadOnlyOnSchoolInformation() {
        // Verify each section in "Add school information" to confirm no "Change" link is present
        cy.get('[data-cy="task-name"] a').each((link) => {
            cy.wrap(link).invoke('attr', 'href').then((href) => {
                cy.visit(href);
                // Ensure the change button is absent
                cy.get('a').contains('Change').should('not.exist');
                // Return to the task list after each check
                cy.get('a.govuk-back-link').click();
            });
        });
        return this;
    }

}


export const decisionPage = new DecisionPage();
