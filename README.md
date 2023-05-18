# Prepare academy conversions service
Internal service for managing applications for schools to become academies.

## Requirements
- .NET 6.0
- NodeJS (for frontend build tools)

## Development Setup

- Run `npm install; npm run build` from the `Dfe.PrepareConversions/wwwroot` directory to build the styles.
- Run `dotnet restore` from the `Dfe.PrepareConversions` project to restore dependencies.
- Run `dotnet run` from the `Dfe.PrepareConversions` project to run the application.

### User-secrets
The following user secrets are required to be able to connect to the TRAMS API (also known as the Academies API):

- `TramsApi:ApiKey` - The API key for the TRAMS API.
- `TramsApi:Endpoint` - The base URL for the TRAMS API.
- `AcademisationApi:ApiKey` - The API key for the Academisation API.
- `AcademisationApi:BaseUrl` - The base URL for the Academisation API.

The following user secret is required for the landing page to be able to navigate to the Transfers service:

- `ServiceLink:TransfersUrl` - The URL for the Transfers service.

For local development, user secrets can be set using the command:
- `dotnet user-secrets set "key" "value"` from the `Dfe.PrepareConversions` project.

Alternatively, there is a Rider plugin called `.NET Core User Secrets` that allows the secrets to be managed via a json file, accessed by right clicking on the project -> `Tools` -> `Open Project User Secrets`.

## Cypress testing

Install cypress and dependencies:
- Run 'npm install' from the Dfe.PrepareConversions.CypressTests directory

### Test execution
You will need to set a secret in `secrets.json` in the following format to run the Cypress command against (you can use any value):

```json
{
  "CypressTestSecret": "<SECRET HERE>"
}
```

To execute the tests locally and view the output:

First set the database config as an environment variable -
For bash -
```
export db='{"server":"localhost", "userName":"sa", "password":"StrongPassword905", "options": { "database": "sip" } }'
```
For windows -
```
set db='{"server":"localhost", "userName":"sa", "password":"StrongPassword905", "options": { "database": "sip" } }'
```

The secret in the below command should match what was set in the `secrets.json` file.
```
npm run cy:open -- --env url="BASE_URL_OF_APP",authorizationHeader="<SECRET HERE>"
```

To execute the tests in headless mode, run the following (the output will log to the console):

```
npm run cy:run -- --env url="BASE_URL_OF_APP",authorizationHeader="<SECRET HERE>"
```

### Loading users from Azure Active Directory
You will need to set a secret in `secrets.json` in the following format to run the Cypress command against (you can use any value):

```
  "AzureAd": {
    "ClientId": "<clientid>",
    "ClientSecret": "<clientsecret>",
    "TenantId": "<tenantid>",
    "GroupId": "<activedirectory-groupid>"
  }
```

### Useful tips

#### Maintaining sessions
Each 'it' block usually runs the test with a clear cache. For our purposes, we may need to maintain the user session to test various scenarios. This can be achieved by adding the following code to your tests:

```
afterEach(() => {
		cy.storeSessionData();
	});
```

##### Writing global commands
The cypress.json file in the `support` folder contains functions which can be used globally throughout your tests. Below is an example of a custom login command

```
Cypress.Commands.add("login",()=> {
	cy.visit(Cypress.env('url')+"/login");
	cy.get("#username").type(Cypress.env('username'));
	cy.get("#password").type(Cypress.env('password')+"{enter}");
	cy.saveLocalStorage();
})

```

Which you can access in your tests like so:

```
before(function () {
	cy.login();
});
```

Further details about Cypress can be found here: https://docs.cypress.io/api/table-of-contents


Further details on using cypress-grep test tagging: https://github.com/cypress-io/cypress-grep
cypress 10.9.0 Latest changes: https://docs.cypress.io/guides/references/changelog

