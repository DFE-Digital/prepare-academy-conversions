/// <reference types ='Cypress'/>

import BasePage from "./BasePage"

export default class Performance extends BasePage {
    static selectors = {
        ofstedInfoText: 'This information comes from TRAMS. It is for reference only. It will not appear in the project document.',
        keyStageText: 'This information comes from Find and compare schools in England. It is for reference only. It will not appear in the project document.',
        ofstedInfoTextId: '.govuk-grid-column-two-thirds > .govuk-body', 
        keyStageLinkText: 'Source of data: Find and compare school performance (opens in new tab)',  
        keyStageLink: (urn) => `https://www.compare-school-performance.service.gov.uk/school/${urn}`,
        keyStageTextId: '.govuk-grid-column-two-thirds > :nth-child(3)',
        keyStageLinkTextId: ':nth-child(4) > .govuk-link',
    }
    static keyStagePath = (keyStageNumber) => `key-stage-${keyStageNumber}-performance-tables`

    static verifyKeyStageScreenText(urn){
        cy.get(this.selectors.keyStageTextId).should('contain.text', this.selectors.keyStageText);
        cy.get(this.selectors.keyStageLinkTextId).should('have.text', this.selectors.keyStageLinkText);
        cy.get(this.selectors.keyStageLinkTextId)
            .should("contain.text", this.selectors.keyStageLinkText)
            .should("have.attr", "href")
            .and('include', this.selectors.keyStageLink(urn));
    }

    static verifyOfsteadScreenText(){
        cy.get(this.selectors.ofstedInfoTextId).should('contain.text', this.selectors.ofstedInfoText); 
    }

    static changeKeyStageInfo(keyStageNumber) {
        cy.checkPath(this.keyStagePath(keyStageNumber));
        cy.get(`[data-cy="key-stage-${keyStageNumber}-back-btn"]`).click();
    }
}