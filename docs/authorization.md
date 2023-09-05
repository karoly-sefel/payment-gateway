# Authorization
The Payment Gateway API implements JWT (JSON Web Token) Bearer token-based authentication for securing endpoints. JWTs include claims such as `merchantId` and scopes to control access to specific API endpoints. 
It's important to note that authentication is handled outside the application's concerns, and tokens should be provided by an external authentication system.

# Authentication
Authentication for the Payment Gateway API is handled externally. This means that before accessing the API, clients should obtain a valid JWT token from an authentication service, such as an Identity Provider (IdP) or an authentication middleware. The JWT token should include the following key components:

**merchantId Claim**: The JWT token should include a merchantId claim that identifies the merchant making the request. This claim helps the API determine the merchant's identity for authorization purposes.

**Scopes**: The JWT token should include scopes that define the actions the bearer of the token is authorized to perform. The Payment Gateway API recognizes two scopes:

- `payment:read`: Grants read access to payment details.
- `payment:process`: Grants permission to process payments.

## Generating JWT Tokens for Development
For development and testing purposes, you can generate JWT tokens with the required claims and scopes using the `dotnet user-jwts` tool. 
This tool allows you to create custom JWT tokens that mimic the behavior of a real authentication system.

Here's an example command to generate a JWT token for development:

```bash
dotnet user-jwts create --scope "payment:read" --scope "payment:process" --expires-on '2030-01-01' --claim "merchantId=merchantA"
```

In this example, the generated JWT token includes the following:

- Scopes: `payment:read` and `payment:process`.
- Expiration date set to `'2030-01-01'`.
- A custom claim `merchantId` with the value `merchantA`.

Please note that in a production environment, JWT tokens should be issued by a trusted authentication service or Identity Provider (IdP) 
following standard authentication protocols such as OAuth 2.0 or OpenID Connect. The provided development command is for testing and development purposes only.

## Token validation

In the Payment Gateway API, issuer authentication has been intentionally disabled in the `appsettings.Development.json` configuration file. 
This configuration choice is made to streamline token sharing among developers during the development and testing phases.

⚠️ While disabling issuer authentication in the development environment simplifies token sharing and testing among developers, it is crucial to understand that this configuration should not be used in a production environment. In production, 
issuer authentication should always be enabled to ensure the security and integrity of authentication tokens.

## JWT tokens for testing

### Token #1 - merchantA full access

```json
Expires On: 2030-01-01T00:00:00.0000000Z
Scopes: payment:read, payment:process
Custom Claims: [merchantId=merchantA]
```

Token: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ikthcm9seSIsInN1YiI6Ikthcm9seSIsImp0aSI6ImQ5ZTFjZWI2Iiwic2NvcGUiOlsicGF5bWVudDpyZWFkIiwicGF5bWVudDpwcm9jZXNzIl0sIm1lcmNoYW50SWQiOiJtZXJjaGFudEEiLCJhdWQiOlsiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSJdLCJuYmYiOjE2OTM5MTE0NjUsImV4cCI6MTg5MzQ1NjAwMCwiaWF0IjoxNjkzOTExNDY2LCJpc3MiOiJkb3RuZXQtdXNlci1qd3RzIn0.x1BJ5EAzumJmY5jtqgUJxYvda_kgerQrDbptN3wUPHg`

### Token #2 - merchantB full access

```json
Expires On: 2030-01-01T00:00:00.0000000Z
Scopes: payment:read, payment:process
Custom Claims: [merchantId=merchantB]
```

Token: ` eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ikthcm9seSIsInN1YiI6Ikthcm9seSIsImp0aSI6IjlhNmRhYzdmIiwic2NvcGUiOlsicGF5bWVudDpyZWFkIiwicGF5bWVudDpwcm9jZXNzIl0sIm1lcmNoYW50SWQiOiJtZXJjaGFudEIiLCJhdWQiOlsiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSJdLCJuYmYiOjE2OTM5MTk1MzcsImV4cCI6MTg5MzQ1NjAwMCwiaWF0IjoxNjkzOTE5NTM4LCJpc3MiOiJkb3RuZXQtdXNlci1qd3RzIn0.UVyy-AvjCI0B1ir6i5sfl3Bk91mry0v3aPcXoxk7FOM`

### Token #3 - merchant A read-only access

```json
Expires On: 2030-01-01T00:00:00.0000000Z
Scopes: payment:read
Custom Claims: [merchantId=merchantA]
```

Token: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ikthcm9seSIsInN1YiI6Ikthcm9seSIsImp0aSI6IjJlMGQ2NmE3Iiwic2NvcGUiOiJwYXltZW50OnJlYWQiLCJtZXJjaGFudElkIjoibWVyY2hhbnRBIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImh0dHBzOi8vbG9jYWxob3N0OjUwMDEiXSwibmJmIjoxNjkzOTE5NjA3LCJleHAiOjE4OTM0NTYwMDAsImlhdCI6MTY5MzkxOTYwOCwiaXNzIjoiZG90bmV0LXVzZXItand0cyJ9.rWlBZaaxt-Bvta50kx9pG8wF2sW3DKV3NvDqjJrbRSg`
