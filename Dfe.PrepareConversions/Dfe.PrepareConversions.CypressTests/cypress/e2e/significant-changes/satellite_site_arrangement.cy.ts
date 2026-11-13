/// <reference types="cypress" />
import satelliteSiteArrangement from '../../pages/significantChangeSatelliteSiteArrangement';

interface Assessment {
    answer?: 'yes' | 'no' | 'notApplicable' | null;
    additionalInformation?: string | null;
    supportingEvidence?: string | null;
}

interface ProjectAssessment {
    satelliteSiteArrangement: Assessment;
    tier: number;
}

describe('Significant change - satellite site arrangement', () => {
    let projectId: string;
    let originalAssessment: Assessment | undefined;
    let originalTier: number;

    const openPage = () => {
        cy.visit(`/significant-change/task-list/${projectId}/satellite-site-arrangement`);
        satelliteSiteArrangement.verifyPageLoaded();
    };

    beforeEach(() => {
        originalAssessment = undefined;
        cy.login();
        cy.acceptCookies();
        cy.visit('/significant-change/project-list');
        cy.getById('school-name-0').click();
        cy.location('pathname').then((path) => {
            projectId = path.split('/').pop()!;
            cy.callAcademisationApi('GET', `/significant-change/${projectId}`).then((response) => {
                expect(response.status).to.eq(200);
                const project = response.body as ProjectAssessment;
                expect(project.satelliteSiteArrangement).to.be.an('object');
                originalAssessment = project.satelliteSiteArrangement;
                originalTier = project.tier;
                cy.callAcademisationApi('PUT', `/significant-change/${projectId}/SatelliteSiteArrangement`, {
                    answer: null,
                    additionalInformation: null,
                    supportingEvidence: null,
                })
                    .its('status')
                    .should('eq', 200);
            });
        });
    });

    afterEach(() => {
        if (originalAssessment) {
            cy.callAcademisationApi(
                'PUT',
                `/significant-change/${projectId}/SatelliteSiteArrangement`,
                originalAssessment
            )
                .its('status')
                .should('eq', 200);
        }
    });

    it('shows the task before satellite considerations and reveals additional information only for No', () => {
        cy.visit(`/significant-change/task-list/${projectId}`);
        cy.getById('task-status-satellite-site-arrangement').should('contain.text', 'Not started');
        cy.contains('h3.app-task-list__section', 'SEN and alternative provision considerations')
            .next('.app-task-list')
            .within(() => {
                cy.get('a').then(($links) => {
                    const titles = [...$links].map((link) => link.textContent?.trim());
                    expect(titles.indexOf('Satellite site arrangement')).to.be.lessThan(
                        titles.indexOf('Special school satellite site considerations')
                    );
                });
                cy.contains('a', 'Satellite site arrangement').click();
            });

        satelliteSiteArrangement
            .verifyPageLoaded()
            .verifyAdditionalInformationVisible(false)
            .selectAnswer('no')
            .verifyAdditionalInformationVisible(true)
            .selectAnswer('yes')
            .verifyAdditionalInformationVisible(false)
            .selectAnswer('not-applicable')
            .verifyAdditionalInformationVisible(false);
    });

    it('validates required fields, persists No with evidence, and sets tier 2', () => {
        openPage();
        satelliteSiteArrangement.save();
        cy.get('.govuk-error-summary').should('contain.text', 'Select an option');
        cy.getById('Answer-error').should('contain.text', 'Select an option');

        satelliteSiteArrangement.selectAnswer('no').save();
        cy.get('.govuk-error-summary').should('contain.text', 'Add additional information');
        cy.getById('AdditionalInformation-error').should('contain.text', 'Add additional information');

        satelliteSiteArrangement
            .enterAdditionalInformation('Further investigation of the proposed expansion is required')
            .enterSupportingEvidence('https://example.test/satellite-arrangement')
            .save();

        cy.location('pathname').should('eq', `/significant-change/task-list/${projectId}`);
        cy.getById('task-status-satellite-site-arrangement').should('contain.text', 'Completed');
        cy.contains('p', `Tier: ${originalTier === 3 ? 3 : 2}`).should('be.visible');

        openPage();
        cy.getById('answer-no').should('be.checked');
        cy.getByDataTest('additional-information').should(
            'have.value',
            'Further investigation of the proposed expansion is required'
        );
        cy.getByDataTest('supporting-evidence').should('have.value', 'https://example.test/satellite-arrangement');
    });

    (['yes', 'not-applicable'] as const).forEach((answer) => {
        it(`saves ${answer} without additional information and clears a previous No explanation`, () => {
            openPage();
            satelliteSiteArrangement.selectAnswer('no').enterAdditionalInformation('Previous explanation').save();
            openPage();
            satelliteSiteArrangement.selectAnswer(answer).verifyAdditionalInformationVisible(false).save();

            cy.location('pathname').should('eq', `/significant-change/task-list/${projectId}`);
            cy.getById('task-status-satellite-site-arrangement').should('contain.text', 'Completed');
            openPage();
            cy.getById(`answer-${answer}`).should('be.checked');
            cy.getByDataTest('additional-information').should('have.value', '');
        });
    });
});
