import { Logger } from '../support/logger';
import FormBasePage from './formBasePage';

class DecisionPage extends FormBasePage {
    public path = 'decision';

    public clickRecordDecisionMenu(): this {
        cy.getByDataCy('record_decision_menu').click();
        return this;
    }

    public clickRecordDecision(): this {
        cy.getByDataCy('record_decision_btn').click();
        return this;
    }

    public clickRecordDecisionWithoutError(): this {
        cy.getByDataCy('record_decision_btn').click();
        return this;
    }

    public checkErrorAndAddDetails(day: string, month: string, year: string, userName: string): this {
        cy.get('.govuk-error-summary').should('be.visible');
        cy.getById('error-summary-title').should('contain', 'There is a problem');
        cy.getByDataCy('error-summary')
            .find('a')
            .should('have.length', 4)
            .eq(0)
            .should(
                'contain',
                'You must enter the name of the person who worked on this project before you can record a decision.'
            )
            .and('be.visible');
        cy.getByDataCy('error-summary')
            .find('a')
            .eq(1)
            .should('contain', 'You must enter a proposed conversion date before you can record a decision.')
            .and('be.visible');
        cy.getByDataCy('error-summary')
            .find('a')
            .eq(2)
            .should('contain', 'You must enter trust name before you can record a decision.')
            .and('be.visible');
        cy.getByDataCy('error-summary')
            .find('a')
            .eq(3)
            .should('contain', 'You must enter an advisory board date before you can record a decision.')
            .and('be.visible');

        // Clicking and filling fields as per the error messages
        cy.contains('a', 'You must enter an advisory board date before you can record a decision').click();
        cy.getById('head-teacher-board-date-day').type(day);
        cy.getById('head-teacher-board-date-month').type(month);
        cy.getById('head-teacher-board-date-year').type(year);
        cy.getByDataCy('select-common-submitbutton').click();
        cy.getById('approved-radio').click();
        cy.clickSubmitBtn();

        cy.contains(
            'a',
            'You must enter the name of the person who worked on this project before you can record a decision.'
        ).click();
        cy.getById('delivery-officer').type(userName);
        cy.get('.autocomplete__option').first().click();
        cy.getByDataCy('continue-Btn').click();

        // Verifying notification messages
        cy.getById('notification-message').should('include.text', 'Project is assigned');

        cy.getById('approved-radio').click();
        cy.clickSubmitBtn();

        cy.contains('a', 'You must enter a proposed conversion date before you can record a decision.').click();
        cy.getById('proposed-conversion-month').type(month);
        cy.getById('proposed-conversion-year').type(year);
        cy.getByDataCy('select-common-submitbutton').click();

        cy.getById('approved-radio').click();
        cy.clickSubmitBtn();

        cy.contains('a', 'You must enter trust name before you can record a decision.').click();
        cy.get('.autocomplete__wrapper > #SearchQuery').type('10058252');
        cy.getById('SearchQuery__option--0').click();
        cy.get('button.govuk-button[data-id="submit"]').click();

        // go back to the
        cy.getByDataCy('select-backlink').click();

        return this;
    }

    public decisionMaker(grade: string): this {
        cy.getById(`${grade}-radio`).click();
        cy.clickSubmitBtn();
        return this;
    }

    public selectNoConditions(): this {
        cy.getById('no-radio').click();
        cy.clickSubmitBtn();
        return this;
    }

    public selectReasonWhyDeferred(): this {
        cy.getById('additionalinformationneeded-checkbox').click();
        cy.getById('additionalinformationneeded-txtarea').type(
            'Fahads Cypress Reason is Additional Information Needed'
        );
        cy.clickSubmitBtn();
        return this;
    }

    public dateAOWasSent(day: string, month: string, year: string): this {
        cy.getById('academy-order-date-day').type(day);
        cy.getById('academy-order-date-month').type(month);
        cy.getById('academy-order-date-year').type(year);
        cy.clickSubmitBtn();
        return this;
    }

    public verifyDecisionDetails(decision: string, grade: string, name: string, date: string): this {
        Logger.log(`Decision: ${decision}, Name: ${name}, Date: ${date}`);
        cy.log(`Decision: ${decision}, Name: ${name}, Date: ${date}`);
        cy.getById('decision')
            .invoke('text')
            .then((text) => {
                expect(text.trim()).to.contain(decision);
            });
        cy.getById('decision-made-by').eq(0).should('contain', grade);
        cy.getById('decision-maker-name').should('contain', name);
        cy.getById('decision-date').should('contain', date);
        cy.clickSubmitBtn();
        return this;
    }

