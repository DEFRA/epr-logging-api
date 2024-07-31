# EPR Protective Monitoring Logging Microservice
## Overview
Protective Monitoring Logging Microservice receives events from EPR microservices, adds common metadata to 
those events and sends them to a Service Bus Queue

## Service Bus Payload

```json
{
    "userId": "7b0b6f12-2e1a-4d18-b211-a891afcbb017",
    "sessionId": "97af8e4c-cc48-4684-a36c-2ef33f77b3da",
    "dateTime": "2023-06-14T11:15:07.419Z",
    "component": "epr_pom_api_submission_status",
    "ip": "10.0.1.1",
    "pmcCode": "02-12",
    "priority": 0,
    "details": {
        "transactionCode": "EPR_ANTIVIRUS_CLEAN_UPLOAD",
        "message": "Anti-Virus service identified no threats in uploaded file",
        "additionalInfo": "FileID: 37b6c6d6-9e52-4525-b475-d3b901289482"
    }
}
```

## How To Run

### Prerequisites
In order to run the service you will need the following dependencies
- .NET 8

#### epr-packaging-common
##### Developers working for a DEFRA supplier
In order to restore and build the source code for this project, access to the `epr-packaging-common` package store will need to have been setup.
 - Login to Azure DevOps
 - Navigate to [Personal Access Tokens](https://dev.azure.com/defragovuk/_usersSettings/tokens)
 - Create a new token
   - Enable the `Packaging (Read)` scope
Add the following to your `src/Nuget.Config`
```xml
<packageSourceCredentials>
  <epr-packaging-common>
    <add key="Username" value="<email address>" />
    <add key="ClearTextPassword" value="<personal access token>" />
  </epr-packaging-common>
</packageSourceCredentials>
```
##### Members of the public
Clone the [epr_common](https://dev.azure.com/defragovuk/RWD-CPR-EPR4P-ADO/_git/epr_common) repository and add it as a project to the solution you wish to use it in. By default the repository will reference the files as if they are coming from the NuGet package. You simply need to update the references to make them point to the newly added project.

### Run

- Complete `appsettings.json` file (or `appsettings.Development.json` if exists) with correct details
- On `LoggingMicroservice.API` directory, execute:
```
dotnet run
```

### Docker
Generate a PAT by following the steps here - [Generate Personal Access Token](https://learn.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops&tabs=Windows#create-a-pat),

Then run in terminal at the LoggingMicroservice solution root (`epr-logging-api/LoggingMicroservice`):

```docker build -t loggingmicroservice -f LoggingMicroservice.API/Dockerfile --build-arg PAT={YOUR PAT HERE} .```

Then after that command has completed run:

```docker run -p 5292:3000 --name loggingmicroservicecontainer loggingmicroservice```

Do a GET Request to `http://localhost:5292/admin/health` to confirm that the service is running

## How To Test

### Unit tests

On root directory, execute
```
dotnet test
```

### Pact tests
N/A

### Integration tests
N/A

## How To Debug
Use debugging tools in your chosen IDE

## Environment Variables - deployed environments

The structure of the `appsettings.json` file can be found in the repository. 
Example configurations for the different environments can be found in 
[epr-app-config-settings](https://dev.azure.com/defragovuk/RWD-CPR-EPR4P-ADO/_git/epr-app-config-settings).

| Variable Name                     | Description                                                                                             |
|-----------------------------------|---------------------------------------------------------------------------------------------------------|
| ProtectiveMonitoring__Environment | Protective Monitoring environment (for ex.`DEV`)                                                        |
| ProtectiveMonitoring__Application | Protective Monitoring application name (for ex.`EPR002`)                                                |
| ServiceBus__ConnectionString      | Service Bus connection string                                                                           |
| ServiceBus__QueueName             | Service Bus queue name                                                                                  |
| ServiceBus__RetryDelaySeconds     | Service Bus delay between retry attempts                                                                |
| ServiceBus__RetryMaxDelaySeconds  | Service Bus maximum permissible delay between retry attempts                                            |
| ServiceBus__MaxRetries            | Service Bus maximum number of retry attempts before considering the associated operation to have failed |

## Additional Information
[ADR-028: Protective Monitoring Logging](https://eaflood.atlassian.net/wiki/spaces/MWR/pages/4334060015/ADR-028+Protective+Monitoring+Logging)

### Logging into Azure
N/A

### Usage
N/A

### Monitoring and Health Check
Health check - `{environment}/admin/health`

## Directory Structure

### Source files

- `LoggingMicroservice/LoggingMicroservice.API` - API .Net source files
- `LoggingMicroservice/LoggingMicroservice.API.UnitTests` - API .Net unit test files

## Contributing to this project

Please read the [contribution guidelines](CONTRIBUTING.md) before submitting a pull request.

## Licence

[Licence information](LICENCE.md).
