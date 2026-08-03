import BasePage from './basePage';

class SignificantChangeProjectListPage extends BasePage {
    public path = 'significant-change/project-list';

    /**
     * Checkbox ids are slugged server-side by Stub(): lowercased, non-alphanumerics to '-'.
     * "PreDecision" -> "predecision", "Change of age range" -> "change-of-age-range".
     * data-cy attributes keep the raw value, so prefer those where the value has no spaces.
     */
    private static slug(value: string): string {
        return value.toLowerCase().replace(/[^a-z0-9-_]/g, '-');
    }

    public visit(): this {
        cy.visit('/significant-change/project-list');
        return this;
    }

    public expandAccordionIfCollapsed(accordionDataCy: string): this {
        cy.getByDataCy(accordionDataCy).then(($accordion) => {
            if ($accordion.attr('aria-expanded') === 'false') {
                cy.wrap($accordion).click();
            }
        });
        return this;
    }

    public expandAssigneeFilter(): this {
        return this.expandAccordionIfCollapsed('select-projectlist-filter-assignee');
    }

    public expandStatusFilter(): this {
        return this.expandAccordionIfCollapsed('select-projectlist-filter-status');
    }

    public expandTierFilter(): this {
        return this.expandAccordionIfCollapsed('select-projectlist-filter-tier');
    }

    public expandRouteFilter(): this {
        return this.expandAccordionIfCollapsed('select-projectlist-filter-route');
    }

    public enterKeyword(keyword: string): this {
        cy.getByDataCy('select-projectlist-filter-keyword').clear().type(keyword);
        return this;
    }

    public selectStatus(statusValue: string): this {
        cy.getByDataCy(`select-projectlist-filter-status-${statusValue}`).check();
        return this;
    }

    public selectTier(tier: number): this {
        cy.getByDataCy(`select-projectlist-filter-tier-${tier}`).check();
        return this;
    }

    public applyFilters(): this {
        cy.getByDataCy('select-projectlist-filter-apply').first().click();
        return this;
    }

    public clearFilters(): this {
        cy.getByDataCy('clear-filter').click();
        return this;
    }

    /** The label comes from the API's Display field, the checkbox value from its Value field. */
    public verifyStatusOption(statusValue: string, expectedLabel: string): this {
        const id = SignificantChangeProjectListPage.slug(statusValue);
        cy.getById(id ? `filter-status-${id}` : '').should('have.value', statusValue);
        cy.get(`label[for="filter-status-${id}"]`).should('contain.text', expectedLabel);
        return this;
    }

    public verifyTierOption(tier: number, expectedLabel: string): this {
        cy.get(`label[for="filter-tier-${tier}"]`).should('contain.text', expectedLabel);
        return this;
    }

    public verifyFilterBannerVisible(): this {
        cy.getByDataCy('select-projectlist-filter-banner').should('be.visible');
        return this;
    }

    public verifyTagPresent(expectedText: string): this {
        cy.get('.moj-filter__tag').should('contain.text', expectedText);
        return this;
    }

    public verifyNoTagsPresent(): this {
        cy.get('.moj-filter__tag').should('not.exist');
        return this;
    }

    public projectCount(): Cypress.Chainable<number> {
        return cy
            .getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                return match ? parseInt(match[0], 10) : 0;
            });
    }
}

export const significantChangeProjectListPage = new SignificantChangeProjectListPage();
export default significantChangeProjectListPage;