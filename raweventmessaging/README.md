# raw-event-messaging Azure Function - CosmosDB

This is .NET Core Azure Function (Http Trigger) &rarr; Event Grid &rarr; Azure Function (Event Grid Trigger) &rarr; Event Hub &rarr; Azure Function (Event Hub Trigger) write to CosmosDB

## Installation

Clone and open in Visual Studio. Make sure you have Azure Package Installed.

Manage your Nuget Package and add Nexus Repository as Package Source.
> https://ecomindo.pkgs.visualstudio.com/Nexus/_packaging/Nexus.Base/nuget/v3/index.json

Install Nexus.Base.CosmosDBRepository to start using the library (this project using version 2020.11.9.1.).

Install AutoMapper by Jimmy Bogard.

Install Microsoft.Azure.WebJobs.Extensions.EventGrid.

Install Microsoft.Azure.WebJobs.Extensions.EventHubs.

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