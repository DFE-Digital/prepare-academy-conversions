/// <reference types="cypress" />
// ***********************************************************
// This example plugins/index.js can be used to load plugins
//
// You can change the location of this file or turn off loading
// the plugins file with the 'pluginsFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/plugins-guide
// ***********************************************************

// This function is called when a project is opened or re-opened (e.g. due to
// the project's config changing)

// ***********************************************************
/**
 * @type {Cypress.PluginConfig}
 */
// eslint-disable-next-line no-unused-vars
const sqlServer = require('cypress-sql-server');

module.exports = (on, config) => {
  // `on` is used to hook into various events Cypress emits
  // `config` is the resolved Cypress config
  var dbConfig;
  try {
    dbConfig = JSON.parse(process.env.db);
  } catch (error) {
    console.log(
      `The db config is not set. Please set the 'db' environment variable in the following format e.g.
       { "userName": "username", "password": "password", "server": "server", "options": { "database": "database", "rowCollectionOnRequestCompletion" : true }}`);
       throw error;
  }

  tasks = sqlServer.loadDBPlugin(dbConfig);
  on('task', tasks); 
}
// ***********************************************************

//***Cypress Grep module for filtering tests Any new tags should be added to the examples**
/**
 * @example {{tags: '@dev'} : Development
 * @example {tags: '@stage'} : Staging
 * @example {tags: '@integration'} : Integration
 * @example {tags: ['@dev', '@stage']}
 */
module.export = (on, config) => {
  
  require('cypress-grep/src/plugin')(config)
}
// ***********************************************************

module.export = (on, config) => {
  require('eslint/src/plugin')(config)
}