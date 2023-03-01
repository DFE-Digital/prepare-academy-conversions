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
import sqlServer from 'cypress-sql-server';
sqlServer.loadDBCommands();

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
})

// Submit Date 'Return' (LA info Page)
Cypress.Commands.add('submitDateLaInfoReturn', (day, month, year) => {
    cy.get('[id="la-info-template-returned-date-day"]').should('be.visible')
    cy.get('[id="la-info-template-returned-date-day"]').clear().type(day)
    cy.get('[id="la-info-template-returned-date-month"]').clear().type(month)
    cy.get('[id="la-info-template-returned-date-year"]').clear().type(year)
})

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
    cy.get('[id="confirm-and-continue-button"]').click()
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

// Record a decision 'Continue' button
Cypress.Commands.add('continueBtn', () => {
    cy.get('[id="submit-btn"]')
})

// Record a decision 'date entry'
Cypress.Commands.add('recordDecisionDate', (day, month, year) => {
    cy.get('[id="decision-date-day"]').clear().type(day)
    cy.get('[id="decision-date-month"]').clear().type(month)
    cy.get('[id="decision-date-year"]').clear().type(year)
})

// Change decision button
Cypress.Commands.add('changeDecision', () => {
    cy.get('[id="record-decision-link"]')
})


// Approved No Btn
Cypress.Commands.add('NoRadioBtn', () => {
    cy.get('[id="no-radio"]')
})

// Approved Yes Btn
Cypress.Commands.add('YesRadioBtn', () => {
    cy.get('[id="yes-radio"]')
})

// Approved Changed Condition
Cypress.Commands.add('ChangeConditionsLink', () => {
    cy.get('[id="change-conditions-set-btn"]')
})

// Approved Yes Condition text box
Cypress.Commands.add('YesTextBox', () => {
    cy.get('[id="ApprovedConditionsDetails"]')
})

// Approved Decision Preview
Cypress.Commands.add('ApprovedDecisionPreview', () => {
    cy.get('[id="decision"]')
})

// Approved Decision Made By
Cypress.Commands.add('ApprovedMadeByPreview', () => {
    cy.get('[id="decision-made-by"]')
})

// Approved Conditions Set
Cypress.Commands.add('ApprovedConditionDetails', () => {
    cy.get('[id="condition-details"]')
})

// Approved Conditions detail
Cypress.Commands.add('AprrovedConditionsSet', () => {
    cy.get('[id="condition-set"]')
})

// Approved Conditions detail
Cypress.Commands.add('ApprovedDecisionDate', () => {
    cy.get('[id="decision-date"]')
})

// Approved Decision Recorded Banner
Cypress.Commands.add('ApprovedMessageBanner', () => {
    cy.get('[id="notification-message"]')
})

// Decline reasons finance box
Cypress.Commands.add('declineFinancebox', () => {
    cy.get('[id="declined-reasons-finance"]')
})

// Decline reasons finance reasons text
Cypress.Commands.add('declineFinancText', () => {
    cy.get('[id="reason-finance"]')
})

// Decline reasons performance box
Cypress.Commands.add('performanceBox', () => {
    cy.get('[id="declined-reasons-performance"]')
})

// Decline reasons performance text
Cypress.Commands.add('performanceText', () => {
    cy.get('[id="reason-performance"]')
})

// Decline reasons governance box
Cypress.Commands.add('governanceBox', () => {
    cy.get('[id="declined-reasons-governance"]')
})

// Decline reasons governance text
Cypress.Commands.add('governanceText', () => {
    cy.get('[id="reason-governance"]')
})

// Decline reasons Trust Box
Cypress.Commands.add('trustBox', () => {
    cy.get('[id="declined-reasons-choiceoftrust"]')
})

// Decline reasons Trust text
Cypress.Commands.add('trustText', () => {
    cy.get('[id="reason-choiceoftrust"]')
})

// Decline radio button
Cypress.Commands.add('declineRadioBtn', () => {
    cy.get('[id="declined-radio"]')
})

// declined reason 'Other'
Cypress.Commands.add('declineOtherbox', () => {
    cy.get('[id="declined-reasons-other"]')
})

// declined reason 'Other reason'
Cypress.Commands.add('declineOthertxt', () => {
    cy.get('[id="reason-other"]')
})

// initial decision preview
Cypress.Commands.add('decision', () => {
    cy.get('[id="decision"]')
})

// declined record notification Message
Cypress.Commands.add('recordnoteMsg', () => {
    cy.get('[id="notification-message"]')
})

// declined reason change link
Cypress.Commands.add('reasonchangeLink', () => {
    cy.get('[id="change-declined-btn"]')

})

// declined decision made by
Cypress.Commands.add('decisionMadeBy', () => {
    cy.get('[id="decision-made-by"]')

})

// declined decision date preview
Cypress.Commands.add('decisionDate', () => {
    cy.get('[id="decision-date"]')

})

// Deferred declined radio button
Cypress.Commands.add('deferredRadioBtn', () => {
    cy.get('[id="deferred-radio"]')
})

// Deferred Additional Information Needed Checkbox
Cypress.Commands.add('addInfoNeededBox', () => {
    cy.get('[id="additionalinformationneeded-checkbox"]')
})

// Deferred Additional Information Needed Textbox
Cypress.Commands.add('addInfoNeededText', () => {
    cy.get('[id="additionalinformationneeded-txtarea"]')
})

// Deferred reasonChangeLink
Cypress.Commands.add('deferredReasonChangeLink', () => {
    cy.get('[id="change-deferred-btn"]')
})

// Deferred Decision
Cypress.Commands.add('deferredDecision', () => {
    cy.get('[id="decision"]')
})

// Deferred Decision Made By
Cypress.Commands.add('deferredDecisionMadeBy', () => {
    cy.get('[id="decision-made-by"]')
})

// Deferred Decision Date
Cypress.Commands.add('deferredDecisionDate', () => {
    cy.get('[id="decision-date"]')
})

// Deferred project status
Cypress.Commands.add('deferredProjectStateId', () => {
    cy.get('[id="notification-message"]')
})

// Deferred Await Ofsted Report Checkbox
Cypress.Commands.add('awaitOfstedReportBox', () => {
    cy.get('[id="awaitingnextofstedreport-checkbox"]')
})

// Deferred Await Ofsted Report Text
Cypress.Commands.add('awaitOfstedReportText', () => {
    cy.get('[id="awaitingnextofstedreport-txtarea"]')
})

// Deferred Performance Check box
Cypress.Commands.add('performanceCheckBox', () => {
    cy.get('[id="performanceconcerns-checkbox"]')
})

// Deferred Performance Check box text
Cypress.Commands.add('performanceCheckText', () => {
    cy.get('[id="performanceconcerns-txtarea"]')
})

// Deferred Other box
Cypress.Commands.add('OtherCheckBox', () => {
    cy.get('[id="other-checkbox"]')
})

// Deferred Other text
Cypress.Commands.add('OtherCheckText', () => {
    cy.get('[id="other-txtarea"]')
})

//--Legal Requirements (Task List)

// Governing Body: Change Link
Cypress.Commands.add('govBodyChangeLink', () => {
    cy.get('[data-cy="select-legal-summary-governingbody-change"]').click()
})

// Save & Continue btn (Universal)
Cypress.Commands.add('saveAndContinueButton', () => {
    cy.get('[data-cy="select-common-submitbutton"]')
})

// Governing Body: status
Cypress.Commands.add('govBodyStatus', () => {
    cy.get('[data-cy="select-legal-summary-governingbody-status"]')
})

// Consulation: Change Link
Cypress.Commands.add('consultationChangeLink', () => {
    cy.get('[data-cy="select-legal-summary-consultation-change"]').click()
})

// Consultation: Status
Cypress.Commands.add('consultationStatus', () => {
    cy.get('[data-cy="select-legal-summary-consultation-status"]')
})

// Diocesan consent: Change Link
Cypress.Commands.add('diocesanConsentChangeLink', () => {
    cy.get('[data-cy="select-legal-summary-diocesanconsent-change"]').click()
})

// Diocesan consent: Status
Cypress.Commands.add('diocesanConsentStatus' ,() => {
    cy.get('[data-cy="select-legal-summary-diocesanconsent-status"]')
})

// Foundation consent: Change Link
Cypress.Commands.add('foundataionConsentChange', () => {
    cy.get('[data-cy="select-legal-summary-foundationconsent-change"]').click()
})

// Foundation consent: Status
Cypress.Commands.add('foundationConsentStatus', () => {
    cy.get('[data-cy="select-legal-summary-foundationconsent-status"]')
})

// Universal: selects conversion project from list
Cypress.Commands.add('selectsConversion', () => {
    let url = Cypress.env('url');
    cy.visit(url);
    cy.get('[data-cy="select-projecttype-input-conversion"]').click();
    cy.get('[data-cy="select-common-submitbutton"]').click();
})

// Universal: selects first project from list
Cypress.Commands.add('selectFirstProject', () => {
    let url = Cypress.env('url');
    cy.visit(url);
    cy.get('[data-cy="select-projecttype-input-conversion"]').click();
    cy.get('[data-cy="select-common-submitbutton"]').click();
    cy.get('[id="school-name-0"]').click();
})

// Assign User to project

// Unassign a user
Cypress.Commands.add('unassignUser', () => {
    cy.get('[data-id="assigned-user"]')
    .invoke('text')
    .then((text) => {
        if (text.includes('Empty')) {
            return
        }
        else {
            // assign link
            cy.get('a[href*="project-assignment"]').click()
            // unassign link
            cy.get('[id="unassign-link"]').click()
            // continue button
            cy.get('[class="govuk-button"]').click()
        }
    })
})

