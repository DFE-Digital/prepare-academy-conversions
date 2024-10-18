import { Logger } from "../support/logger";

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
        cy.get('[data-test="record_decision_error_btn"]').click();
        return this;
    }

    clickRecordDecisionWithoutError() {
        cy.get('[data-cy="record_decision_btn"]').click();
        return this;
    }


    checkErrorAndAddDetails(day, month, year, userName) {

        cy.get('#errorSummary').should('be.visible');
        cy.get('#error-summary-title').should('contain', 'There is a problem');
        cy.get('[data-cy="error-summary"]')
            .find('a')
            .should('have.length', 2)
            .eq(0)
            .should('contain', 'You must enter an advisory board date before you can record a decision.')
            .and('be.visible');
        cy.get('[data-cy="error-summary"]')
            .find('a')
            .eq(1)
            .should('contain', 'You must enter the name of the person who worked on this project before you can record a decision.')
            .and('be.visible');
        cy.contains('a', 'You must enter an advisory board date before you can record a decision').click();
        cy.get('#head-teacher-board-date-day').type(day);
        cy.get('#head-teacher-board-date-month').type(month);
        cy.get('#head-teacher-board-date-year').type(year);
        cy.get('[data-cy="select-common-submitbutton"]').click();

        cy.get('[data-cy="record_decision_menu"] > .moj-sub-navigation__link').click();


        cy.get('[data-test="record_decision_error_btn"]').click();
        cy.contains('a', 'You must enter the name of the person who worked on this project before you can record a decision.').click();
        cy.get('#delivery-officer').type(userName);
        cy.get('.autocomplete__option').first().click();
        cy.get('[data-cy="continue-Btn"]').click();


        cy.get('#notification-message').should('include.text', 'Project is assigned');
        cy.get('#main-content > :nth-child(2) > :nth-child(2)').should('include.text', userName);

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

    selectReasonWhyDeferred() {
        cy.get('#additionalinformationneeded-checkbox').click();
        cy.get('#additionalinformationneeded-txtarea').type('Fahads Cypress Reason is Additional Information Needed');
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

    verifyDecisionDetails(decision, grade, name, date) {
        Logger.log(`Decision: ${decision}, Name: ${name}, Date: ${date}`);
        cy.log(`Decision: ${decision}, Name: ${name}, Date: ${date}`);
        cy.get('#decision')
            .invoke('text')
            .then((text) => {
                expect(text.trim()).to.contain(decision);
            });
        cy.get('#decision-made-by').eq(0).should('contain', grade);
        cy.get('#decision-maker-name').should('contain', name);
        cy.get('#decision-date').should('contain', date);
        cy.get('#submit-btn').click();
        return this;
    }

    verifyDecisionDetailsBeforeChanging(decision, name, date) {
        Logger.log(`Decision: ${decision}, Name: ${name}, Date: ${date}`);
        cy.log(`Decision: ${decision}, Name: ${name}, Date: ${date}`);
        cy.get('#decision')
            .invoke('text')
            .then((text) => {
                expect(text.trim().toLowerCase()).to.contain(decision.toLowerCase().trim());
            });
        cy.get('#decision-maker-name').should('contain', name);
        cy.get('#decision-date').should('contain', date);
        return this;
    }

    verifyDecisionDetailsAfterChanging(decision, name, date) {
        cy.get('#decision')
            .invoke('text')
            .then((text) => {
                expect(text.trim().toLowerCase()).to.contain(decision.toLowerCase().trim());
            });
        cy.get('#decision-made-by').should('contain', name);
        console.log('Date:', date);
        cy.get('#decision-date').should('contain', date);
        return this;
    }

    changeDecisionDetails() {
        cy.get('[data-cy="record_decision_menu"]').click();
        this.verifyDecisionDetailsBeforeChanging('DEFERRED', 'Fahad Darwish', '12 December 2023');
        cy.get('#record-decision-link').click();
        cy.get('#declined-radio').click();
        cy.get('#submit-btn').click();
        cy.get('#directorgeneral-radio').click();
        cy.get('#submit-btn').click();
        cy.get('#submit-btn').click();
        cy.get('#declined-reasons-finance').click();
        cy.get('#reason-finance').type('Fahads Cypress Reason is Finance');
        cy.get('#submit-btn').click();
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


    changeDecisionApproved() {
        cy.get('[data-cy="record_decision_menu"]').click();
        cy.get('#record-decision-link').click();
        cy.get('#approved-radio').click();
        cy.get('#submit-btn').click();
        cy.get('#minister-radio').click();
        cy.get('#submit-btn').click();
        cy.get('#no-radio').click();
        cy.get('#submit-btn').click();
        cy.get('#decision-maker-name').clear().type('Fahad Darwish');
        cy.get('#submit-btn').click();
        cy.get('#decision-date-day').clear();
        cy.get('#decision-date-day').type('12');
        cy.get('#decision-date-month').clear();
        cy.get('#decision-date-month').type('11');
        cy.get('#decision-date-year').clear();
        cy.get('#decision-date-year').type('2023');
        cy.get('#submit-btn').click();
        this.verifyDecisionDetailsAfterChanging('Approved', 'Minister', '12 November 2023');
        cy.get('#submit-btn').click();
        cy.get('[data-cy="decision-recorded-confirmation"]')
            .should('be.visible');
        cy.get('[data-cy="decision-recorded-confirmation"]').should('contain', 'Decision recorded');
        cy.get('[data-cy="approved-body"]').should('contain', 'Manchester Academy')
            .and('contain', 'Approved');

        cy.get('[data-cy="open-in-compelete-btn"]')
            .should('be.visible');
        return this;
    }
    deleteProject(projectId) {
        cy.callAcademisationApi("DELETE", `/conversion-project/${projectId}/Delete`).then((response) => {
            expect(response.status).to.eq(200);
        });
    }

    clickToGoBackandCheckReadOnly() {
        cy.get('[data-cy="select-backlink"]').click();
        cy.get('[data-cy="record_decision_menu"]').click();
        cy.get('[data-cy="approved-message_banner"]')
            .should('be.visible')
            .and('contain', 'This project was approved');

        return this;
    }



    goBackToTaskList() {
        cy.get('[data-cy="back-link"]').click();
        return this;
    }


}


export const decisionPage = new DecisionPage();
