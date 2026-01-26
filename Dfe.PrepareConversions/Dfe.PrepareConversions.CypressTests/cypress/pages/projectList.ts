/// <reference types="cypress" />
import BasePage from './basePage';
import { EnvUrl } from '../constants/cypressConstants';

export default class projectList extends BasePage {
  static path = 'project-list';

  static checkProjectListPage(): void {
    cy.url().should('include', this.path);
  }

  static getNthProject(n = 1): Cypress.Chainable<JQuery<HTMLElement>> {
    this.checkProjectListPage();
    return cy.get(`[id="school-name-${n}"]`);
  }

  static getNthProjectDeliveryOfficer(
    n = 1
  ): Cypress.Chainable<JQuery<HTMLElement>> {
    this.checkProjectListPage();
    return cy.get(`[id="assigned-to-${n}"]`);
  }

  static filterProjectList(titleFilter: string): void {
    const filterQuery = `?Title=${encodeURIComponent(titleFilter)}`;
    cy.visit(`${Cypress.env(EnvUrl)}/${this.path}${filterQuery}`);
  }

  static filterByRegion(region: string): void {
    this.filterProjectList(region);
  }

  static filterByStatus(status: string): void {
    this.filterProjectList(status);
  }

  static filterByAdvisoryBoardDate(advisoryBoardDate: string): void {
    this.filterProjectList(advisoryBoardDate);
  }

  static filterByTitle(title: string): void {
    this.filterProjectList(title);
  }

  static selectFirstItem(): void {
    this.checkProjectListPage();
    this.getNthProject().click();
  }

  static selectProject(
    projectName = 'Gloucester school'
  ): Cypress.Chainable<number | string> {
    this.filterProjectList(projectName);
    this.selectFirstItem();
    return cy.url().then((url) => this.getIdFromUrl(url));
  }

  static filterProject(projectName = 'Gloucester school'): typeof projectList {
    this.filterProjectList(projectName);
    return this;
  }

  static selectVoluntaryProject(): Cypress.Chainable<number | string> {
    cy.login({ titleFilter: 'Voluntary Cypress Project' });
    cy.get('[id="school-name-0"]').click();

    return cy.url().then((url) => this.getIdFromUrl(url));
  }

  static getIdFromUrl(url: string): number | string {
    const urlSplit = url.toString().split('/');
    for (let i = urlSplit.length - 1; i > 0; i--) {
      const potentialId = parseInt(urlSplit[i]);

      if (!isNaN(potentialId)) return potentialId;
    }

    return '';
  }
}
