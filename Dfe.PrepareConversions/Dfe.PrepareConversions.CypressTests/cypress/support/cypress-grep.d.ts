/// <reference types="cypress" />

/** Mirrors @cypress/grep global augmentation (avoids TS path issues with package "exports"). */
declare global {
    namespace Cypress {
        interface SuiteConfigOverrides {
            tags?: string | string[];
        }
        interface TestConfigOverrides {
            tags?: string | string[];
        }
    }
}

export {};
