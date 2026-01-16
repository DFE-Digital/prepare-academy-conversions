# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:4b8f0b08534833b39bb662fb19a65e78cb086f5ca8dd35de3f87026de8885be4 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5003
EXPOSE 5002


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:aa05b91be697b83229cb000b90120f0783604ad74ed92a0b45cdf3d1a9c873de AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dfe.PrepareConversions/Dfe.PrepareConversions.csproj", "Dfe.PrepareConversions/"]
COPY ["Dfe.PrepareConversions.Data/Dfe.PrepareConversions.Data.csproj", "Dfe.PrepareConversions.Data/"]
COPY ["Dfe.PrepareConversions.DocumentGeneration/Dfe.PrepareConversions.DocumentGeneration.csproj", "Dfe.PrepareConversions.DocumentGeneration/"]
COPY ["Dfe.PrepareTransfers.Data.TRAMS/Dfe.PrepareTransfers.Data.TRAMS.csproj", "Dfe.PrepareTransfers.Data.TRAMS/"]
COPY ["Dfe.PrepareTransfers.Data/Dfe.PrepareTransfers.Data.csproj", "Dfe.PrepareTransfers.Data/"]
COPY ["Dfe.PrepareTransfers.Helpers/Dfe.PrepareTransfers.Helpers.csproj", "Dfe.PrepareTransfers.Helpers/"]
RUN dotnet restore "./Dfe.PrepareConversions/Dfe.PrepareConversions.csproj"
COPY . .
WORKDIR "/src/Dfe.PrepareConversions"
RUN dotnet build "./Dfe.PrepareConversions.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Dfe.PrepareConversions.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dfe.PrepareConversions.dll"]