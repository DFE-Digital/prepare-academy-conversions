/* eslint-env node */

import { defineConfig } from 'cypress'
import { generateZapReport } from './cypress/plugins/generateZapReport'

export default defineConfig({
  video: false,
  retries: 2,
  e2e: {
    // We've imported your old cypress plugins here.
    // You may want to clean this up later by importing these.
    setupNodeEvents(on, config) {
      
      on('before:run', () => {
        // Map cypress env vars to process env vars for usage outside of Cypress run
        process.env = config.env
      })

      on('after:run', async () => {
        if(process.env.zapReport) {
          await generateZapReport()
        }
      })

      require('./cypress/plugins/index.js')(on, config)
      return config;
    },
    baseUrl: 'http://s184d01-acacdnendpoint-ata0dwfremepeff8.z01.azurefd.net/'
  },
})
