# Areas for improvement

- Clarify requirements/desired behaviour with domain expert and finalise API contract

  > Handling payments is a complex topic, this project is only meant to implement a few, simple use cases leaving lots of room for improvement
  >  
  > For more details on a production-grade API see: https://www.checkout.com/docs/payments/accept-payments/accept-a-payment-using-the-api

- Increase test coverage by adding tests for all supported scenarios
- Create deployment pipelines
- Add Infrastructure as Code (IaC) using Terraform to automate the creation of cloud infrastructure
- Improve validation, e.g by using FluentValidation and a custom MediatR Pipeline Behaviour to run Command and Query validators and return validation results
- Integrate with Azure Key Vault and/or Azure App Configuration (or similar technologies) for configuration and secret management
- Decouple reads and writes
  - The application uses the CQRS pattern which at the moment is driven by a single repository class. Consider using a materialized view for reads
- Add e2e automation tests (e.g. by using Playwright or Selenium)
- Create load and stress tests before deploying the application to production (use [k6](https://k6.io/docs/test-types/load-test-types/) for example)
- Make the application more robust
  - Use Polly for retrying HTTP calls
  - Make the payment processing async and use a message broker/service bus
  - Handle errors and downtime of the Acquiring Bank API
- Logging and monitoring
  - Use OpenTelemetry for distributed tracing, metrics and logs
- Rate limit API requests
- Consider managing idempotency for API operations
