/* eslint-env node */

import { defineConfig } from 'Cypress';
import { generateZapReport } from './cypress/plugins/generateZapReport.js';

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
    },
  },
  video: false,
  retries: 0,
  e2e: {
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
