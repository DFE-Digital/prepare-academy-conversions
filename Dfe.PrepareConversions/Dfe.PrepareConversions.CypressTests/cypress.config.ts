/* eslint-env node */

import { defineConfig } from 'cypress';
import { generateZapReport } from './cypress/plugins/generateZapReport';
import pluginConfig from './cypress/plugins/index';

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
    specPattern: 'cypress/e2e',
    supportFile: 'cypress/support/e2e.ts',
    setupNodeEvents(on, config) {
      on('after:run', async () => {
        if (process.env.ZAP) {
          await generateZapReport();
        }
      });

      on('task', {
        log(message: string) {
          console.log(message);
          return null;
        },
      });

      pluginConfig(on, config);
      return config;
    },
  },
  userAgent: 'PrepareConversions/1.0 Cypress',
});
