import BasePage from './basePage';

class ProjectAssignmentPage extends BasePage {
    public slug = 'project-assignment';

    public assignDeliveryOfficer(deliveryOfficer: string): this {
        cy.getById('delivery-officer').type(deliveryOfficer);
        cy.get('li').contains(deliveryOfficer).click();
        this.continue();
        return this;
    }

    public unassignDeliveryOfficer(): this {
        cy.getById('unassign-link').click();

        return this;
    }
}
const projectAssignmentPage = new ProjectAssignmentPage();
export default projectAssignmentPage;