    public verifyDecisionDetailsBeforeChanging(decision: string, name: string, date: string): this {
        Logger.log(`Decision: ${decision}, Name: ${name}, Date: ${date}`);
        cy.log(`Decision: ${decision}, Name: ${name}, Date: ${date}`);
        cy.getById('decision')
            .invoke('text')
            .then((text) => {
                expect(text.trim().toLowerCase()).to.contain(decision.toLowerCase().trim());
            });
        cy.getById('decision-maker-name').should('contain', name);
        cy.getById('decision-date').should('contain', date);
        return this;
    }

    public verifyDecisionDetailsAfterChanging(decision: string, name: string, date: string): this {
        cy.getById('decision')
            .invoke('text')
            .then((text) => {
                expect(text.trim().toLowerCase()).to.contain(decision.toLowerCase().trim());
            });
        cy.getById('decision-made-by').should('contain', name);
        console.log('Date:', date);
        cy.getById('decision-date').should('contain', date);
        return this;
    }

    public changeDecisionDetails(): this {
        cy.getByDataCy('record_decision_menu').click();
        this.verifyDecisionDetailsBeforeChanging('DEFERRED', 'Fahad Darwish', '12 December 2023');
        cy.getById('record-decision-link').click();
        cy.getById('declined-radio').click();
        cy.clickSubmitBtn();
        cy.getById('directorgeneral-radio').click();
        cy.clickSubmitBtn();
        cy.clickSubmitBtn();
        cy.getById('declined-reasons-finance').click();
        cy.getById('reason-finance').type('Fahads Cypress Reason is Finance');
        cy.clickSubmitBtn();
        cy.clickSubmitBtn();

        //change the date to 12th November 2023
        cy.getById('decision-date-day').clear();
        cy.getById('decision-date-day').type('12');
        cy.getById('decision-date-month').clear();
        cy.getById('decision-date-month').type('11');
        cy.getById('decision-date-year').clear();
        cy.getById('decision-date-year').type('2023');
        cy.clickSubmitBtn();
        this.verifyDecisionDetailsAfterChanging('Declined', 'Director General', '12 November 2023');
        cy.clickSubmitBtn();
        return this;
    }

    public changeDecisionDAODetails(): this {
        cy.getByDataCy('record_decision_menu').click();
        cy.getById('record-decision-link').click();
        cy.getById('daorevoked-radio').click();
        cy.clickSubmitBtn();
        cy.clickSubmitBtn();
        cy.clickSubmitBtn();
        cy.getById('MinisterApproval').click();
        cy.getById('LetterSent').click();
        cy.getById('LetterSaved').click();
        cy.clickSubmitBtn();
        cy.getById('schoolclosedorclosing-checkbox').click();
        cy.getById('schoolclosedorclosing-txtarea').type('Cypress Test Fahad - Reason school is closing');
        cy.clickSubmitBtn();
        cy.getById('decision-maker-name').clear();
        cy.getById('decision-maker-name').type('Fahad Darwish');
        cy.clickSubmitBtn();
        cy.clickSubmitBtn();
        this.verifyDecisionDetailsAfterChanging('DAO', 'Minister', '12 November 2023');
        cy.clickSubmitBtn();
        return this;
    }

    public changeDecisionApproved(): this {
        cy.getByDataCy('record_decision_menu').click();
        cy.getById('record-decision-link').click();
        cy.getById('approved-radio').click();
        cy.clickSubmitBtn();
        cy.getById('minister-radio').click();
        cy.clickSubmitBtn();
        cy.getById('no-radio').click();
        cy.clickSubmitBtn();
        cy.getById('decision-maker-name').clear().type('Fahad Darwish');
        cy.clickSubmitBtn();
        cy.getById('decision-date-day').clear();
        cy.getById('decision-date-day').type('12');
        cy.getById('decision-date-month').clear();
        cy.getById('decision-date-month').type('11');
        cy.getById('decision-date-year').clear();
        cy.getById('decision-date-year').type('2023');
        cy.clickSubmitBtn();
        this.verifyDecisionDetailsAfterChanging('Approved', 'Minister', '12 November 2023');
        cy.clickSubmitBtn();
        cy.getByDataCy('decision-recorded-confirmation').should('be.visible');
        cy.getByDataCy('decision-recorded-confirmation').should('contain', 'Decision recorded');
        cy.getByDataCy('approved-body').should('contain', 'Manchester Academy').and('contain', 'Approved');

        cy.getByDataCy('open-in-compelete-btn').should('be.visible');
        return this;
    }

    public deleteProject(projectId: string): void {
        this.deleteConversionProject(projectId);
    }

    public clickToGoBackandCheckReadOnly(): this {
        cy.getByDataCy('select-backlink').click();
        cy.getByDataCy('record_decision_menu').click();
        cy.getByDataCy('approved-message_banner').should('be.visible').and('contain', 'This project was approved');

        return this;
    }

    public goBackToTaskList(): this {
        cy.getByDataCy('back-link').click();
        return this;
    }
}

const decisionPage = new DecisionPage();

export { decisionPage };
export default decisionPage;
