/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
let url = Cypress.env('url') + '/task-list/2054?rd=true'
let projecList = Cypress.env('url') + '/project-list'

describe(' Edit Deferred journey', () => {
    beforeEach(() => {
        cy.clearCookies()
        cy.visit(url)
    })

// Edit Deferred Path - Regional Director, Additional information needed 
it('TC01: J3 Edit a recorded decision Deferred - Reg Director Region, Finance', () => {
    // Click on change your decision button 
    cy.changeDecision().should('contain.text', 'Change your decision').click()
     //select iniital decision
    cy.deferredRadioBtn().click()
    // clicks on the continue button
    cy.continueBtn().click()
    cy.get('[id="regionaldirectorforregion-radio"]').click()
    // clicks on the continue button
    cy.continueBtn().click()
    cy.addInfoNeededBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            // clicks on finance box
                // clicks on the Give Reasons box
            cy.addInfoNeededText().clear().type('This is the second test')

        }
        else {
            cy.addInfoNeededBox().click()
            .then(()=> {
                // clicks on the Give Reasons box
            cy.addInfoNeededText().clear().type('This is the first test')
            })
        }
    })
    // clicks on the continue button
    cy.continueBtn().click()
    // date entry
    cy.recordDecisionDate(10, 10, 2021)
    // clicks on the continue button
    cy.continueBtn().click()
    // Change condition
    cy.deferredReasonChangeLink().click() 
    cy.addInfoNeededBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            // clicks on finance box
                // clicks on the Give Reasons box
            cy.addInfoNeededText().clear().type('This is the second test')

        }
        else {
            cy.addInfoNeededBox().click()
            .then(()=> {
                // clicks on the Give Reasons box
            cy.addInfoNeededText().clear().type('This is the first test')
            })
        }
    })
    cy.continueBtn().click()
    cy.continueBtn().click()
    // preview answers before submit
    cy.deferredDecision().should('contain.text', 'Deferred')
    cy.deferredDecisionMadeBy().should('contain.text', 'Regional Director for the region')
    //cy.get('[id="deferred-reasons"]').should($el => expect($el.text().trim()).to.equal('Additional information needed:\n                    This is the second test\n                    Awaiting next ofsted report:\n                    This is the second test\n                    Performance concerns:\n                    This is the second test\n                    Other:\n                    This is the second test'))
    cy.deferredDecisionDate().should('contain.text', '10 October 2021')
    // clicks on the record a decision button to submit
    cy.recordThisDecision().click()
    // recorded decision confirmation
    cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
    cy.visit(projecList)
    cy.projectStateId().should('contain.text', 'DEFERRED')
})

// Edit Deferred Path - A different Regional Director, ofsted report 
it('TC02: J3 Edit a recorded decision Declined - Different Reg Director, ofsted report', () => {
    // Click on change your decision button 
    cy.changeDecision().should('contain.text', 'Change your decision').click()
     //select iniital decision
    cy.deferredRadioBtn().click()
    // clicks on the continue button
    cy.continueBtn().click()
    // selects regional director button
    cy.get('[id="otherregionaldirector-radio"]').click()
    // clicks on the continue button
    cy.continueBtn().click()
    cy.awaitOfstedReportBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            // clicks on finance box
                // clicks on the Give Reasons box
            cy.awaitOfstedReportText().clear().type('This is the second test')

        }
        else {
            cy.awaitOfstedReportBox().click()
            .then(() => {
                // clicks on the Give Reasons box
            cy.awaitOfstedReportText().clear().type('This is the first test')
            })
        }
    })
    // clicks on the continue button
    cy.continueBtn().click()
    // date entry
    cy.recordDecisionDate(10, 10, 2021)
    // clicks on the continue button
    cy.continueBtn().click()
    // Change condition
    cy.deferredReasonChangeLink().click() 
    cy.awaitOfstedReportBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            // clicks on finance box
                // clicks on the Give Reasons box
            cy.awaitOfstedReportText().clear().type('This is the second test')
        }
        else {
            cy.awaitOfstedReportBox().click()
            .then(() => {
                // clicks on the Give Reasons box
            cy.awaitOfstedReportText().clear().type('This is the first test')
            })
        }
    })
    cy.continueBtn().click()
    cy.continueBtn().click()
    // preview answers before submit
    cy.deferredDecision().should('contain.text', 'Deferred')
    cy.deferredDecisionMadeBy().should('contain.text', 'A different Regional Director')
    //cy.get('[id="deferred-reasons"]').should($el => expect($el.text().trim()).to.equal('Additional information needed:\n                    This is the second test\n                    Awaiting next ofsted report:\n                    This is the second test\n                    Performance concerns:\n                    This is the second test\n                    Other:\n                    This is the second test'))
    cy.deferredDecisionDate().should('contain.text', '10 October 2021')
    // clicks on the record a decision button to submit
    cy.recordThisDecision().click()
    // recorded decision confirmation
    cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
    cy.visit(projecList)
    cy.projectStateId().should('contain.text', 'DEFERRED')
})

