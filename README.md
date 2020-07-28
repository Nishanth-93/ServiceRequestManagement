# Service Request Management API


## Table of Contents

- [Introduction](#introduction)
- [Demo](#demo)
- [Features](#features)
- [Design Considerations](#design-considerations)
- [Technologies](#technologies)

# Introduction

[![codecov](https://codecov.io/gh/aaron-jaeger/ServiceRequestManagement/branch/main/graph/badge.svg)](https://codecov.io/gh/aaron-jaeger/ServiceRequestManagement)
![Build](https://github.com/aaron-jaeger/ServiceRequestManagement/workflows/Build/badge.svg)
![Deploy](https://github.com/aaron-jaeger/ServiceRequestManagement/workflows/Deploy/badge.svg)

A simple API that provides functionality to improve tenant experience in a building while improving communication between building staff and tenants.

## Demo

[Try out the API with Swagger here!](https://servicerequestmanagement.azurewebsites.net/swagger/index.html)

## Features

This API allows tenants and building staff to create, read, update, and delete service requests.

## Design Considerations

This API was developed with domain driven design ([DDD](https://martinfowler.com/tags/domain%20driven%20design.html)), [CQRS](https://www.martinfowler.com/bliki/CQRS.html) and clean architecture as the core principles. 

## Technologies

- [.Net Core 3.1](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-3-1)
- [.Net Standard 2.1](https://devblogs.microsoft.com/dotnet/announcing-net-standard-2-1/)
- [MediatR](https://github.com/jbogard/MediatR) *Personally one of my favorite NuGet package.
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Swashbuckle](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio)
- [xUnit](https://xunit.github.io/)
- [Serilog](https://serilog.net/)
- [Coverlet](https://github.com/coverlet-coverage/coverlet)
- [Codecov.io](https://codecov.io/gh/aaron-jaeger/ServiceRequestManagement)
- [Azure Web Apps](https://azure.microsoft.com/en-us/services/app-service/web/)
- [Azure Container Registry](https://azure.microsoft.com/en-us/services/container-registry/)
- [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/)
- [Azure SQL Database](https://azure.microsoft.com/en-us/services/sql-database/)

