# Set the major version of dotnet
ARG DOTNET_VERSION=8.0
# Set the major version of nodejs
ARG NODEJS_VERSION_MAJOR=22

# Build assets
FROM node:${NODEJS_VERSION_MAJOR}-bullseye-slim AS assets
WORKDIR /app
COPY ./Dfe.PrepareConversions/Dfe.PrepareConversions/wwwroot .
RUN npm install && \
    npm run build

# Build the app using the dotnet SDK
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-azurelinux3.0 AS build
WORKDIR /build

# Copy csproj files for restore caching
COPY Dfe.PrepareConversions/Dfe.PrepareConversions.Data/Dfe.PrepareConversions.Data.csproj ./Dfe.PrepareConversions/Dfe.PrepareConversions.Data/
COPY Dfe.PrepareConversions/Dfe.PrepareConversions.DocumentGeneration/Dfe.PrepareConversions.DocumentGeneration.csproj ./Dfe.PrepareConversions/Dfe.PrepareConversions.DocumentGeneration/
COPY Dfe.PrepareConversions/Dfe.PrepareConversions/Dfe.PrepareConversions.csproj ./Dfe.PrepareConversions/Dfe.PrepareConversions/
COPY Dfe.PrepareConversions/Dfe.PrepareTransfers.Data.TRAMS/Dfe.PrepareTransfers.Data.TRAMS.csproj ./Dfe.PrepareConversions/Dfe.PrepareTransfers.Data.TRAMS/
COPY Dfe.PrepareConversions/Dfe.PrepareTransfers.Data/Dfe.PrepareTransfers.Data.csproj ./Dfe.PrepareConversions/Dfe.PrepareTransfers.Data/
COPY Dfe.PrepareConversions/Dfe.PrepareTransfers.Helpers/Dfe.PrepareTransfers.Helpers.csproj ./Dfe.PrepareConversions/Dfe.PrepareTransfers.Helpers/

WORKDIR /build/Dfe.PrepareConversions
RUN --mount=type=secret,id=github_token dotnet nuget add source --username USERNAME --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json" && \
    dotnet restore Dfe.PrepareConversions

# Copy remaining source code
COPY ./Dfe.PrepareConversions/ .

# Build and publish
RUN dotnet build Dfe.PrepareConversions --no-restore -c Release && \
    dotnet publish Dfe.PrepareConversions --no-build -c Release -o /app

# Copy entrypoint script
COPY ./script/web-docker-entrypoint.sh /app/docker-entrypoint.sh

# Build a runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0 AS final
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/DFE-Digital/prepare-academy-conversions"

# Copy published app and assets
COPY --from=build /app .
COPY --from=assets /app/ ./wwwroot/

# Set permissions and user
RUN chmod +x ./docker-entrypoint.sh
USER $APP_UID
