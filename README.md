This ASP.NET Core Web API project is a microservice designed to provide information about platforms, such as .NET, SQL Server, Docker, etc. This model is defined below
```csharp

public class Platform
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Publisher { get; set; }

    public required string Cost { get; set; }
}

```
The project implements basic CRUD operations with minimal business logic, allowing the focus to be on deploying microservices using Docker and Kubernetes. The service uses gRPC for synchronous communication and RabbitMQ for 
asynchronous communication with other microservices.

Whenever a platform is created, the platform service will publish an event on the message bus to notify other services about the new platfrom. Additionally, a gRPC gateway is provided, enabling other services to query
the platform service's SQL Server database for all current platforms.
