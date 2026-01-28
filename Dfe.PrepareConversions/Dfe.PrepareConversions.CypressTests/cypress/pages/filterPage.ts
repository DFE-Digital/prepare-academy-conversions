import BasePage from './basePage';

class FilterPage extends BasePage {
    public path = 'project-list';

    public expandAccordionIfCollapsed(accordionDataCy: string): this {
        cy.getByDataCy(accordionDataCy).then(($accordion) => {
            if ($accordion.attr('aria-expanded') === 'false') {
                cy.wrap($accordion).click();
            }
        });
        return this;
    }

    public expandRegionFilter(): this {
        return this.expandAccordionIfCollapsed('select-projectlist-filter-region');
    }

    public expandProjectStatusFilter(): this {
        return this.expandAccordionIfCollapsed('select-projectlist-filter-project-status');
    }

    public selectRegions(regionIds: string[]): this {
        regionIds.forEach((regionId) => {
            cy.getById(`filter-project-region-${regionId}`).check();
        });
        return this;
    }

    public selectProjectStatus(status: string): this {
        cy.getByDataCy(`select-projectlist-filter-status-${status}`).check();
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

    public verifyRegionInResults(regionIndex: number, expectedRegions: string[]): this {
        cy.getById(`region-${regionIndex}`).each(($regionElement) => {
            const regionText = $regionElement.text();
            const matchesExpectedRegion = expectedRegions.some((region) => regionText.includes(region));
            expect(matchesExpectedRegion, `Region "${regionText}" should match one of: ${expectedRegions.join(', ')}`)
                .to.be.true;
        });
        return this;
    }

    public verifyProjectStatusInResults(expectedStatus: string): this {
        cy.get('[id^="project-status-"]').invoke('text').should('include', expectedStatus);
        return this;
    }

    public verifyAllRegionsInResults(count: number, expectedRegions: string[]): this {
        for (let i = 0; i < count; i++) {
            this.verifyRegionInResults(i, expectedRegions);
        }
        return this;
    }
}

export const filterPage = new FilterPage();
export default filterPage;
