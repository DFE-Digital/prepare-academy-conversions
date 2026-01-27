// Scans each page for accessibility violations
// Note: there are some pages that cannot be scanned in this way as they require
// session data setting so must be done manually -
// transfers/outgoingtrustacademies and transfers/checkyouranswers

describe('Check accessibility of the different pages', () => {
    it('Validate conversions accessibility links', () => {
        cy.fixture('conversionLinks.json').then((conversionLinks) => {
            conversionLinks.forEach((link: string) => {
                cy.visit(`/${link}`);
                cy.executeAccessibilityTests();
            });
        });
    });

    it('Validate transfers accessibility links', () => {
        cy.fixture('transfersLinks.json').then((transfersLinks) => {
            transfersLinks.forEach((link: string) => {
                cy.visit(`/transfers/${link}`);
                cy.executeAccessibilityTests();
            });
        });
    });

    it('Validate general accessibility links', () => {
        cy.fixture('otherLinks.json').then((otherLinks) => {
            otherLinks.forEach((link: string) => {
                cy.visit(`/${link}`);
                cy.executeAccessibilityTests();
            });
        });
    });
});
