/* eslint-env node */

const { defineConfig } = require('Cypress');
import { generateZapReport } from './cypress/plugins/generateZapReport.js';

export default defineConfig({
  defaultCommandTimeout: 10000,
  reporter: 'cypress-multi-reporters',
  reporterOptions: {
    reporterEnabled: 'mochawesome',
    mochawesomeReporterOptions: {
      reportDir: 'cypress/reports/mocha',
      quiet: true,
      overwrite: false,
      html: false,
      json: true,
    },
  },
  video: false,
  retries: 0,
  e2e: {
    specPattern: 'cypress/e2e',
    supportFile: 'cypress/support/e2e.js',
    setupNodeEvents(on, config) {
      on('after:run', async () => {
        if (process.env.ZAP) {
          await generateZapReport();
        }
      });

      on('task', {
        log(message) {
          console.log(message);
          return null;
        },
      });

      require('./cypress/plugins/index.js')(on, config);
      return config;
    },
  },
  userAgent: 'PrepareConversions/1.0 Cypress',
});
