# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /build

# Copy csproj and restore as distinct layers
COPY ApplyToBecome.Data.Mock/ ./ApplyToBecome.Data.Mock/
COPY ApplyToBecome.Data/ ./ApplyToBecome.Data/
COPY DocumentGeneration/ ./DocumentGeneration/
COPY ApplyToBecomeInternal/ ./ApplyToBecomeInternal/

WORKDIR ApplyToBecomeInternal
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Stage 2
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ApplyToBecome.dll"]
