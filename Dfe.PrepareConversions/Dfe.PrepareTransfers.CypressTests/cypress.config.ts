import { defineConfig } from 'cypress'
const { generateZapReport } = require('./cypress/plugins/generateZapReport')
const { verifyDownloadTasks } = require('cy-verify-downloads')

export default defineConfig({
  reporter: 'cypress-multi-reporters',
  reporterOptions: {
    reporterEnabled: 'mochawesome',
    mochawesomeReporterOptions: {
      reportDir: 'cypress/reports/mocha',
      quiet: true,
      overwrite: false,
      html: false,
      json: true,
    }
  },
  video: false,
  retries: 0,
  e2e: {
    // eslint-disable-next-line no-unused-vars
    setupNodeEvents(on, config) {
      // implement node event listeners here

      on('after:run', async () => {
        if (process.env.ZAP) {
          await generateZapReport()
        }
      }),

      on('task', verifyDownloadTasks)
    },
  },
  userAgent: 'PrepareConversions/1.0 Cypress'
})
