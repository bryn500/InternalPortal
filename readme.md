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

## Run

```
dotnet build
dotnet run
```

