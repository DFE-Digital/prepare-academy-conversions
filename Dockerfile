# Stage 1 - Restore and publish .NET layers
ARG ASPNET_IMAGE_TAG=6.0-bullseye-slim
ARG NODEJS_IMAGE_TAG=18.12-bullseye
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /build

COPY ./Dfe.PrepareConversions/ ./Dfe.PrepareConversions/

WORKDIR /build/Dfe.PrepareConversions
RUN dotnet restore Dfe.PrepareConversions.sln
RUN dotnet build -c Release Dfe.PrepareConversions.sln --no-restore
RUN dotnet publish Dfe.PrepareConversions -c Release -o /app --no-restore

# Stage 2 - Build assets
FROM node:${NODEJS_IMAGE_TAG} as build
COPY --from=publish /app /app
WORKDIR /app/wwwroot
RUN npm install
RUN npm run build

# Stage 3 - Final
ARG ASPNET_IMAGE_TAG
FROM "mcr.microsoft.com/dotnet/aspnet:${ASPNET_IMAGE_TAG}" AS final
COPY --from=build /app /app

WORKDIR /app
COPY ./script/web-docker-entrypoint.sh ./docker-entrypoint.sh
RUN chmod +x ./docker-entrypoint.sh
EXPOSE 80/tcp
