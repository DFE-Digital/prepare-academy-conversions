/* eslint-env node */

import { defineConfig } from 'cypress'
import { generateZapReport } from './cypress/plugins/generateZapReport'

export default defineConfig({
  reporter: 'cypress-multi-reporters',
  reporterOptions: {
    reporterEnabled: 'mochawesome',
    mochawesomeReporterOptions: {
      reportDir: 'cypress/reports/mocha',
      quite: true,
      overwrite: false,
      html: false,
      json: true,
    }
  },
  video: false,
  retries: 0,
  e2e: {
    // We've imported your old cypress plugins here.
    // You may want to clean this up later by importing these.
    setupNodeEvents(on, config) {

      on('after:run', async () => {
        if (process.env.ZAP) {
          await generateZapReport()
        }
      })

      on('task', {
        log(message) {
          console.log(message);
          return null;
        },
      });

      require('./cypress/plugins/index.js')(on, config)
      return config;
    }
  },
  userAgent: 'PrepareConversions/1.0 Cypress'
})
