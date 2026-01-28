import BasePage from './basePage';

class OutgoingTrustAcademiesPage extends BasePage {
    public path = 'transfers/outgoingtrustacademies';

    public selectMultipleAcademies(academies): this {
        academies.array.forEach((academy) => {
            this.selectSingleAcademy(academy);
        });
        return this;
    }

    public selectSingleAcademy(academy): this {
        cy.get('.govuk-checkboxes').contains(academy).click();
        return this;
    }

    public selectOptionYes(): this {
        cy.getByDataTest('true').click();
        return this;
    }

    public selectOptionById(optionId: string): this {
        cy.getById(optionId).click();
        return this;
    }

    public submitForm(): this {
        cy.get('[type="submit"]').click();
        return this;
    }
}

const outgoingTrustAcademiesPage = new OutgoingTrustAcademiesPage();

export default outgoingTrustAcademiesPage;
