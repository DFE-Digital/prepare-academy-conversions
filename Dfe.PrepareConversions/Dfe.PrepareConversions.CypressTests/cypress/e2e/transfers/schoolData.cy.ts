import homePage from 'cypress/pages/home';
import projectPage from 'cypress/pages/project';
import schoolDataPage from 'cypress/pages/schoolData';
import { Logger } from '../../support/logger';

describe('School Data', () => {
    const trustName = 'The Bishop Fraser Trust';

    beforeEach(() => {
        Logger.log('Visit the transfers homepage before each test');
        cy.visit('/transfers/home');
        homePage.filterProjects(trustName).selectFirstProject();

        projectPage.viewSchoolData();
    });

    it('Shows General Information', () => {
        schoolDataPage.checkGeneralInformation().confirm();
    });

    it('Shows Pupil Numbers', () => {
        schoolDataPage.checkPupilNumbers().confirm();
    });

    it('Shows Latest Ofsted Report', () => {
        schoolDataPage.checkOfstedReport();
    });

    it('Shows KS4 Performance Tables', () => {
        schoolDataPage.checkKS4Tables();
    });

    it('Shows KS5 Performance Tables', () => {
        schoolDataPage.checkKS5Tables();
    });
});