Accessibility
```
i.e.,
Basic usage

it('Has no detectable a11y violations on load', () => {
  // Test the page at initial load
  cy.checkA11y()
})
//By default, it will scan the whole page but it also can be configured to run against a specific element, or to exclude some elements.
```
```
i.e.,
Applying a context and run parameters

it('Has no detectable a11y violations on load (with custom parameters)', () => {
  // Test the page at initial load (with context and options)
  cy.checkA11y('.example-class', {
    runOnly: {
      type: 'tag',
      values: ['wcag2a']
    }
  })
})
```
For more receipes: https://www.npmjs.com/package/cypress-axe


##### Upgrading Cypress version
EVERY 6 to 8 weeks, there is a significant update that will be rolled out with some changes.
To see the cypress change longs navigate to this link: https://docs.cypress.io/guides/references/changelog#
Here you can view the bug fixes, performance fixes and features etc. Latest version you will find at the top of the list with release date. You can jump to the specific version by clicking on the links on the right side under section on this page.

## Update Cypress using NPM
-Close the cypress runner properly by clicking on Stop button then x button.

-Type below command. Here replace 12.2.0 with latest version
 `npm install --save-dev cypress@12.2.0`

-Check Cypress version is updated using below command
 `$ npx cypress --version`

 ##Update Cypress using package.json
 -Cose the cypress runner properly by clicking on Stop button then x button.

 -Navigate to your package.json.

 -Change the cypress version to the current updated version in package.json
  `"cypress": "12.2.0"`

 -Type below command
  `$ npx install cypress`

 -Check cypress version
  `$ npx cypress --version`


##### Cypress Linting
 ESLint is a tool for identifying and reporting on patterns found in ECMAScript/JavaScript code, with the goal of making code more consistent and avoiding bugs.

 Note: If you installed ESLint globally then you must also install eslint-plugin-cypress globally.

 -Installation using npm
  `npm install eslint-plugin-cypress --save-dev`

 -Installation using yarn
  `yarn add eslint-plugin-cypress --dev`

 -Usage: Add an .eslintrc.json file to your cypress directory with the following:
```
   {
      "plugins": [
      "cypress"
     ]
    }
```
-Add rules, example:
```
  {
    "rules": {
      "cypress/no-assigning-return-values": "error",
      "cypress/no-unnecessary-waiting": "error",
      "cypress/assertion-before-screenshot": "warn",
      "cypress/no-force": "warn",
      "cypress/no-async-tests": "error",
      "cypress/no-pause": "error"
    }
  }
```
 -Use the recommended configuration and you can forego configuring plugins, rules, and env individually.
```
  {
    "extends": [
      "plugin:cypress/recommended"
    ]
  }
```
### Security testing with ZAP

The Cypress tests can also be run, proxied via OWASP ZAP for passive security scanning of the application.

These can be run using the configured docker-compose.yml, which will spin up containers for the ZAP daemon and the Cypress tests, including all networking required. You will need to update any config in the file before running

Create a .env file for docker, this file needs to include

* all of your required cypress configuration
* HTTP_PROXY e.g. http://zap:8080
* ZAP_API_KEY, can be any random guid

Example env:
```
URL=<Enter URL>
API_KEY=<Enter API key>
HTTP_PROXY=http://zap:8080
ZAP_API_KEY=<Enter random guid>
```
_Note: You might have trouble running this locally because of docker thinking localhost is the container and not your machine_

To run docker compose use:

`docker-compose -f docker-compose.yml --exit-code-from cypress`

_Note: `--exit-code-from cypress` tells the container to quit when cypress finishes_

You can also exclude URLs from being intercepted by using the NO_PROXY setting

e.g. `NO_PROXY=*.google.com,yahoo.co.uk`

Alternatively, you can run the Cypress tests against an existing ZAP proxy by setting the environment configuration
```
HTTP_PROXY="<zap-daemon-url>"
NO_PROXY="<list-of-urls-to-ignore>"
```
and setting the runtime variables

`zapReport=true,zapApiKey=<zap-api-key>,zapUrl="<zap-daemon-url>"`