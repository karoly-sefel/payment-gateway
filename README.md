# Payment Gateway

<img src="docs/assets/icon.png" width="128" width="128">

The Payment Gateway is a HTTP REST API that allows merchants to offer a way for their shoppers to pay for their product.

[![Run tests](https://github.com/karoly-sefel/payment-gateway/actions/workflows/run-tests.yml/badge.svg?branch=main)](https://github.com/karoly-sefel/payment-gateway/actions/workflows/run-tests.yml)

## Key Features

### Payment Processing
Merchants can easily initiate and validate payment transactions, receiving real-time payment status updates.

### Payment Retrieval
Retrieve detailed information about previously made payments, facilitating reconciliation and reporting needs.

## Solution Organization
The solution is organized as follows:

```
├───.github           # GitHub Actions workflows and related files.
├───docs              # Documentation files, including requirements, development guidelines, and API documentation.
├───scripts           # Scripts for building, running, and testing the project.
├───src               # Source code files for the Payment Gateway application.
│   └───PaymentGateway.sln   # Visual Studio solution file.
├───tools             # Tools and utilities related to the project.
├───README.md         # The main README file providing an overview of the project.

```

## Documentation
For detailed information about project requirements and development guidelines, refer to the documentation on the links below:

- [Requirements](docs/requirements.md)
- [Development](docs/development.md)
- [Solution Structure](docs/solution-structure.md)
- [Testing](docs/testing.md)
- [API Documentation](docs/api-documentation.md)
- [Authentication/Authorization](docs/authorization.md)
- [Pipelines](docs/pipelines.md)
- [Cloud Architecture](docs/cloud-architecture.md)
- [Areas for improvement](docs/areas-for-improvement.md)
- [Extra mile bonus points](docs/extra-mile.md)
