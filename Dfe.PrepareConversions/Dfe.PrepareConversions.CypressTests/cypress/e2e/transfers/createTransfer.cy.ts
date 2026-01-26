import homePage from 'cypress/pages/home';
import newTransferPage from 'cypress/pages/newTransfer';
import outgoingTrustSearchPage from 'cypress/pages/outgoingTrustSearch';
import outgoingTrustSearchResultsPage from 'cypress/pages/outgoingTrustSearchResults';
import outgoingTrustDetailsPage from 'cypress/pages/outgoingTrustDetailsPage';
import outgoingTrustAcademiesPage from 'cypress/pages/outgoingTrustAcademies';
import incomingTrustSearchPage from 'cypress/pages/incomingTrustSearch';
import checkAnswersPage from 'cypress/pages/checkAnswers';
import projectPage from 'cypress/pages/project';
import benefitsPage from 'cypress/pages/benefits';
import datesPage from 'cypress/pages/dates';
import downloadPage from 'cypress/pages/download';
import featuresPage from 'cypress/pages/features';
import legalRequirementsPage from 'cypress/pages/legalRequirements';
import previewPage from 'cypress/pages/preview';
import projectAssignmentPage from 'cypress/pages/projectAssignment';
import rationalePage from 'cypress/pages/rationale';
import trustInformationProjectDatesPage from 'cypress/pages/trustInformationProjectDates';
import dayjs from 'dayjs';
import { Logger } from '../../support/logger';
import { EnvUrl } from '../../constants/cypressConstants';

describe('Create a new transfer', () => {
    let outgoingTrustData, incomingTrustData, projectId;

    const advisoryBoardDate = dayjs().add(3, 'month');
    const transferDate = dayjs().add(4, 'month');

    beforeEach(() => {
        cy.fixture('trustInformation.json').then((jsonData) => {
            outgoingTrustData = jsonData[0];

            outgoingTrustData.academies = outgoingTrustData.academies[0];

            incomingTrustData = jsonData[1];
        });
    });

    context('Create new transfer', () => {
        it.only('Creates a new academy transfer', () => {
            homePage.open().startCreateNewTransfer();

            newTransferPage.clickCreateNewTransfer();

            outgoingTrustSearchPage.searchTrustsByName(outgoingTrustData.name);

            outgoingTrustSearchResultsPage.selectTrust(outgoingTrustData.name);

            outgoingTrustDetailsPage.checkTrustDetails(outgoingTrustData).continue();

            outgoingTrustAcademiesPage.selectSingleAcademy(outgoingTrustData.academies).continue();

            // CURRENTLY FAILING - WILL WORK ON IT SOON
            // Select the option (Is the result of this transfer the formation of a new trust?) with id "false", then click continue
            outgoingTrustAcademiesPage.selectOptionYes();
            outgoingTrustAcademiesPage.submitForm();

            incomingTrustSearchPage.searchTrustsByName(incomingTrustData.name);

            checkAnswersPage.checkDetails(outgoingTrustData, incomingTrustData).continue();

            cy.url().then(($url) => {
                cy.log(`Generated Project URL: ${$url}`);
                cy.wrap($url).should('include', `${projectPage.slug}`);
                projectId = $url.split('/').pop();
                cy.log(`Captured Project ID: ${projectId}`);
            });
        });
    });

    context('Project information', () => {
        it('Has the Project Reference and School Name', () => {
            projectPage.loadProject(projectId).checkProjectId(projectId).checkSchoolName(incomingTrustData.name);
        });
    });

    context('Delivery Officer', () => {
        it('Assign and Unassign Delivery Officer', () => {
            cy.url()
                .then((url) => {
                    projectId = url.match(/task-list\/(\d+)/)![1];
                    Logger.log(`Project ID: ${projectId}`);
                })
                .then(() => {
                    const deliveryOfficer = 'Richika Dogra';

                    projectPage
                        .loadProject(projectId)
                        .checkDeliveryOfficerDetails('Empty')
                        .startChangeDeliveryOfficer();

                    projectAssignmentPage.assignDeliveryOfficer(deliveryOfficer);

                    projectPage.checkDeliveryOfficerAssigned(deliveryOfficer).startChangeDeliveryOfficer();

                    projectAssignmentPage.unassignDeliveryOfficer();

                    projectPage.checkDeliveryOfficerAssigned('Empty');
                });
        });
    });

    context('Completing details', () => {
        it('Fill in Features', () => {
            projectPage.loadProject(projectId).checkFeaturesStatus('Not Started').startTransferFeatures();

            featuresPage.completeReasonForTransfer().completeTypeOfTransfer().markAsComplete().confirmFeatures();

            projectPage.checkFeaturesStatus('Completed');
        });

        it('Fill in Transfer Dates', () => {
            cy.visit(`${Cypress.env(EnvUrl)}/transfers/project/${projectId}`);
            projectPage.checkTransferDatesStatus('Not Started').startTransferDates();

            datesPage
                .completeAdvisoryBoardDate(advisoryBoardDate)
                .completeExpectedTransferDate(transferDate)
                .confirmDates();

            projectPage.checkTransferDatesStatus('IN PROGRESS');
        });

        it('Fill in Benefits and Risks', () => {
            cy.visit(`${Cypress.env(EnvUrl)}/transfers/project/${projectId}`);

            projectPage.loadProject(projectId).checkBenefitsAndRiskStatus('Not Started').startBenefitsAndRisk();

            benefitsPage
                .completeBenefits()
                .completeRisks()
                .completeEqualitiesImpactAssessment()
                .markAsComplete()
                .confirmBenefitsRisks();

            projectPage.checkBenefitsAndRiskStatus('Completed');
        });

        it('Fill in Legal Requirements', () => {
            cy.visit(`${Cypress.env(EnvUrl)}/transfers/project/${projectId}`);

            projectPage.loadProject(projectId).checkLegalRequirementsStatus('Not Started').startLegalRequirements();

            legalRequirementsPage
                .completeResolution()
                .completeAgreement()
                .completeDiocesanConsent()
                .markAsComplete()
                .confirmLegalRequirements();

            projectPage.checkLegalRequirementsStatus('Completed');
        });

        it('Fill in Rationale', () => {
            cy.visit(`${Cypress.env(EnvUrl)}/transfers/project/${projectId}`);

            projectPage.loadProject(projectId).checkRationaleStatus('Not Started').startRationale();

            rationalePage.completeRationale().completeChosenReason().markAsComplete().confirmRationale();

            projectPage.checkRationaleStatus('Completed');
        });

        it('Fill in Trust Information and Project Dates', () => {
            projectPage
                .loadProject(projectId)
                .checkTrustInformationProjectDatesStatus('Not Started')
                .startTrustInformationProjectDates();

            trustInformationProjectDatesPage
                .completeRecommendationAndAuthor()
                .confirmTrustInformationProjectDates()
                .checkOtherTableData(advisoryBoardDate, transferDate)
                .confirmTrustInformationProjectDates();

            projectPage.checkTrustInformationProjectDatesStatus('Completed');
        });
    });

    context('Project Template', () => {
        it.skip('Preview Project Template', () => {
            projectPage.loadProject(projectId).openPreviewProjectTemplate();

            previewPage.checkSections();
        });

        it.skip('Generate Project Template', () => {
            projectPage.loadProject(projectId).generateProjectTemplate();

            downloadPage.downloadProjectTemplate();
        });
    });
});
