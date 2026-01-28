/// <reference types="cypress" />
import BasePage from './basePage';

class Performance extends BasePage {
    private selectors = {
        ofstedInfoText:
            'This information comes from Ofsted. It is for reference only. It will not appear in the project document.',
        keyStageText:
            'This information comes from Find and compare schools in England. It is for reference only. It will not appear in the project document.',
        ofstedInfoTextId: '.govuk-grid-column-two-thirds > .govuk-body',
        keyStageLinkText: 'Source of data: Find and compare school performance (opens in new tab)',
        keyStageLink: (urn: string) => `https://www.compare-school-performance.service.gov.uk/school/${urn}`,
        keyStageTextId: '.govuk-grid-column-two-thirds > :nth-child(3)',
        keyStageLinkTextId: ':nth-child(4) > .govuk-link',
    };

    public keyStagePath(keyStageNumber: number): string {
        return `key-stage-${keyStageNumber}-performance-tables`;
    }

    public verifyKeyStageScreenText(urn: string): this {
        cy.get(this.selectors.keyStageTextId).should('contain.text', this.selectors.keyStageText);
        cy.get(this.selectors.keyStageLinkTextId).should('have.text', this.selectors.keyStageLinkText);
        cy.get(this.selectors.keyStageLinkTextId)
            .should('contain.text', this.selectors.keyStageLinkText)
            .should('have.attr', 'href')
            .and('include', this.selectors.keyStageLink(urn));
        return this;
    }

    public verifyOfsteadScreenText(): this {
        cy.get(this.selectors.ofstedInfoTextId).should('contain.text', this.selectors.ofstedInfoText);
        return this;
    }

    public changeKeyStageInfo(keyStageNumber: number): this {
        cy.checkPath(this.keyStagePath(keyStageNumber));
        cy.getByDataCy(`key-stage-${keyStageNumber}-back-btn`).click();
        return this;
    }
}

const performance = new Performance();

export default performance;
