/// <reference types ='Cypress'/>
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
import 'cypress-localstorage-commands'

//--Universal 

Cypress.Commands.add('login',() => {
	cy.visit(`${Cypress.env('url')}${'/project-list'}`)
});

// Preserving Session Data (Universal)
Cypress.Commands.add('storeSessionData', () => {
	Cypress.Cookies.preserveOnce('.ManageAnAcademyConversion.Login')
	let str = []
	cy.getCookies().then((cookie) => {
		cy.log(cookie)
		for (let l = 0; l < cookie.length; l++) {
			if (cookie.length > 0 && l == 0) {
				str[l] = cookie[l].name
				Cypress.Cookies.preserveOnce(str[l])
			} else if (cookie.length > 1 && l > 1) {
				str[l] = cookie[l].name
				Cypress.Cookies.preserveOnce(str[l])
			};
		};
	})
});

// School Listing Summary Page (Universal)
Cypress.Commands.add('selectSchoolListing', (listing) => {
    cy.get('#school-name-'+listing).click()
    cy.get('*[href*="/confirm-school-trust-information-project-dates"]').should('be.visible')
    cy.saveLocalStorage()
});

// Save and Continue Button (Universal)
Cypress.Commands.add('saveContinueBtn', () => {
    cy.get('[id="save-and-continue-button"]')
});

// Confirm and Continue Button (Universal)
Cypress.Commands.add('confirmContinueBtn', () => {
    cy.get('[id="confirm-and-continue-button"]')
});

// Preview Project Template Button (Universal)
Cypress.Commands.add('previewProjectTempBtn', () => {
    cy.get('[id="preview-project-template-button"]')
});

// Generate Project Template Button (Universal)
Cypress.Commands.add('generateProjectTempBtn', () => {
    cy.get('[id="generate-project-template-button"]')
});

//--LA Info Page

// Submit Date 'Sent' (LA Info Page)
Cypress.Commands.add('submitDateLaInfoSent', (day, month, year) => {
	cy.get('[id="la-info-template-sent-date-day"]').should('be.visible')
    cy.get('[id="la-info-template-sent-date-day"]').clear().type(day)
	cy.get('[id="la-info-template-sent-date-month"]').clear().type(month)
	cy.get('[id="la-info-template-sent-date-year"]').clear().type(year)
	cy.saveLocalStorage()
});

// Submit Date 'Return' (LA info Page)
Cypress.Commands.add('submitDateLaInfoReturn', (day, month, year) => {
    cy.get('[id="la-info-template-returned-date-day"]').should('be.visible')
    cy.get('[id="la-info-template-returned-date-day"]').clear().type(day)
    cy.get('[id="la-info-template-returned-date-month"]').clear().type(month)
    cy.get('[id="la-info-template-returned-date-year"]').clear().type(year)
});

// Form Status (LA info Page)
Cypress.Commands.add('statusLaInfo', () => {
    cy.get('[id="la-info-template-status"]')
})

// Form Status is Complete (LA info Page)
Cypress.Commands.add('completeStatusLaInfo', () => {
    cy.get('[id="la-info-template-complete"]')
})

// Checkbox: unchecked (LA info Page)
Cypress.Commands.add('uncheckLaInfo', () => {
    cy.get('*[href*="/confirm-local-authority-information-template-dates"]').click()
    cy.get('[id="la-info-template-complete"]').click()
    cy.get('[id="save-and-continue-button"]').click()
})

// Commentbox (LA info Page)
Cypress.Commands.add('commentBoxLaInfo', () => {
    cy.get('[id="la-info-template-comments"]')
})

// Commentbox: Clear (LA info Page)
Cypress.Commands.add('commentBoxClearLaInfo', () => {
    cy.get('[data-test="change-la-info-template-comments"]').click()
    cy.get('[id="la-info-template-comments"]').clear()
    cy.get('[id="save-and-continue-button"]').click()
})

// Sent Date Summary (LA info Page)
Cypress.Commands.add('sentDateSummLaInfo', () => {
    cy.get('[id="la-info-template-sent-date"]')
})

// Returned Date Summary (LA info Page)
Cypress.Commands.add('returnDateSummLaInfo', () => {
    cy.get('[id="la-info-template-returned-date"]')
})

//--School Trust Info Page

// Checkbox: unchecked (School Trust Info Page)
Cypress.Commands.add('uncheckSchoolTrust', () => {
    cy.get('*[href*="/confirm-school-trust-information-project-dates"]').click()
    cy.completeStatusSchoolTrust().click()
    cy.confirmContinueBtn().click()
});

// Submit Date (School Trust Info Page)
Cypress.Commands.add('submitDateSchoolTrust', (day, month, year) => {
	cy.get('#head-teacher-board-date-day').should('be.visible')
	cy.get('#head-teacher-board-date-day').clear().type(day)
	cy.get('#head-teacher-board-date-month').clear().type(month)
	cy.get('#head-teacher-board-date-year').clear().type(year)
	cy.saveLocalStorage()
});

// Form Status (School Trust Info Page)
Cypress.Commands.add('statusSchoolTrust', () => {
    cy.get('[id=school-and-trust-information-status]')
});

// Form Status is Complete (School Trust Info Page)
Cypress.Commands.add('completeStatusSchoolTrust', () => {
    cy.get('[id="school-and-trust-information-complete"]')
});

//--General Info Page

// MP Name (General Info Page)
Cypress.Commands.add('mpName', () => {
    cy.get('[id="member-of-parliament-name"]')
});

// MP Party (General Info Page)
Cypress.Commands.add('mpParty', () => {
    cy.get('[id="member-of-parliament-party"]')
});

// Distance Inf (General Info Page) - Conditional: Miles box
Cypress.Commands.add('milesIsEmpty', () => {
    cy.get('[data-test="change-distance-to-trust-headquarters"]').click()
    cy.get('[id="distance-to-trust-headquarters"]')
    .invoke('text')
    .then((text) => {
        if (text.includes('Empty')) {
            return
        }
        else {
            cy.get('[id="distance-to-trust-headquarters"]').click()
            .type('{backspace}{backspace}{backspace}{backspace}{backspace}{backspace}{backspace}{backspace}{backspace}')
            .should('contain.text', '')
        }
    })
})

// Distance Info (General Info Page) - Conditional: Additional Info box
Cypress.Commands.add('addInfoIsEmpty', () => {
    cy.get('[id="distance-to-trust-headquarters-additional-information"]')
    .click()
    .invoke('text')
    .then((text) => {
        if (text.length, 1) {
            cy.get('[id="distance-to-trust-headquarters-additional-information"]')
            .click()
            .type('{backspace}{backspace}{backspace}{backspace}{backspace}{backspace}{backspace}{backspace}{backspace}')
            .should('contain.text', '')
            cy.get('[id="save-and-continue-button"]').click()
        }
        else {
            return
        }
    })
})

// Distance Info (General Info Page) - Change LinkI D
Cypress.Commands.add('changeLink', () => {
    cy.get('[data-test="change-distance-to-trust-headquarters"]')
})

// Distance Info (General Info Page) - Miles field ID
Cypress.Commands.add('disMiles', () => {
    cy.get('[id="distance-to-trust-headquarters"]')
})

// Distance Info (General Info Page) - Save & Continue Button ID
Cypress.Commands.add('saveContinue', () => {
    cy.get('[id="save-and-continue-button"]')
})

// Request external dev - requres environment setup on yml file
// Cypress.Commands.add('beData', () => {
//     const apiKey = Cypress.env('apiKey')
//     const beUrl = Cypress.env('beUrl')

//     cy.request({
//         method:'GET',
//         url: beUrl + '/v2/apply-to-become/application/A2B_1373',
//         headers: {
//             ApiKey: apiKey,
//             "Content-type" : "application/json"
//          }
//     })
