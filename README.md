# Developing Custom Processors using OpenTelemetry in .NET 8

## SampleApi

SampleApi demonstrates the usage of OpenTelemetry for adding custom processors to enrich the logging in an ASP.NET Core application. It includes a `WeatherForecastController` that serves as an example API endpoint.

### Prerequisites

- .NET Core SDK (version 8.0 or later)

### Getting Started

1. Clone the repository:

```
git clone https://github.com/aws-samples/dotnet-opentelemetry-samples.git
```

2. Navigate to the project directory:

```
cd dotnet-opentelemetry-samples/SampleApi
```

3. Build the project:

```
dotnet build
```

4. Run the project:

```
dotnet run
```

The application should now be running at `https://localhost:7143` or `http://localhost:5192` (or any other port specified in `launchSettings.json`).

### API Endpoints

#### Weather Forecast

- *URL: `/WeatherForecast`*
- *Method: GET*
- *Description: Returns a list of weather forecasts for the next five days.*

### OpenTelemetry Integration

This project uses OpenTelemetry for logging purposes. The following OpenTelemetry components are configured:

1. **Resource Builder**: A default resource builder is used, and a service name "SampleApiService" is added.
2. **Log Processors**:
   - `LogEnrichProcessor`: This processor enriches log events with additional information, such as the HTTP context and the current user's username.
   - `LogExceptionStackTraceProcessor`: This processor adds stack trace information and inner exception details to log events that contain exceptions.
3. **Exporters**:
   - Otlp Exporter: This exporter sends logs to an OpenTelemetry Collector using the OTLP protocol. You can comment out this line and use `AddConsoleExporter()` instead to view logs in the console during local development.

### Logging

This application uses OpenTelemetry for logging, and the logs are enriched with additional information using custom processors. When you run the application and make requests to the `/WeatherForecast` endpoint, you'll see log entries similar to the following:

```json
{
    "body": "Weather Forecast Get API call",
    "severity_number": 9,
    "severity_text": "Information",
    "trace_id": "f18110933d85d8d6ea326ad2b16bee73",
    "span_id": "e3b551998393564b",
    "attributes": {
        "ProcessorName": "LogEnrichProcessor",
        "Username": "Anonymous"
    },
    "scope": {
        "name": "SampleApi.Controllers.WeatherForecastController"
    },
    "resource": {
        "service.instance.id": "f27d1248-8cb1-4aeb-ada2-3eaa0dc08122",
        "service.name": "SampleApiService",
        "telemetry.sdk.language": "dotnet",
        "telemetry.sdk.name": "opentelemetry",
        "telemetry.sdk.version": "1.8.0"
    }
} 
```

The log entries includes the following additional information:
- **ProcessorName**: The name of the log processor that enriched the log entry. In this case, it's the `LogEnrichProcessor`. 
- **Username**: The username of the current user making the request. If the user is not authenticated, it will show as "Anonymous".
- **exception.stacktrace**: If an exception occurred during the request, this field will contain the stack trace of the exception.
- **exception.innerexception**: If an exception occurred during the request and it has an inner exception, this field will contain the inner exception details.

If you want to test the exception logging, you can intentionally throw an exception in the `WeatherForecastController` or any other part of the application. The log entry for the exception will include the stack trace and inner exception details, if applicable.

By default, the logs are sent to an OpenTelemetry Collector using the Otlp exporter. If you want to view the logs in the console during local development, you can comment out the `AddOtlpExporter()` line and uncomment the `AddConsoleExporter()` line in the `Logging` configuration.

```csharp
builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService("SampleApiService"))
        .AddProcessor(new LogEnrichProcessor(new HttpContextAccessor()))
        .AddProcessor(new LogExceptionStackTraceProcessor())
        .AddOtlpExporter();
        //.AddConsoleExporter(); // Uncomment this line for console logging
});
```

With the `AddConsoleExporter()` enabled, the logs will be printed to the console, making it easier to inspect them during development and testing.

### Dependencies

- [OpenTelemetry.Exporter.Console](https://www.nuget.org/packages/OpenTelemetry.Exporter.Console): The console exporter prints data to the Console window. ConsoleExporter supports exporting logs, metrics and traces.
- [OpenTelemetry.Exporter.OpenTelemetryProtocol](https://www.nuget.org/packages/OpenTelemetry.Exporter.OpenTelemetryProtocol): The OTLP (OpenTelemetry Protocol) exporter implementation.

### Contributions

See [CONTRIBUTING](CONTRIBUTING.md) for more information.

### License

This project is licensed under the [MIT License](LICENSE).

### Disclaimer

This project is a sample Weather Forecasting Web API used to demonstrate OpenTelemetry integration and does not incorporate any form of authentication/authorization and is not intended to be used as-is in a Production environment.

The code sample provided in this blog post is intended for educational and demonstration purposes only. While reasonable efforts have been made to ensure the code is functional and secure, users should not rely on it for use in production environments without thorough testing and review.

This code is provided "as-is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose, and non-infringement. In no event shall the author(s) be liable for any claim, damages, or other liability, whether in an action of contract, tort, or otherwise, arising from, out of, or in connection with the code or its use.

Users are responsible for ensuring the code meets their specific requirements and complies with all applicable laws and regulations. It is strongly recommended that users seek professional support and guidance before deploying this code in any production setting.
