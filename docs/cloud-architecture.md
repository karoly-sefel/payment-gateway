# Cloud Architecture
This document provides an overview of the cloud architecture designed for the Payment Gateway project using Azure as the cloud provider. 
The proposed solution leverages managed cloud services to ensure both flexibility and high-performance.

The cloud infrastructure to use depends on the exact application requirements, cloud provider preferences, company/team preferences and
existing infrastructure. For example if the company already has a Kubernetes cluster or monitoring solution it makes sense to use those instead of reinventing the wheel.

## Cloud-agnostic Approach

While this document list infrastructure components for the Azure Cloud, it can be adapted to other cloud providers with similar components. 

For instance:
- Azure Cosmos DB can be substituted with AWS DynamoDB.
- Azure Front Door can be replaced with AWS CloudFront.
- Amazon Elastic Container Service can be used instead of Azure Container Apps

## Components

- **Azure Front Door**
    - SSL termination: Provides SSL/TLS encryption termination for incoming traffic
    - Load balancer (Layer 7)
    - Web Application Firewall (WAF): Offers protection against DDoS attacks and web application vulnerabilities.
- **Azure Container Registry**
    - Stores container images
- **Azure Active Directory**
    - Identity Management: Manages client IDs, client secrets and access control for merchants.
- **Cosmos DB**
    - Offers a globally distributed, highly available NoSQL data store with an SQL-like query language.
    - Change Feed: Provides an at least once delivery guarantee for change data capture, ideal for generating materialized views.
- **Key Vault**
    - Application secrets: Safely stores application secrets, ensuring secure access and management.
- **Azure App Configuration**
    - Configuration Management: Stores configuration values for the application, organized with labels for different environments.
- **Azure Container Apps**
    - Managed Kubernetes Alternative: Provides the capabilities of Kubernetes without the need to manage a cluster (AKS).
    - Offers auto scaling using KEDA (Kubernetes Event-driven Autoscaling)
- **Grafana Cloud**
    - Enables the export of OpenTelemetry data, facilitating application monitoring and observability.
