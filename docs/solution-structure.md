# Payment Gateway Solution Structure
This document provides an overview of the solution structure for the Payment Gateway project, which follows the Clean Architecture pattern. The solution consists of multiple projects, each with a specific responsibility.

The project uses a [feature folder](http://www.kamilgrzybek.com/design/feature-folders/) structure for code organization.

## Projects

### 1. API

The Api project is a .NET 7 Minimal API project that serves as the entry point for external requests. It defines API endpoints and handles HTTP requests and responses.
It accepts HTTP requests and converts them into commands or queries.
This project follows the CQRS pattern using MediatR to delegate command and query handling to the application layer.

#### Key Components:

API Endpoints: Define HTTP endpoints for various features.
Configuration: Configure routing, middleware, and dependency injection.
Program.cs: Configure application services and middleware.

### 2. Application Layer

The Application layer contains application-specific business logic and use cases. 
It serves as an intermediate layer between the API and the core domain.
The application layer supports issuing commands which affect the domain model and executing queries to get data.

It acts as a single point of entry to the application. This is useful if you want to issue commands or execute
queries from multiple clients.

#### Key Components:

- Command Handlers: Handle commands (e.g., processing payments).
- Query Handlers: Handle queries (e.g., retrieving payment details).
- DTOs (Data Transfer Objects): Define data structures for input and output.

### 3. Domain Layer

The Domain project defines the core domain models and entities. It represents the fundamental concepts of the payment gateway system, including payment requests, transactions, and other domain-specific entities.

#### Key Components:

Entities: Define domain models representing core concepts.
Value Objects: Define immutable objects describing aspects of the domain.
Interfaces: Define interfaces for services and repositories.

### 4. Infrastructure Layer

The infrastructure layer provides specific implementations of contracts defined by the domain and application layers. 
It is responsible for data storage, external integrations, and cross-cutting concerns.

#### Key Components:

- Repository Implementations: Implement data storage and retrieval.
- External Service Integrations: Simulate or integrate with external services (e.g., a bank simulator).
- Cross-Cutting Concerns: Implement aspects like logging, caching, and validation.

### 5. Api.Specs Project
   
The Api.Specs project contains tests written using SpecFlow, which is a behavior-driven development (BDD) framework. 
These tests validate the behavior and functionality of the API endpoints defined in the Api project.

#### Key Components:

- SpecFlow Feature Files: Define behavior scenarios in Gherkin language.
- Step Definitions: Implement step definitions to execute scenarios.

## Dependencies

- [MediatR](https://github.com/jbogard/MediatR): Used for implementing the Mediator pattern and handling commands and queries.
- [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions): Used for managing results and functional programming.
- [SpecFlow](https://docs.specflow.org/projects/getting-started/en/latest/): Used for writing behavior-driven tests for the API endpoints.
