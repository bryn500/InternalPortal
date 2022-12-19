# APIM internal portal

## About

.net 6 implementation of APIM portal

### Dependencies and technologies

Technologies used:

- Net 6

## Installation

### 1. Update the user secrets

This project uses the ASP.NET Core [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) to store sesitive data. If using Visual Studio, right-click on each web project specified below and select "Manage User Secrets" - fill in `secrets.json` with the following:

```json
{
}
```

## Build

npm scripts copy and minify required govuk files and transpile any scss files to css

```console
npm run build
```

The scss can be watched and automatically rebuilt with changes with the following

```console
npm run dev
```

## Run

```console
dotnet build
dotnet run
``
