# syntax=docker/dockerfile:1
# Build the React frontend, fold it into the .NET API's wwwroot, publish, and run as ONE app.
# Used mainly for LOCAL full-stack parity (docker compose). Production uses code deploy to App Service.

# 1) Frontend build
FROM node:24-alpine AS frontend
WORKDIR /fe
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build

# 2) Backend build + publish (embeds the frontend build into wwwroot)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY backend/ ./
COPY --from=frontend /fe/dist/ ./src/Skuttoo.Api/wwwroot/
RUN dotnet restore src/Skuttoo.Api/Skuttoo.Api.csproj
RUN dotnet publish src/Skuttoo.Api/Skuttoo.Api.csproj -c Release -o /app --no-restore

# 3) Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app ./
# Writable (and, on App Service, persistent) location for the SQLite database.
RUN mkdir -p /home/data && chown -R $APP_UID /home/data
USER $APP_UID
ENV ASPNETCORE_URLS=http://+:8080 \
    ConnectionStrings__Default="Data Source=/home/data/app.db"
EXPOSE 8080
ENTRYPOINT ["dotnet", "Skuttoo.Api.dll"]