// Assign User

Cypress.Commands.add('assignUser', () => {
    cy.get('[data-id="assigned-user"]')
    .invoke('text')
    .then((text) => {
        if (text.includes('Empty')) {
            cy.get('a[href*="project-assignment"]').click()
            cy.get('[id="delivery-officer"]').click().type('Chris Sherlock').type('{enter}')
            cy.get('[class="govuk-button"]').click()
        }
    })
})

//Navigate To Filter Projects section
Cypress.Commands.add('navigateToFilterProjects',() => {  
    cy.get('[data-cy="select-projectlist-filter-expand"]').click();
    cy.get('[data-cy="select-projectlist-filter-clear"]').click();
    cy.get('[data-cy="select-projectlist-filter-expand"]').click();
    cy.get('[data-id="filter-container"]').should('be.visible');
  });

  // Submit End of current financial Date (School Budget Info Page)
Cypress.Commands.add('submitEndOfCurrentFinancialYearDate', (day, month, year) => {
	cy.get('[id="financial-year-day"]').should('be.visible');
    cy.get('[id="financial-year-day"]').clear().type(day);
	cy.get('[id="financial-year-month"]').clear().type(month);
	cy.get('[id="financial-year-year"]').clear().type(year);
	cy.saveLocalStorage();
})

// Submit End of next financial Date (School Budget info Page)
Cypress.Commands.add('submitEndOfNextFinancialYearDate', (day, month, year) => {
    cy.get('[id="next-financial-year-day"]').should('be.visible');
    cy.get('[id="next-financial-year-day"]').clear().type(day);
    cy.get('[id="next-financial-year-month"]').clear().type(month);
    cy.get('[id="next-financial-year-year"]').clear().type(year);
})

// End of Current Financial Year Date (School Budget info Page)
Cypress.Commands.add('endOfCurrentFinancialYearInfo', () => {
    cy.get('[id="financial-year"]')
})

// End of Next Financial Year Date (School Budget info Page)
Cypress.Commands.add('endOfNextFinancialYearInfo', () => {
    cy.get('[id="next-financial-year"]')
})

Cypress.Commands.add('clearFilters', () => {
    cy.get('[data-cy="select-projectlist-filter-clear"]').should('have.text', 'Clear filters') 
  cy.get('[data-cy="select-projectlist-filter-clear"]').click();
})

Cypress.Commands.add('excuteAccessibilityTests', (wcagStandards, continueOnFail, impactLevel) => {
    cy.injectAxe();
    cy.checkA11y(null, {
        runOnly: {
            type: 'tag',
            values: wcagStandards
        },
        includedImpacts: impactLevel
    }, null, continueOnFail);
})

Cypress.Commands.add('createInvoluntaryProject', () => {
  cy.get('[role="button"]').should('contain.text', "Start a new involuntary conversion project")
  cy.get('a[href="/start-new-project/school-name"]').click()
  cy.selectSchool()
  cy.selectTrust()
  cy.url().should('include', 'start-new-project/check-school-trust-details')
  cy.get('[data-id="submit"]').click()
  cy.url().should('include', 'project-list')
  cy.get('[id="school-name-0"]').should('include.text', 'Glo' )
  cy.get('#application-to-join-trust-0').should('include.text', 'CIT')
})

Cypress.Commands.add('selectSchool', () => {
    cy.url().should('include', '/start-new-project/school-name')
    cy.get('[id="SearchQuery"]').first().type('glo')
    cy.get('#SearchQuery__option--0').click();
    cy.get('[data-id="submit"]').click()
})

Cypress.Commands.add('selectTrust', () => {
    cy.url().should('include', 'start-new-project/trust-name')
    cy.get('[id="SearchQuery"]').first().type('cit')
    cy.get('#SearchQuery__option--1').click();
    cy.get('[data-id="submit"]').click()
})

Cypress.Commands.add('submitDateSNMReceivedForm', (day, month, year) => {
	cy.get('#form-7-received-date-day').should('be.visible')
	cy.get('#form-7-received-date-day').clear().type(day)
	cy.get('#form-7-received-date-month').clear().type(month)
	cy.get('#form-7-received-date-year').clear().type(year)
	cy.saveLocalStorage()
});

Cypress.Commands.add('submitDAODate', (day, month, year) => {
	cy.get('#dao-pack-sent-date-day').should('be.visible')
	cy.get('#dao-pack-sent-date-day').clear().type(day)
	cy.get('#dao-pack-sent-date-month').clear().type(month)
	cy.get('#dao-pack-sent-date-year').clear().type(year)
	cy.saveLocalStorage()
});
