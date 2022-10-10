# Apply to Become an academy, internal service
Internal service for managing applications for schools to become academies.

## Requirements
- .NET Core 3.1
- NodeJS (for frontend build tools)

## Development Setup

- Run `npm install; npm run build` from the `ApplyToBecomeInternal/wwwroot` directory to build the styles.
- Run `dotnet restore` from the `ApplyToBecomeInternal` project to restore dependencies.
- Run `dotnet run` from the `ApplyToBecomeInternal` project to run the application.

### User-secrets
The following user secrets are required to be able to connect to the TRAMS API (also known as the Academies API):

- `TramsApi:ApiKey` - The API key for the TRAMS API.
- `TramsApi:Endpoint` - The base URL for the TRAMS API.
- `AcademisationApi:ApiKey` - The API key for the Academisation API.
- `AcademisationApi:BaseUrl` - The base URL for the Academisation API.

The following user secret is required for the landing page to be able to navigate to the Transfers service:

- `ServiceLink:TransfersUrl` - The URL for the Transfers service.

For local development, user secrets can be set using the command:
- `dotnet user-secrets set "key" "value"` from the `ApplyToBecomeInternal` project.

Alternatively, there is a Rider plugin called `.NET Core User Secrets` that allows the secrets to be managed via a json file, accessed by right clicking on the project -> `Tools` -> `Open Project User Secrets`.

## Cypress testing

> Note: We've introduced a new [Cypress Selector process](Cypress_Selectors.md).

Install cypress and dependencies:
- Run 'npm install' from the ApplyToBecomeCypressTests directory

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

To execute tests with grep tags on dev:

```
$ npm run cy:run -- --env grepTags=@dev,grepTags=@stage,url="BASE_URL_OF_APP",authorizationHeader="<SECRET HERE>"
```

To execute tests with grep tags on stage:

```
$ npm run cy:run -- --env grepTags=@stage,url="BASE_URL_OF_APP",authorizationHeader="<SECRET HERE>"
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

To run tests with multiple tags in a list:

```
i.e., greTags=@dev+@stage 
```

To run tests including multiple tags independently targeting individual tags:

```
i.e., grepTags=@dev,grepTags=@stage
```

Further details on using cypress-grep test tagging: https://github.com/cypress-io/cypress-grep 
cypress 10.9.0 Latest changes: https://docs.cypress.io/guides/references/changelog 