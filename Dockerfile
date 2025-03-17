# Set the major version of dotnet
ARG DOTNET_VERSION=8.0
# Set the major version of nodejs
ARG NODEJS_VERSION_MAJOR=22

# Build assets
FROM node:${NODEJS_VERSION_MAJOR}-bullseye-slim AS assets
WORKDIR /app
COPY ./Dfe.PrepareConversions/Dfe.PrepareConversions/wwwroot .
RUN npm ci --ignore-scripts && \
    npm run build

# Build the app using the dotnet SDK
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-azurelinux3.0 AS build
WORKDIR /build

# Copy csproj files for restore caching
ARG PROJECT_NAME="Dfe.PrepareConversions"
COPY ${PROJECT_NAME}/${PROJECT_NAME}.Data/${PROJECT_NAME}.Data.csproj                              ./${PROJECT_NAME}/${PROJECT_NAME}.Data/
COPY ${PROJECT_NAME}/${PROJECT_NAME}.DocumentGeneration/${PROJECT_NAME}.DocumentGeneration.csproj  ./${PROJECT_NAME}/${PROJECT_NAME}.DocumentGeneration/
COPY ${PROJECT_NAME}/${PROJECT_NAME}/${PROJECT_NAME}.csproj                                        ./${PROJECT_NAME}/${PROJECT_NAME}/
COPY ${PROJECT_NAME}/Dfe.PrepareTransfers.Data.TRAMS/Dfe.PrepareTransfers.Data.TRAMS.csproj        ./${PROJECT_NAME}/Dfe.PrepareTransfers.Data.TRAMS/
COPY ${PROJECT_NAME}/Dfe.PrepareTransfers.Data/Dfe.PrepareTransfers.Data.csproj                    ./${PROJECT_NAME}/Dfe.PrepareTransfers.Data/
COPY ${PROJECT_NAME}/Dfe.PrepareTransfers.Helpers/Dfe.PrepareTransfers.Helpers.csproj              ./${PROJECT_NAME}/Dfe.PrepareTransfers.Helpers/

WORKDIR /build/${PROJECT_NAME}
RUN --mount=type=secret,id=github_token dotnet nuget add source --username USERNAME --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json" && \
    dotnet restore ${PROJECT_NAME}

# Copy remaining source code
COPY ./${PROJECT_NAME}/ .

# Build and publish
RUN dotnet build ${PROJECT_NAME} --no-restore -c Release && \
    dotnet publish ${PROJECT_NAME} --no-build -c Release -o /app

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
