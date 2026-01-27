class ProjectPage {
    public slug = 'project';

    public loadProject(projectId): this {
        cy.visit(`/transfers/project/${projectId}`);
        return this;
    }

    public checkProjectId(id): this {
        cy.get('.govuk-caption-l').should('have.text', `Project reference: SAT-${id}`);

        return this;
    }

    public checkSchoolName(schoolName): this {
        cy.get('h1').should('contain.text', schoolName);

        return this;
    }

    public checkDeliveryOfficerAssigned(deliveryOfficer): this {
        this.checkDeliveryOfficerDetails(deliveryOfficer);

        return this;
    }

    public checkDeliveryOfficerDetails(deliveryOfficer): this {
        cy.getByDataId('assigned-user').should('have.text', deliveryOfficer);

        if (deliveryOfficer === 'Empty') {
            cy.getByDataId('assigned-user').should('have.class', 'empty');
        } else {
            cy.getByDataId('assigned-user').should('not.have.class', 'empty');
        }

        return this;
    }

    public startChangeDeliveryOfficer(): this {
        cy.containsText('Change').click();

        return this;
    }

    public checkFeaturesStatus(status): this {
        cy.getByDataTest('features').should('have.text', status.toUpperCase());

        return this;
    }

    public startTransferFeatures(): this {
        cy.getByDataTest('transfer-features').click();

        return this;
    }

    public checkTransferDatesStatus(status): this {
        cy.getByDataTest('dates').should('have.text', status.toUpperCase());

        return this;
    }

    public startTransferDates(): this {
        cy.getByDataTest('transfer-dates').click();

        return this;
    }

    public checkBenefitsAndRiskStatus(status): this {
        cy.getByDataTest('benefits').should('have.text', status.toUpperCase());

        return this;
    }

    public startBenefitsAndRisk(): this {
        cy.getByDataTest('transfer-benefits').click();

        return this;
    }

    public checkLegalRequirementsStatus(status): this {
        cy.getByDataTest('legal-requirements').should('have.text', status.toUpperCase());

        return this;
    }

    public startLegalRequirements(): this {
        cy.getByDataTest('transfer-legal-requirements').click();

        return this;
    }

    public checkRationaleStatus(status): this {
        cy.getByDataTest('rationale').should('have.text', status.toUpperCase());

        return this;
    }

    public startRationale(): this {
        cy.getByDataTest('transfer-rationale').click();

        return this;
    }

    public checkTrustInformationProjectDatesStatus(status): this {
        cy.getByDataTest('academyandtrustinformation').should('have.text', status.toUpperCase());

        return this;
    }

    public startTrustInformationProjectDates(): this {
        cy.getByDataTest('academy-trust-information').click();

        return this;
    }

    public openPreviewProjectTemplate(): this {
        cy.getByDataTest('preview-htb').click();

        return this;
    }

    public generateProjectTemplate(): this {
        cy.getByDataTest('generate-htb').click();

        return this;
    }

    public viewSchoolData(): this {
        cy.getByDataTest('sd-academy-1').click();

        return this;
    }
}

const projectPage = new ProjectPage();

export default projectPage;
