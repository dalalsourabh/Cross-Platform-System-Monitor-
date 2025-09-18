# SystemCheck - Cross-Platform System Resource Monitor

A cross-platform desktop application built in C# that monitors system resources (CPU, memory, disk usage) and supports a plugin system allowing for custom integrations.

## Features

- Real-time monitoring of system resources:
  - CPU usage (%)
  - Memory usage (used/total)
  - Disk usage (used/total)
- Plugin architecture for extensibility
- Included plugins:
  - File Logger: Logs system data to a local file
  - REST API Integration: Posts system data to a configurable endpoint
- Cross-platform compatibility (Windows, Linux, macOS)
- Configurable monitoring interval

## Requirements

- .NET 6.0 SDK or later
- Windows, Linux, or macOS

## How to Build and Run

### Building the Application

1. Clone the repository
2. Navigate to the project directory
3. Build the solution:

```
dotnet build
```

### Running the Application

```
dotnet run --project SystemCheck
```

## Configuration

The application can be configured through the `appsettings.json` file:

```json
{
  "MonitoringIntervalSeconds": 5,
  "ApiEndpoint": "https://example.com/api/systemstats",
  "FileLogger": {
    "FilePath": "logs/system_stats.log"
  }
}
```

- `MonitoringIntervalSeconds`: Time between resource checks (in seconds)
- `ApiEndpoint`: URL for the REST API plugin to send data
- `FileLogger.FilePath`: Path where log files will be stored

## Architecture

This application follows the Clean Architecture pattern with a focus on:

1. **Separation of Concerns**: Core business logic is separated from platform-specific implementations
2. **Dependency Inversion**: High-level modules don't depend on low-level modules
3. **Extensibility**: Plugin system allows for adding new functionality without modifying core code

### Key Components

- **Core Interfaces**: Define contracts for monitoring and plugins
- **Platform-Specific Implementations**: Concrete implementations for Windows (with abstractions for other platforms)
- **Plugin System**: Extensible architecture for adding new data consumers
- **Dependency Injection**: Services are registered and resolved at runtime

## Adding New Plugins

To create a new plugin:

1. Implement the `IMonitorPlugin` interface
2. Register the plugin in the DI container in `Program.cs`

Example:

```csharp
public class MyCustomPlugin : IMonitorPlugin
{
    public string Name => "My Custom Plugin";
    
    public Task InitializeAsync()
    {
        // Setup code
        return Task.CompletedTask;
    }
    
    public Task HandleUpdateAsync(SystemResourceData data)
    {
        // Handle the system resource data
        return Task.CompletedTask;
    }
}
```

## Design Decisions and Challenges

### Cross-Platform Compatibility

The application uses a strategy pattern to abstract platform-specific monitoring code. Currently, Windows implementations are provided, but the architecture allows for easy addition of Linux and macOS implementations.

### Plugin Architecture

The plugin system uses a simple observer pattern where plugins subscribe to system resource updates. This allows for extending functionality without modifying the core monitoring logic.

### Dependency Injection

Microsoft's built-in DI container is used to manage dependencies and facilitate testing. This makes the code more modular and easier to maintain.

## SystemCheckAPI Service

The project includes a dedicated API service to receive, store, and provide system monitoring data:

### Features

- RESTful API built with ASP.NET Core
- Endpoints:
  - `POST /api/SystemData`: Receives system resource data from the monitoring application
  - `GET /api/SystemData`: Returns all stored system resource data (limited to last 100 entries)
  - `GET /api/SystemData/latest`: Returns only the most recent system resource data
- In-memory storage with thread-safe operations
- CORS support for cross-origin requests

### Running the API

```
cd SystemCheckAPI/SystemCheckAPI
dotnet run
```

The API will be available at:
- https://localhost:7080/api/SystemData (HTTPS)
- http://localhost:5080/api/SystemData (HTTP)

### Using with SystemCheck

To use the SystemCheck application with the API, update the `appsettings.json` file:

```json
{
  "ApiEndpoint": "https://localhost:7080/api/SystemData"
}
```

## Limitations

- Full monitoring support is currently implemented for Windows only
- The REST API plugin doesn't handle authentication
- No built-in visualization of historical data