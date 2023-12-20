/// <reference types ='Cypress'/>
import BasePage from './BasePage'

export default class projectList extends BasePage {

   static path = 'project-list'

   static checkProjectListPage() {
      cy.url().should('include', this.path)
   }

   static getNthProject(n = 0) {
      this.checkProjectListPage()
      return cy.get(`[id="school-name-${n}"]`)
   }

   static getNthProjectDeliveryOfficer(deliveryOfficer) {
      this.checkProjectListPage()
      return cy.get(`[data-cy="select-projectlist-filter-officer-${deliveryOfficer}"]`)
   }

   static filterProjectList(titleFilter) {
      const filterQuery = `?Title=${encodeURIComponent(titleFilter)}`
      cy.visit(`${Cypress.env('url')}/${this.path}${filterQuery}`)
   }

   static selectFirstItem() {
      this.checkProjectListPage()
      this.getNthProject().click()
   }

   static selectProject(projectName = 'Gloucester school') {
      this.filterProjectList(projectName)
      this.selectFirstItem()
      return cy.url().then(url => this.getIdFromUrl(url))
   }

   static selectVoluntaryProject() {
      cy.login({ titleFilter: 'Voluntary Cypress Project' })
      cy.get('[id="school-name-0"]').click()

      return cy.url().then(url => this.getIdFromUrl(url))
   }

   static getIdFromUrl(url) {
      const urlSplit = url.toString().split('/')
      for (let i = urlSplit.length - 1; i > 0; i--) {
         const potentialId = parseInt(urlSplit[i])

         if (!isNaN(potentialId)) return potentialId
      }

      return ''
   }
}