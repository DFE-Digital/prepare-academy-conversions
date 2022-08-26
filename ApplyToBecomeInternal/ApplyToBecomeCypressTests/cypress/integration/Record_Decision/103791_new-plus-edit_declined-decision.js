/// <reference types ='Cypress'/>

// uri to be updated once academisation API is integrated
let url = Cypress.env('url') + '/task-list/2054?rd=true'
let projecList = Cypress.env('url') + '/project-list'

describe('103791 Edit Declined journey', () => {
    beforeEach(() => {
        cy.DeclinedDeleteAddNewRecord()
        cy.clearCookies()
        cy.visit(url)
    })

    // Edit Approval Path - Regional Director, Finance 
    it('TC01: J2 Edit a recorded decision Declined - Reg Director Region, Finance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
         //select iniital decision
        cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.get('[id="regionaldirectorforregion-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.declineFinancebox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // clicks on the finance text box
                cy.declineFinancText().clear().type('This is the second test')
    
            }
            else {
                // clicks on the finance  box
                cy.declineFinancebox().click()
                .then(()=> {
                // clicks on the finance text box
                cy.declineFinancText().clear().type('This is the first test')
                })
            }
        })
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.declineFinancebox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                //clicks on the finance text box
                cy.declineFinancText().clear().type('This is the second test')
    
            }
            else {
                // clicks on the finance  box
                cy.declineFinancebox().click()
                .then(()=> {
                // clicks on the finance text box
                cy.declineFinancText().clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'Regional Director for the region')
        //**will have to review this once DB has been cleared */
        //cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.decisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DECLINED')
    })

    // Edit Approval Path - A different Regional Director, Performance 
    it('TC02: J2 Edit a recorded decision Declined - Different Reg Director, Performance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
         //select iniital decision
        cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="otherregionaldirector-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.performanceBox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // performance text box
                cy.performanceText().clear().type('This is the second test')
            }
            else {
                // performance  box
                cy.performanceBox().click()
                .then(() => {
                    // performance text box
                cy.performanceText().clear().type('This is the first test')
                })
            }
        })
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.performanceBox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // performance text box
                cy.performanceText().clear().type('This is the second test')

            }
            else {
                // performance box
                cy.performanceBox().click()
                .then(() => {
                // performance text box
                cy.performanceText().clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'A different Regional Director')
        //**will have to review this once DB has been cleared */
        //cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.decisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DECLINED')
        
    })
    
    // Edit Approval Path - Director General, Governance
    it('TC03: J2 Edit a recorded decision Declined - Director General, Governance', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
         //select iniital decision
         cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="directorgeneral-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.governanceBox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // governance text box
                cy.governanceText().clear().type('This is the second test')
            }
            else {
                 // governance box
                cy.governanceBox().click()
                .then(()=> {
                // governance text box
                cy.governanceText().clear().type('This is the first test')
                })
            }
        })
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.governanceBox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // governance text box
                cy.governanceText().clear().type('This is the second test')
            }
            else {
                 // governance box
                cy.governanceBox().click()
                .then(()=> {
                // governance text box
                cy.governanceText().clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'Director General')
        //**will have to review this once DB has been cleared */
        //cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.decisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DECLINED')
    })

    // Edit Approval Path - Minister, Choice of trust
    it('TC04: J2 Edit a recorded decision Declined - Minister, Choice of trust', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
         //select iniital decision
         cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="minister-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.trustBox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // trust text box
                cy.trustText().clear().type('This is the second test')
            }
            else {
                // trust box
                cy.trustBox().click()
                .then(()=> {
                // trust text box
                cy.trustText().clear().type('This is the first test')
                })
            }
        })
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.trustBox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // trust text box
                cy.trustText().clear().type('This is the second test')
            }
            else {
                // trust box
                cy.trustBox().click()
                .then(()=> {
                // trust text box
                cy.trustText().clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'Minister')
        //**will have to review this once DB has been cleared */
        //cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.decisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DECLINED')
    })

    // Edit Approval Path - None, Other
    it('TC05: J2 Edit a recorded decision Declined - None, Other', () => {
        // Click on change your decision button 
        cy.changeDecision().should('contain.text', 'Change your decision').click()
         //select iniital decision
         cy.declineRadioBtn().click()
        // clicks on the continue button
        cy.continueBtn().click()
        // selects regional director button
        cy.get('[id="none-radio"]').click()
        // clicks on the continue button
        cy.continueBtn().click()
        cy.declineOtherbox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // Other text box
                cy.declineOthertxt().clear().type('This is the second test')

            }
            else {
                // Other box
                cy.declineOtherbox().click()
                .then(()=> {
                // Other text box
                cy.declineOthertxt().clear().type('This is the first test')
                })
            }
        })
        // clicks on the continue button
        cy.continueBtn().click()
        // date entry
        cy.recordDecisionDate(10, 8, 2022)
        // clicks on the continue button
        cy.continueBtn().click()
        // Change condition
        cy.reasonchangeLink().click()
        cy.declineOtherbox()
        .invoke('attr', 'aria-expanded')
        .then(ariaExpand => {
            if (ariaExpand.includes(true)) {
                // Other text box
                cy.declineOthertxt().clear().type('This is the second test')

            }
            else {
                // Other box
                cy.declineOtherbox().click()
                .then(()=> {
                // Other text box
                cy.declineOthertxt().clear().type('This is the first test')
                })
            }
        })
        cy.continueBtn().click()
        cy.continueBtn().click()
        // preview answers before submit
        cy.decision().should('contain.text', 'Declined')
        cy.decisionMadeBy().should('contain.text', 'None')
        //**will have to review this once DB has been cleared */
        //cy.get('[id="decline-reasons"]').should($el => expect($el.text().trim()).to.equal('Other:\n                    This is the second test\n                    Finance:\n                    This is the second test\n                    Governance:\n                    This is the second test\n                    Performance:\n                    This is the second test\n                    Choice of trust:\n                    This is the second test'))
        cy.decisionDate().should('contain.text', '10 August 2022')
        // clicks on the record a decision button to submit
        cy.recordThisDecision().click()
        // recorded decision confirmation
        cy.recordnoteMsg().should('contain.text', 'Decision recorded')
        cy.visit(projecList)
        cy.projectStateId().should('contain.text', 'DECLINED')
    })
})