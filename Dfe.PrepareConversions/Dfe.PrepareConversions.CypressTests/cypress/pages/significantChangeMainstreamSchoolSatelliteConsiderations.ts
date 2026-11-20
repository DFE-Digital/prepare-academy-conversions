/// <reference types="cypress" />
import BasePage from './basePage';

class SignificantChangeMainstreamSchoolSatelliteConsiderations extends BasePage {
    public path = 'mainstream-school-satellite-considerations';

    public verifyPageLoaded(): this {
        return this.verifyPageLoadedWithLegend(
            'Mainstream school satellite considerations',
            'Does the proposal meet the requirements set out for special school provision on a satellite site in the operational guidance?'
        );
    }
}

const significantChangeMainstreamSchoolSatelliteConsiderations =
    new SignificantChangeMainstreamSchoolSatelliteConsiderations();

export default significantChangeMainstreamSchoolSatelliteConsiderations;
