# nexus-crud Azure Function - CosmosDB

This is .NET Core Azure Function for CosmosDB CRUD using Nexus library and Automapper.

## Installation

Clone and open in Visual Studio. Make sure you have Azure Package Installed.

Manage your Nuget Package and add Nexus Repository as Package Source.
> https://ecomindo.pkgs.visualstudio.com/Nexus/_packaging/Nexus.Base/nuget/v3/index.json

Install Nexus.Base.CosmosDBRepository to start using the library.

Install AutoMapper by Jimmy Bogard.

## Usage
Add variable to local.settings.json

```JSON
"CosmosDBEndPoint": "https://your-endpoint:port/",
"CosmosDBKey": "your_key=="
```

## Contributing
None

## License
None