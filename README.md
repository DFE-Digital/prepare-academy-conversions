# Prepare academy conversions service
Internal service for managing applications for schools to become academies.

## Requirements
- .NET 8.0
- NodeJS 21 (for frontend build tools)

## .NET development

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

For local .NET development, user secrets can be set using the command:
- `dotnet user-secrets set "key" "value"` from the `Dfe.PrepareConversions` project.

Alternatively, there is a Rider plugin called `.NET Core User Secrets` that allows the secrets to be managed via a json file, accessed by right clicking on the project -> `Tools` -> `Open Project User Secrets`.

## Docker development

The development Docker Compose setup starts the application and a Redis container. The application is available over HTTPS at `https://localhost:5003`.

The following are required:

- Docker Desktop with Docker Compose.
- A GitHub personal access token with `read:packages` permission. This is used to restore private NuGet packages during the image build.
- A local HTTPS development certificate exported as `.https/Dfe.PrepareConversions.pfx`.

Copy `.env.development.example` to `.env.development` and provide the required API, Entra ID, certificate, and other local development values. Set `Kestrel__Certificates__Default__Password` to the password used when exporting the certificate. This is a local password and can be any strong value; a UUID string of your choosing is suitable.

Generate the certificate from the repository root before starting the container:

```bash
mkdir -p .https
dotnet dev-certs https -ep ./.https/Dfe.PrepareConversions.pfx -p "<locally chosen UUID password>" --trust
```

Set `GITHUB_TOKEN` in the shell used to run Docker Compose, then build and start the development containers:

```powershell
$env:GITHUB_TOKEN = "<github personal access token>"
docker compose -f docker-compose.development.yml up -d --build
```

The Compose file exposes the application on port `5003` and Redis on port `6379`. View container logs with:

```powershell
docker compose -f docker-compose.development.yml logs -f app
```

Stop the development containers with:

```powershell
docker compose -f docker-compose.development.yml down
```

The `.env.development` file, `.https` directory, and certificate are local Docker development files and must not be committed.

## Cypress Testing

Tests are located in `Dfe.PrepareConversions/Dfe.PrepareConversions.CypressTests`.

### Setup

```bash
cd Dfe.PrepareConversions/Dfe.PrepareConversions.CypressTests
npm install
```

Create `cypress.env.json` in the CypressTests directory:

```json
{
   "url": "BASE_URL_OF_APP",
   "cypressTestSecret": "<SECRET>",
   "academisationApiUrl": "<SECRET>",
   "academisationApiKey": "<SECRET>"
}
```

### Running Tests

| Command | Description |
|---------|-------------|
| `npm run cy:open` | Open Cypress Test Runner (Edge) |
| `npm run cy:run` | Run all tests headless (Edge) |
| `npm run cy:run:conversions` | Run only conversion tests |
| `npm run cy:run:transfers` | Run only transfer tests |

Use `@cypress/grep` to filter tests by tags or title patterns.

### Project Structure

```
cypress/
├── e2e/
│   ├── conversions/    # Conversion journey tests
│   └── transfers/      # Transfer journey tests
├── pages/              # Page Object classes (extend BasePage)
├── fixtures/           # Test data JSON files
├── constants/          # Shared constants
├── support/            # Custom commands and setup
└── plugins/            # Cypress plugins (ZAP integration)
```

### Custom Commands

Common commands are defined in `cypress/support/commands.ts`:
- `cy.login()` - Authenticate and visit project list
- `cy.callAcademisationApi()` - Make authenticated API requests
- `cy.executeAccessibilityTests()` - Run axe-core accessibility checks
- `cy.checkAccessibilityAcrossPages()` - Run a11y checks on all visited URLs
- Data attribute selectors: `cy.getByDataTest()`, `cy.getByDataCy()`, `cy.getById()`

### Accessibility Testing

Tests use `cypress-axe` for accessibility validation. The framework tracks visited URLs and can run accessibility checks across all pages in a test journey.

### Linting & Formatting

| Command | Description |
|---------|-------------|
| `npm run lint` | Check for ESLint issues |
| `npm run lint:fix` | Auto-fix ESLint issues |
| `npm run format` | Format files with Prettier |
| `npm run format:check` | Check formatting without changes |

ESLint is configured with `eslint-plugin-cypress` rules and Prettier integration.

### Security testing with ZAP

The Cypress tests can also be run, proxied via OWASP ZAP for passive security scanning of the application.

These can be run using the configured docker-compose.yml, which will spin up containers for the ZAP daemon and the Cypress tests, including all networking required. You will need to update any config in the file before running

Create a .env file for docker, this file needs to include

* all of your required cypress configuration
* HTTP_PROXY e.g. http://zap:8080
* ZAP_API_KEY, can be any random guid

Example env:

```dotenv
URL=<Enter URL>
API_KEY=<Enter API key>
HTTP_PROXY=http://zap:8080
ZAP_API_KEY=<Enter random guid>
```

_Note: You might have trouble running this locally because of docker thinking localhost is the container and not your machine_

To run docker compose use:

```
docker-compose -f docker-compose.yml --exit-code-from cypress
```

_Note: `--exit-code-from cypress` tells the container to quit when cypress finishes_

You can also exclude URLs from being intercepted by using the NO_PROXY setting

e.g. `NO_PROXY=*.google.com,yahoo.co.uk`

Alternatively, you can run the Cypress tests against an existing ZAP proxy by setting the environment configuration

```dotenv
HTTP_PROXY="<zap-daemon-url>"
NO_PROXY="<list-of-urls-to-ignore>"
```

and setting the runtime variables

```
zapReport=true,zapApiKey=<zap-api-key>,zapUrl="<zap-daemon-url>"
```

### Linting Sonar rules

Include the following extension in your IDE installation: [SonarQube for IDE](https://marketplace.visualstudio.com/items?itemName=SonarSource.sonarlint-vscode)

Update your [settings.json file](https://code.visualstudio.com/docs/getstarted/settings#_settings-json-file) to include the following

```json
"sonarlint.connectedMode.connections.sonarcloud": [   
    {
        "connectionId": "DfE",
        "organizationKey": "dfe-digital",
        "disableNotifications": false
    }   
]
```

Then follow [these steps](https://youtu.be/m8sAdYCIWhY) to connect to the SonarCloud instance.
