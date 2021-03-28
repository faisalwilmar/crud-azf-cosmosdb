# nexus-crud Azure Function - CosmosDB

This is .NET Core Azure Function with Domain-Driven Structure for CosmosDB CRUD using Nexus library and Automapper.
Also sending (and receiving) notification using Azure Event Grid and Event Hubs.

This project already using DDD (Domain-driven Design), Swashbuckle Swagger UI API Documentation, and XUnit for unit test.

## Installation

Clone and open in Visual Studio. Make sure you have Azure Package Installed.

Manage your Nuget Package and add Nexus Repository as Package Source.
> https://ecomindo.pkgs.visualstudio.com/Nexus/_packaging/Nexus.Base/nuget/v3/index.json

Using:
- Nexus.Base.CosmosDBRepository to start using the library (this project using version 2020.11.9.1.)
- AutoMapper by Jimmy Bogard
- AzureExtension.Swashbuckle by vitalybibikov
- Moq by Daniel Cazzulino
- XUnit by James Newkirk, Brad Wilson

Don't forget to read every PackageReference in every csproj file and install the Package.

## Usage
Add variable to local.settings.json

```JSON
"CosmosDBEndPoint": "https://your-endpoint:port/",
"CosmosDBKey": "your_key==",
"EventGridEndPoint": "your_event_grid_endpoint_on_overview_page",
"EventGridEndKey": "your_event_grid_key",
"EventHubConnectionString": "your_event_hub_key_in_SharedAccessPolicies"
```

Make sure you have:
1. CosmosDB
2. Event Hubs
3. Event Grid

## Contributing
None

## License
None