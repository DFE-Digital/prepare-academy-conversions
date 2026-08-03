/// <reference types="cypress" />
import { Logger } from '../../support/logger';
import significantChangeProjectListPage from '../../pages/significantChangeProjectList';

describe('Significant change project list', () => {
    beforeEach(() => {
        cy.login();
        Logger.log('Visit the homepage before each test');
        cy.visit('/');
        cy.acceptCookies();
    });

    it('Should navigate to significant change project list from service navigation', () => {
        cy.get('.govuk-service-navigation').contains('a', 'Significant changes').click();

        cy.checkPath('/significant-change/project-list');
        cy.getByDataCy('select-heading').should('contain.text', 'Significant change projects');
        cy.contains('.govuk-service-navigation__item--active', 'Significant changes').should('be.visible');
    });

    it('Should display project list rows or no-results state', () => {
        cy.visit('/significant-change/project-list');

        cy.getByDataCy('select-projectlist-filter-count')
            .invoke('text')
            .then((countText) => {
                const match = countText.match(/\d+/);
                const count = match ? parseInt(match[0], 10) : 0;

                if (count > 0) {
                    cy.getByDataCy('select-projectlist-row').its('length').should('be.greaterThan', 0);
                    cy.getById('urn-0').should('contain.text', 'URN:');
                    cy.getById('incoming-trust-0').should('contain.text', 'Trust:');
                    cy.getById('tier-0').should('contain.text', 'Tier:');
                    cy.getById('type-and-route-0').should('contain.text', 'Route:');
                    cy.getById('assigned-to-0').should('contain.text', 'Unassigned');
                    cy.get('strong[id^="project-status-"]')
                        .first()
                        .invoke('attr', 'class')
                        .should('match', /govuk-tag--(green|yellow|orange|red|purple)/);
                } else {
                    cy.contains('There are no matching results.').should('be.visible');
                }
            });
    });

    it('Should offer every status and tier, labelled as the API sends them', () => {
        // Statuses and tiers are closed sets served by the API, so this holds regardless of what
        // projects exist — it is the closed-set rule observed from the browser.
        significantChangeProjectListPage.visit().expandStatusFilter();

        significantChangeProjectListPage
            .verifyStatusOption('PreDecision', 'Pre decision')
            .verifyStatusOption('Approved', 'Approved')
            .verifyStatusOption('ApprovedWithConditions', 'Approved with conditions')
            .verifyStatusOption('Deferred', 'Deferred')
            .verifyStatusOption('Declined', 'Declined')
            .verifyStatusOption('Withdrawn', 'Withdrawn');

        significantChangeProjectListPage
            .expandTierFilter()
            .verifyTierOption(1, 'Tier 1')
            .verifyTierOption(2, 'Tier 2')
            .verifyTierOption(3, 'Tier 3');
    });

    it('Should render selected filter tags using display names, not slugs or raw values', () => {
        significantChangeProjectListPage.visit().expandStatusFilter().selectStatus('PreDecision');
        significantChangeProjectListPage.expandTierFilter().selectTier(1).applyFilters();

        significantChangeProjectListPage
            .verifyFilterBannerVisible()
            .verifyTagPresent('Pre decision')
            .verifyTagPresent('Tier 1');

        // Guards the Stub() regression: tags must not render "pre-decision" or "PreDecision".
        cy.get('.moj-filter__tag').should('not.contain.text', 'pre-decision');
        cy.get('.moj-filter__tag').should('not.contain.text', 'PreDecision');
    });

    it('Should filter by tier and clear the filter again', () => {
        significantChangeProjectListPage.visit();

        significantChangeProjectListPage.projectCount().then((unfilteredCount) => {
            if (unfilteredCount === 0) {
                Logger.log('No significant change projects available - skipping filter assertions');
                return;
            }

            significantChangeProjectListPage
                .expandTierFilter()
                .selectTier(1)
                .applyFilters()
                .verifyFilterBannerVisible()
                .verifyTagPresent('Tier 1');

            significantChangeProjectListPage.projectCount().should('be.lte', unfilteredCount);

            significantChangeProjectListPage.clearFilters().verifyNoTagsPresent();
            significantChangeProjectListPage.projectCount().should('eq', unfilteredCount);
        });
    });

    it('Should keep filters applied when paging', () => {
        significantChangeProjectListPage.visit().expandTierFilter().selectTier(1).applyFilters();

        cy.get('body').then(($body) => {
            if ($body.find("[test-id='nextPage']").length === 0) {
                Logger.log('Only one page of filtered results - skipping pagination assertion');
                return;
            }

            cy.get("[test-id='nextPage']").click();

            // Filters survive paging via TempData — _Pagination.cshtml forwards only currentPage.
            significantChangeProjectListPage.verifyTagPresent('Tier 1');
        });
    });
});