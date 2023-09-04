# Development

## Prerequisites

- JetBrains Rider 2023.1+ (recommended) or Visual Studio 2022
- .NET 7 SDK

## Run / Debug

Solution file: [src/PaymentGateway.sln](../src/PaymentGateway.sln)

To run the Payment Gateway API, use the https launch profile. 
You can access the Payment Gateway API at the following addresses:
- http://localhost:5000
- https://localhost:5001

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
