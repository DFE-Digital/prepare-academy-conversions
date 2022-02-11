// ***********************************************
// This example commands.js shows you how to
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
import "cypress-localstorage-commands";

Cypress.Commands.add("login",()=> {
	cy.visit(Cypress.env('url')+"/login");
	cy.get("#username").type(Cypress.env('username'));
	cy.get("#password").type(Cypress.env('password')+"{enter}");
	cy.saveLocalStorage();
})

Cypress.Commands.add("submitDate", (day, month, year) => {
	cy.get("#head-teacher-board-date-day").should('be.visible')
	cy.get("#head-teacher-board-date-day").clear().type(day);
	cy.get("#head-teacher-board-date-month").clear().type(month);
	cy.get("#head-teacher-board-date-year").clear().type(year);
	cy.saveLocalStorage();
});

Cypress.Commands.add("submitDateLaInfoSent", (day, month, year) => {
	cy.get('[id="la-info-template-sent-date-day"]').should('be.visible')
    cy.get('[id="la-info-template-sent-date-day"]').clear().type(day);
	cy.get('[id="la-info-template-sent-date-month"]').clear().type(month);
	cy.get('[id="la-info-template-sent-date-year"]').clear().type(year);
	cy.saveLocalStorage();
});

Cypress.Commands.add("submitDateLaInfoReturn", (day, month, year) => {
    cy.get('[id="la-info-template-returned-date-day"]').should('be.visible')
    cy.get('[id="la-info-template-returned-date-day"]').clear().type(day)
    cy.get('[id="la-info-template-returned-date-month"]').clear().type(month)
    cy.get('[id="la-info-template-returned-date-year"]').clear().type(year)
});

Cypress.Commands.add('storeSessionData',()=>{
    Cypress.Cookies.preserveOnce('.ManageAnAcademyConversion.Login')
    let str = [];
    cy.getCookies().then((cookie) => {
        cy.log(cookie);
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
})

Cypress.Commands.add('selectSchoolListing',(listing)=>{
    cy.get("#school-name-"+listing).click();
    cy.get('*[href*="/confirm-school-trust-information-project-dates"]').should(
        "be.visible"
    );
    cy.saveLocalStorage();
})