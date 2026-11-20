/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeTrustExperienceAndGovernance extends BasePage {
    public path = 'trust-experience-and-governance';

    public verifyPageLoaded(): this {
        return this.verifyPageLoadedWithLegend(
            'Trust experience and governance',
            'Does the trust currently have the experience and governance arrangements in place to support the proposed change?'
        );
    }
}

const significantChangeTrustExperienceAndGovernance = new SignificantChangeTrustExperienceAndGovernance();

export default significantChangeTrustExperienceAndGovernance;
