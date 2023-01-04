# APIM internal portal

## About

.net 7 implementation of APIM portal

### Dependencies and technologies

Technologies used:

- Net 7

## Installation

### 1. Update the user secrets

This project uses the ASP.NET Core [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) to store sesitive data. If using Visual Studio, right-click on each web project specified below and select "Manage User Secrets" - fill in `secrets.json` with the following:

```json
{
	"ManagmentApi": {
        "SubscriptionPath": "/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/resourceGroups/xxxxx/providers/Microsoft.ApiManagement/service/#apim resource name#",
        "ManagementApiPrimaryKey": "#apim managment api key#",
        "ManagementApiId": "#apim managment api id#",
        "BackendUrl": "#apim base url#",
  }
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
```

## Test

UI tests require the project to be running and TEST_HOST in app settings to point to the app's running url

All other tests can be run without setup

```console
dotnet test
```