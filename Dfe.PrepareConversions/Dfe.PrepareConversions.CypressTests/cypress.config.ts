/* eslint-env node */

import { defineConfig } from 'cypress'

export default defineConfig({
  video: false,
  retries: 2,
  e2e: {
    // We've imported your old cypress plugins here.
    // You may want to clean this up later by importing these.
    setupNodeEvents(on, config) {
      return require('./cypress/plugins/index.js')(on, config)
    },
    baseUrl: 'https://s184d01-acacdnendpoint-ata0dwfremepeff8.z01.azurefd.net/swagger/index.html'
  },
})
