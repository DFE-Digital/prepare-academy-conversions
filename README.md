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

For local development, user secrets can be set using the command:
- `dotnet user-secrets set "key" "value"` from the `ApplyToBecomeInternal` project.

Alternatively, there is a Rider plugin called `.NET Core User Secrets` that allows the secrets to be managed via a json file, accessed by right clicking on the project -> `Tools` -> `Open Project User Secrets`.

## Cypress testing

### Test execution
The Cypress tests will run against the front-end of the application, so the credentials you provide below should be of the user that is set up to run against the UI.

To execute the tests locally and view the output, run the following:

```
npm run cy:open -- --env username='USERNAME',password='PASSWORD',url="BASE_URL_OF_APP"
```

To execute the tests in headless modet, run the following (the output will log to the console):

```
npm run cy:run -- --env username='USERNAME',password='PASSWORD',url="BASE_URL_OF_APP"
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