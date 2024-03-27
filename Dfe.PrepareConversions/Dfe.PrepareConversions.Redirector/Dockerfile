FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dfe.PrepareConversions.Redirector/Dfe.PrepareConversions.Redirector.csproj", "Dfe.PrepareConversions.Redirector/"]
RUN dotnet restore "Dfe.PrepareConversions.Redirector/Dfe.PrepareConversions.Redirector.csproj"
COPY . .

# Stage 2 - Build assets
FROM node:18.20-bullseye as frontend
COPY --from=build /src /src
WORKDIR /src/Dfe.PrepareConversions/wwwroot
RUN npm install
RUN npm run build
RUN mkdir -p /src/Dfe.PrepareConversions.Redirector/wwwroot/css/
RUN mv dist/site.css /src/Dfe.PrepareConversions.Redirector/wwwroot/css/site.css

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS sdk
COPY --from=frontend /src /src
WORKDIR /src/Dfe.PrepareConversions.Redirector
RUN dotnet build "Dfe.PrepareConversions.Redirector.csproj" -c Release
RUN dotnet publish "Dfe.PrepareConversions.Redirector.csproj" -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
COPY --from=sdk /app /app
WORKDIR /app
ENTRYPOINT ["dotnet", "Dfe.PrepareConversions.Redirector.dll"]