// Edit Deferred Path - Director General, Performance concerns
it('TC03: J3 Edit a recorded decision Declined - Director General, Performance', () => {
    // Click on change your decision button 
    cy.changeDecision().should('contain.text', 'Change your decision').click()
     //select iniital decision
    cy.deferredRadioBtn().click()
    // clicks on the continue button
    cy.continueBtn().click()
    // selects regional director button
    cy.get('[id="directorgeneral-radio"]').click()
    // clicks on the continue button
    cy.continueBtn().click()
    cy.performanceCheckBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            cy.performanceCheckText().clear().type('This is the second test')
        }
        else {
             // clicks on performance box
            cy.performanceCheckBox().click()
            .then(()=> {
            // clicks on the Give Reasons box
            cy.performanceCheckText().clear().type('This is the first test')
            })
        }
    })
    // clicks on the continue button
    cy.continueBtn().click()
    // date entry
    cy.recordDecisionDate(10, 10, 2021)
    // clicks on the continue button
    cy.continueBtn().click()
    // Change condition
    cy.deferredReasonChangeLink().click() 
    cy.performanceCheckBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            cy.performanceCheckText().clear().type('This is the second test')
        }
        else {
             // clicks on performance box
            cy.performanceCheckBox().click()
            .then(()=> {
            // clicks on the Give Reasons box
            cy.performanceCheckText().clear().type('This is the first test')
            })
        }
    })
    cy.continueBtn().click()
    cy.continueBtn().click()
    // preview answers before submit
    cy.deferredDecision().should('contain.text', 'Deferred')
    cy.deferredDecisionMadeBy().should('contain.text', 'Director General')
    //cy.get('[id="deferred-reasons"]').should($el => expect($el.text().trim()).to.equal('Additional information needed:\n                    This is the second test\n                    Awaiting next ofsted report:\n                    This is the second test\n                    Performance concerns:\n                    This is the second test\n                    Other:\n                    This is the second test'))
    cy.deferredDecisionDate().should('contain.text', '10 October 2021')
    // clicks on the record a decision button to submit
    cy.recordThisDecision().click()
    // recorded decision confirmation
    cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
    cy.visit(projecList)
    cy.projectStateId().should('contain.text', 'DEFERRED')
})

