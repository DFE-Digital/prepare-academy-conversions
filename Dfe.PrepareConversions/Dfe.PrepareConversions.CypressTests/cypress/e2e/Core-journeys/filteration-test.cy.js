/// <reference types="Cypress" />
import projectList from "../../pages/projectList";


describe('Filteration Tests', { tags: ['@dev', '@stage'] }, () => {

  const currentDate = new Date();
  const nextYearDate = new Date();
  nextYearDate.setFullYear(currentDate.getFullYear() + 1);
  const oneMonthAgoDate = new Date();
  oneMonthAgoDate.setMonth(currentDate.getMonth() - 1);
  const nextMonthDate = new Date();
  nextMonthDate.setMonth(currentDate.getMonth() + 1);

  const testData = {
    projectName: 'Sponsored Cypress Project',
    completedText: 'Completed',
    projectAssignment: {
      deliveryOfficer: 'Richika Dogra',
      assignedOfficerMessage: 'Project is assigned',
    },
    schoolOverview: {
      pan: '98765',
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
      forecastedCapitalNextYear: 12
    },
    pupilForecast: {
      additionalInfomation: 'Pupil Forecast Additional Information'
    },
    rationale: 'This is why this school should become an academy',
    risksAndIssues: 'Here are the risks and issues for this conversion',
    localAuthority: {
      comment: 'Comment',
      sharepointLink: 'https://sharepoint.com'
    },
    performanceInfo: 'Additional Information',
    keyStages: [4, 5]
  }

  before(() => {
    
  })

  beforeEach(() => {
    projectList.selectProject(testData.projectName)
  })

  it('Should filter projects by region', () => {
      // Visit the home page or the initial project list page
      cy.visit(`${Cypress.env('url')}/`); 
    // Check if the Region accordion section is not expanded
    cy.get('[data-cy="select-projectlist-filter-region"]').should('have.attr', 'aria-expanded', 'false').then(($accordion) => {
      const isAccordionExpanded = $accordion.attr('aria-expanded') === 'true';

      // If the accordion is not expanded, click to expand it
      if (!isAccordionExpanded) {
        cy.get('[data-cy="select-projectlist-filter-region"]').click();
      }
      // Select the region checkbox
      cy.get('#filter-project-region-east-midlands').check();
      cy.get('#filter-project-region-east-of-england').check();
      cy.get('#filter-project-region-london').check();
      cy.get('#filter-project-region-north-east').check();

      // Apply the selections
      cy.get('[data-cy="select-projectlist-filter-apply"]').click();

      // Assert that the results are updated based on the selected regions
      for (let i = 0; i < 10; i++) {
        const regionSelector = `#region-${i}`;

        // Ensure that the selected regions are present in the results
        cy.get(regionSelector).each(($regionElement) => {
          const region = $regionElement.text();
          expect(region).to.satisfy((text) => {
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
      cy.get('[data-cy="clear-filter"]').click();
    });
  });

  it('Filter by Project Status', () => {
    // Visit the home page or the initial project list page
    cy.visit(`${Cypress.env('url')}/`); 

    // Check if the Project Status accordion section is not expanded
    cy.get('[data-cy="select-projectlist-filter-project-status"]').should('have.attr', 'aria-expanded', 'false').then(($accordion) => {
      const isAccordionExpanded = $accordion.attr('aria-expanded') === 'true';

      // If the accordion is not expanded, click to expand it
      if (!isAccordionExpanded) {
        cy.get('[data-cy="select-projectlist-filter-project-status"]').click();
      }

      const projectStatuses = ['Approved', 'Approved with conditions', 'Declined', 'Deferred', 'Pre advisory board'];

      // Select each project status checkbox and check the URL
      projectStatuses.forEach((statusToFilter) => {
        // Select the project status checkbox
        cy.get(`[data-cy="select-projectlist-filter-status-${statusToFilter}"]`).check();

        // Perform actions to apply the filter by project status
        cy.get('[data-cy="select-projectlist-filter-apply"]').click();

        // Log the actual URL to help debug the issue
        cy.url().then((url) => {
          cy.log(`Actual URL for ${statusToFilter}: ${url}`);

          // Add assertions to check if the filter applied correctly
          cy.get('[id^="project-status-"]').invoke('text').then((text) => {
            // Convert text to an array if it's not already
            const texts = Array.isArray(text) ? text : [text];

            // Check each text for inclusion (case-insensitive)
            texts.forEach((individualText) => {
              expect(individualText.toLowerCase()).to.include(statusToFilter.toLowerCase());
            });
          });
        });

        // Clear the filter
        cy.get('[data-cy="clear-filter"]').click();

      });

    });
  });

});


