/// <reference types="cypress" />
import { filterPage } from '../../pages/filterPage';
import { Logger } from '../../support/logger';

describe('Filtration Tests', {}, () => {
    const regionFilterConfig = {
        regionIds: ['east-midlands', 'east-of-england', 'london', 'north-east'],
        expectedRegionNames: ['East Midlands', 'East of England', 'London', 'North East'],
        resultCount: 5,
    };

    const projectStatuses = ['Approved', 'Approved with conditions', 'Declined', 'Deferred', 'Pre advisory board'];

    before(() => {});

    beforeEach(() => {
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    afterEach(() => {
        filterPage.clearFilters();
    });

    it('Should filter projects by region', () => {
        filterPage
            .expandRegionFilter()
            .selectRegions(regionFilterConfig.regionIds)
            .applyFilters()
            .verifyAllRegionsInResults(regionFilterConfig.resultCount, regionFilterConfig.expectedRegionNames);
    });

    projectStatuses.forEach((statusToFilter) => {
        it(`Should filter by project status: ${statusToFilter}`, () => {
            filterPage
                .expandProjectStatusFilter()
                .selectProjectStatus(statusToFilter)
                .applyFilters()
                .verifyProjectStatusInResults(statusToFilter);
        });
    });

    it('Check accessibility across pages', () => {
        cy.checkAccessibilityAcrossPages();
    });
});