// Edit Deferred Path - Minister, Other
it('TC04: J3 Edit a recorded decision Declined - Minister, Other', () => {
    // Click on change your decision button 
    cy.changeDecision().should('contain.text', 'Change your decision').click()
     //select iniital decision
    cy.deferredRadioBtn().click()
    // clicks on the continue button
    cy.continueBtn().click()
    // selects regional director button
    cy.get('[id="minister-radio"]').click()
    // clicks on the continue button
    cy.continueBtn().click()
    cy.OtherCheckBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            cy.OtherCheckText().clear().type('This is the second test')
        }
        else {
            // clicks on other box
            cy.OtherCheckBox().click()
            .then(()=> {
            // clicks on the Give Reasons box
            cy.OtherCheckText().clear().type('This is the first test')
            })
        }
    })
    // clicks on the continue button
    cy.continueBtn().click()
    // date entry
    cy.recordDecisionDate(10, 10, 2021)
    // clicks on the continue button
    cy.continueBtn().click()
    // Change condition
    cy.deferredReasonChangeLink().click() 
    cy.OtherCheckBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            cy.OtherCheckText().clear().type('This is the second test')
        }
        else {
            // clicks on finance box
            cy.OtherCheckBox().click()
            .then(()=> {
            // clicks on the Give Reasons box
            cy.OtherCheckText().clear().type('This is the first test')
            })
        }
    })
    cy.continueBtn().click()
    cy.continueBtn().click()
    // preview answers before submit
    cy.deferredDecision().should('contain.text', 'Deferred')
    cy.deferredDecisionMadeBy().should('contain.text', 'Minister')
    //cy.get('[id="deferred-reasons"]').should($el => expect($el.text().trim()).to.equal('Additional information needed:\n                    This is the second test\n                    Awaiting next ofsted report:\n                    This is the second test\n                    Performance concerns:\n                    This is the second test\n                    Other:\n                    This is the second test'))
    cy.deferredDecisionDate().should('contain.text', '10 October 2021')
    // clicks on the record a decision button to submit
    cy.recordThisDecision().click()
    // recorded decision confirmation
    cy.deferredProjectStateId().should('contain.text', 'Decision recorded')
    cy.visit(projecList)
    cy.projectStateId().should('contain.text', 'DEFERRED')
})

// Edit Deferred Path - None, Other
it('TC05: J3 Edit a recorded decision Declined - None, Other', () => {
    // Click on change your decision button 
    cy.changeDecision().should('contain.text', 'Change your decision').click()
     //select iniital decision
    cy.deferredRadioBtn().click()
    // clicks on the continue button
    cy.continueBtn().click()
    // selects regional director button
    cy.get('[id="none-radio"]').click()
    // clicks on the continue button
    cy.continueBtn().click()
    cy.OtherCheckBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            cy.OtherCheckText().clear().type('This is the second test')
        }
        else {
            // clicks on finance box
            cy.OtherCheckBox().click()
            .then(()=> {
            // clicks on the Give Reasons box
            cy.OtherCheckText().clear().type('This is the first test')
            })
        }
    })
    // clicks on the continue button
    cy.continueBtn().click()
    // date entry
    cy.recordDecisionDate(10, 10, 2021)
    // clicks on the continue button
    cy.continueBtn().click()
    // Change condition
    cy.deferredReasonChangeLink().click() 
    cy.OtherCheckBox()
    .invoke('attr', 'aria-expanded')
    .then(ariaExpand => {
        if (ariaExpand.includes(true)) {
            cy.OtherCheckText().clear().type('This is the second test')
        }
        else {
            // clicks on finance box
            cy.OtherCheckBox().click()
            .then(()=> {
            // clicks on the Give Reasons box
            cy.OtherCheckText().clear().type('This is the first test')
            })
        }
    })
    cy.continueBtn().click()
    cy.continueBtn().click()
    // preview answers before submit
    cy.deferredDecision().should('contain.text', 'Deferred')
    cy.deferredDecisionMadeBy().should('contain.text', 'None')
    //cy.get('[id="deferred-reasons"]').should($el => expect($el.text().trim()).to.equal('Additional information needed:\n                    This is the second test\n                    Awaiting next ofsted report:\n                    This is the second test\n                    Performance concerns:\n                    This is the second test\n                    Other:\n                    This is the second test'))
    cy.deferredDecisionDate().should('contain.text', '10 October 2021')
    // clicks on the record a decision button to submit
    cy.recordThisDecision().click()
    // recorded decision confirmation
    cy.deferredProjectStateId().should('contain.text', 'Decision recorded')      
    cy.visit(projecList)
    cy.projectStateId().should('contain.text', 'DEFERRED')
    })
})