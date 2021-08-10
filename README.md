# Apply to Become an academy, internal service
Internal service for managing applications for schools applying to become academies.

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

Alternatively, there is a Rider plugin called `.NET Core User Secrets` that allows the secrets to be
managed via a json file, accessed by right clicking on the project -> `Tools` -> `Open Project User Secrets`.
