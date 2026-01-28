/// <reference types="cypress" />
import projectList from '../../pages/projectList';
import { filterPage } from '../../pages/filterPage';
import { currentDate, nextYearDate } from '../../constants/testConstants';
import { Logger } from '../../support/logger';

describe('Filtration Tests', {}, () => {
    const testData = {
        projectName: 'Deanshanger Primary School',
        completedText: 'Completed',
        projectAssignment: {
            deliveryOfficer: 'Chris Sherlock',
            assignedOfficerMessage: 'Project is assigned',
        },
        schoolOverview: {
            pan: '999',
            pfiDescription: 'PFI Description',
            distance: '15',
            distanceDecription: 'Distance description',
            mp: 'Important Politician, Independent',
        },
        budget: {
            endOfFinanicalYear: currentDate,
            forecastedRevenueCurrentYear: 20,
            forecastedCapitalCurrentYear: 10,
            endOfNextFinancialYear: nextYearDate,
            forecastedRevenueNextYear: 15,
            forecastedCapitalNextYear: 12,
        },
        pupilForecast: {
            additionalInfomation: 'Pupil Forecast Additional Information',
        },
        rationale: 'This is why this school should become an academy',
        risksAndIssues: 'Here are the risks and issues for this conversion',
        localAuthority: {
            comment: 'Comment',
            sharepointLink: 'https://sharepoint.com',
        },
        performanceInfo: 'Additional Information',
        keyStages: [2],
    };

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
        projectList.filterProject(testData.projectName);
    });

    it('Should filter projects by region', () => {
        filterPage
            .expandRegionFilter()
            .selectRegions(regionFilterConfig.regionIds)
            .applyFilters()
            .verifyAllRegionsInResults(regionFilterConfig.resultCount, regionFilterConfig.expectedRegionNames)
            .clearFilters();
    });

    projectStatuses.forEach((statusToFilter) => {
        it(`Should filter by project status: ${statusToFilter}`, () => {
            filterPage
                .expandProjectStatusFilter()
                .selectProjectStatus(statusToFilter)
                .applyFilters()
                .verifyProjectStatusInResults(statusToFilter)
                .clearFilters();
        });
    });

    it('Check accessibility across pages', () => {
        cy.checkAccessibilityAcrossPages();
    });
});
