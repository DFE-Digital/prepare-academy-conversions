/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
let url = Cypress.env('url') + '/task-list/2054?rd=true'

describe('103791 Edit Declined journey', () => {
    beforeEach(() => {
        cy.clearCookies()
        cy.visit(url)
    })

    // Edit Approval Path - Regional Director, Finance 
    it('TC01: J2 Edit a recorded decision Declined - Reg Director Region, Finance', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
         //select iniital decision
        cy.get('[id="declined-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.get('[id="declined-reasons-finance"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // clicks on finance box
                    // clicks on the Give Reasons box
                cy.get('[id="reason-finance"]').clear().type('This is the second test')
    
            }
            else {
                cy.get('[id="declined-reasons-finance"]').click()
                .then(()=> {
                    // clicks on the Give Reasons box
                cy.get('[id="reason-finance"]').clear().type('This is the first test')
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
        cy.get('[id="change-declined-btn"]').click()
        cy.get('[id="declined-reasons-finance"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // clicks on finance box
                    // clicks on the Give Reasons box
                cy.get('[id="reason-finance"]').clear().type('This is the second test')
    
            }
            else {
                cy.get('[id="declined-reasons-finance"]').click()
                .then(()=> {
                    // clicks on the Give Reasons box
                cy.get('[id="reason-finance"]').clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.get('[id="decision"]').should('contain.text', 'Declined')
        cy.get('[id="decision-made-by"]').should('contain.text', 'Regional Director for the region')
        cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.get('[id="decision-date"]').should('contain.text', '10 October 2021')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.get('[id="notification-message"]').should('contain.text', 'Decision recorded')
    })

    // Edit Approval Path - A different Regional Director, Performance 
    it('TC02: J2 Edit a recorded decision Declined - Different Reg Director, Performance', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
         //select iniital decision
        cy.get('[id="declined-radio"]').click()
        
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="otherregionaldirector-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.get('[id="declined-reasons-performance"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // clicks on finance box
                    // clicks on the Give Reasons box
                cy.get('[id="reason-performance"]').clear().type('This is the second test')

            }
            else {
                cy.get('[id="declined-reasons-performance"]').click()
                .then(() => {
                    // clicks on the Give Reasons box
                cy.get('[id="reason-performance"]').clear().type('This is the first test')
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
        cy.get('[id="change-declined-btn"]').click()
        cy.get('[id="declined-reasons-performance"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // clicks on finance box
                    // clicks on the Give Reasons box
                cy.get('[id="reason-performance"]').clear().type('This is the second test')

            }
            else {
                cy.get('[id="declined-reasons-performance"]').click()
                .then(() => {
                    // clicks on the Give Reasons box
                cy.get('[id="reason-performance"]').clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.get('[id="decision"]').should('contain.text', 'Declined')
        cy.get('[id="decision-made-by"]').should('contain.text', 'A different Regional Director')
        cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.get('[id="decision-date"]').should('contain.text', '10 October 2021')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.get('[id="notification-message"]').should('contain.text', 'Decision recorded')
        
    })
    
    // Edit Approval Path - Director General, Governance
    it('TC03: J2 Edit a recorded decision Declined - Director General, Governance', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()
         //select iniital decision
         cy.get('[id="declined-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="directorgeneral-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()

        cy.get('[id="declined-reasons-governance"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                cy.get('[id="reason-governance"]').clear().type('This is the second test')
            }
            else {
                 // clicks on finance box
                cy.get('[id="declined-reasons-governance"]').click()
                .then(()=> {
                // clicks on the Give Reasons box
                cy.get('[id="reason-governance"]').clear().type('This is the first test')
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
        cy.get('[id="change-declined-btn"]').click()
        cy.get('[id="declined-reasons-governance"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                cy.get('[id="reason-governance"]').clear().type('This is the second test')
            }
            else {
                 // clicks on finance box
                cy.get('[id="declined-reasons-governance"]').click()
                .then(()=> {
                // clicks on the Give Reasons box
                cy.get('[id="reason-governance"]').clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.get('[id="decision"]').should('contain.text', 'Declined')

        cy.get('[id="decision-made-by"]').should('contain.text', 'Director General')

        cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.get('[id="decision-date"]').should('contain.text', '10 October 2021')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.get('[id="notification-message"]').should('contain.text', 'Decision recorded')
    })

    // Edit Approval Path - Minister, Choice of trust
    it('TC04: J2 Edit a recorded decision Declined - Minister, Choice of trust', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()

         //select iniital decision
         cy.get('[id="declined-radio"]').click()

        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="minister-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()

        cy.get('[id="declined-reasons-choiceoftrust"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                cy.get('[id="reason-choiceoftrust"]').clear().type('This is the second test')
            }
            else {
                // clicks on finance box
                cy.get('[id="declined-reasons-choiceoftrust"]').click()
                .then(()=> {
                // clicks on the Give Reasons box
                cy.get('[id="reason-choiceoftrust"]').clear().type('This is the first test')
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
        cy.get('[id="change-declined-btn"]').click()
        cy.get('[id="declined-reasons-choiceoftrust"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                cy.get('[id="reason-choiceoftrust"]').clear().type('This is the second test')
            }
            else {
                // clicks on finance box
                cy.get('[id="declined-reasons-choiceoftrust"]').click()
                .then(()=> {
                // clicks on the Give Reasons box
                cy.get('[id="reason-choiceoftrust"]').clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.get('[id="decision"]').should('contain.text', 'Declined')
        cy.get('[id="decision-made-by"]').should('contain.text', 'Minister')
        cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.get('[id="decision-date"]').should('contain.text', '10 October 2021')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.get('[id="notification-message"]').should('contain.text', 'Decision recorded')
    })

    // Edit Approval Path - None, Other
    it('TC05: J2 Edit a recorded decision Declined - None, Other', () => {
        // Click on change your decision button 
        cy.get('[id="record-decision-link"]').should('contain.text', 'Change your decision').click()

         //select iniital decision
         cy.get('[id="declined-radio"]').click()

        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="none-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()

        cy.get('[id="declined-reasons-other"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // clicks on the Give Reasons box
                cy.get('[id="reason-other"]').clear().type('This is the second test')

            }
            else {
                // clicks on finance box
                cy.get('[id="declined-reasons-other"]').click()
                .then(()=> {
                // clicks on the Give Reasons box
                cy.get('[id="reason-other"]').clear().type('This is the first test')
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
        cy.get('[id="change-declined-btn"]').click()
        cy.get('[id="declined-reasons-other"]')
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // clicks on the Give Reasons box
                cy.get('[id="reason-other"]').clear().type('This is the second test')

            }
            else {
                // clicks on finance box
                cy.get('[id="declined-reasons-other"]').click()
                .then(()=> {
                // clicks on the Give Reasons box
                cy.get('[id="reason-other"]').clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.get('[id="decision"]').should('contain.text', 'Declined')

        cy.get('[id="decision-made-by"]').should('contain.text', 'None')

        cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.get('[id="decision-date"]').should('contain.text', '10 October 2021')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.get('[id="notification-message"]').should('contain.text', 'Decision recorded')
    })
})