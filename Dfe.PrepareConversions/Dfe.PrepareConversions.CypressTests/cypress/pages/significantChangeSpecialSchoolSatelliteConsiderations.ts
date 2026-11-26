/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeSpecialSchoolSatelliteConsiderations extends BasePage {
    public path = 'special-school-satellite-considerations';

    public verifyPageLoaded(): this {
        return this.verifyPageLoadedWithLegend(
            'Special school satellite considerations',
            'Does the proposal meet the requirements set out for satellite sites in the operational guidance?'
        );
    }
}

const significantChangeSpecialSchoolSatelliteConsiderations =
    new SignificantChangeSpecialSchoolSatelliteConsiderations();

export default significantChangeSpecialSchoolSatelliteConsiderations;
