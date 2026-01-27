/// <reference types="cypress" />
import projectList from '../../pages/projectList';
import { decisionPage } from '../../pages/decisionPage';
import { EnvUrl } from '../../constants/cypressConstants';
import { currentDate, nextYearDate } from '../../constants/testConstants';

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

    before(() => {});

    beforeEach(() => {
        cy.visit(Cypress.env(EnvUrl));
        projectList.filterProject(testData.projectName);
    });

    it('Should filter projects by region', () => {
        // Check if the Region accordion section is not expanded
        cy.getByDataCy('select-projectlist-filter-region')
            .should('have.attr', 'aria-expanded', 'false')
            .then(($accordion) => {
                const isAccordionExpanded = $accordion.attr('aria-expanded') === 'true';

                // If the accordion is not expanded, click to expand it
                if (!isAccordionExpanded) {
                    cy.getByDataCy('select-projectlist-filter-region').click();
                }
                // Select the region checkbox
                cy.getById('filter-project-region-east-midlands').check();
                cy.getById('filter-project-region-east-of-england').check();
                cy.getById('filter-project-region-london').check();
                cy.getById('filter-project-region-north-east').check();

                // Apply the selections
                decisionPage.clickApplyFilters();

                // Assert that the results are updated based on the selected regions
                for (let i = 0; i < 5; i++) {
                    const regionSelector = `region-${i}`;

                    // Ensure that the selected regions are present in the results
                    cy.getById(regionSelector).each(($regionElement) => {
                        const region = $regionElement.text();
                        expect(region).to.satisfy((text: string) => {
                            return (
                                text.includes('East Midlands') ||
                                text.includes('East of England') ||
                                text.includes('London') ||
                                text.includes('North East')
                            );
                        });
                    });
                }

                // Clear the filters
                cy.getByDataCy('clear-filter').click();
            });
    });

    it('Filter by Project Status', () => {
        // Check if the Project Status accordion section is not expanded
        cy.getByDataCy('select-projectlist-filter-project-status')
            .should('have.attr', 'aria-expanded', 'false')
            .then(($accordion) => {
                const isAccordionExpanded = $accordion.attr('aria-expanded') === 'true';

                // If the accordion is not expanded, click to expand it
                if (!isAccordionExpanded) {
                    cy.getByDataCy('select-projectlist-filter-project-status').click();
                }

                const projectStatuses = [
                    'Approved',
                    'Approved with conditions',
                    'Declined',
                    'Deferred',
                    'Pre advisory board',
                ];

                // Select each project status checkbox and check the URL
                projectStatuses.forEach((statusToFilter) => {
                    // Select the project status checkbox
                    cy.getByDataCy(`select-projectlist-filter-status-${statusToFilter}`).check();

                    // Perform actions to apply the filter by project status
                    decisionPage.clickApplyFilters();

                    // Log the actual URL to help debug the issue
                    cy.url().then((url) => {
                        cy.log(`Actual URL for ${statusToFilter}: ${url}`);

                        // Add assertions to check if the filter applied correctly
                        cy.get('[id^="project-status-"]')
                            .invoke('text')
                            .then((text) => {
                                // Convert text to an array if it's not already
                                const texts = Array.isArray(text) ? text : [text];

                                // Check each text for inclusion (case-insensitive)
                                texts.forEach((individualText) => {
                                    expect(individualText.toLowerCase()).to.include(statusToFilter.toLowerCase());
                                });
                            });
                    });

                    // Clear the filter
                    cy.getByDataCy('clear-filter').click();
                });
            });
    });
});
