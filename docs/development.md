# Development

## Prerequisites

- JetBrains Rider 2023.1+ (recommended) or Visual Studio 2022
- .NET 7 SDK
- Docker Desktop 4.20.1+

## Run / Debug

Solution file: [src/PaymentGateway.sln](../src/PaymentGateway.sln)

To run the Payment Gateway API, use the https launch profile. 
You can access the Payment Gateway API at the following addresses:
- http://localhost:5000
- https://localhost:5001

## Build and run API in Docker container

> **NOTE**: run all commands from the project root or adjust the paths accordingly

To build a container image for the API run:
```powershell
./scripts/build.ps1
```

This will create a container image called `payment-gateway:latest`

To run the API in a container, use:
```powershell
./scripts/run.ps1
```
The API can be accessed at http://localhost:8000

Optionally, a version parameter can be passed to both scripts to override the default `"latest"` tag
e.g. `./scripts/build.ps1 -version 1.0.0` and `./scripts/run.ps1 -version 1.0.0`

## Run unit tests

The following command will build the test project in a container and execute the tests:

```powershell
./scripts/test.ps1
```

## Troubleshooting

⚠️ "**Your connection isn't private**" warning may appear in the browser when accessing https://localhost:5001

Ensure that the .NET development certificate is installed and trusted on your machine.

To check the HTTPS certificate, open an elevated command prompt and run the following command:
```powershell
dotnet dev-certs https
```

To trust the certificate run:

```powershell
dotnet dev-certs https --trust
```

For more information on the dotnet dev-certs tool, refer to: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs
