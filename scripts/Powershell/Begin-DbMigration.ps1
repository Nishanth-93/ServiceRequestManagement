param(
    [Parameter(Mandatory=$true)]
    [string]
    $Migration
)

dotnet ef migrations add $Migration --project .\src\ServiceRequestManagement.Infrastructure\ServiceRequestManagement.Infrastructure.csproj --startup-project .\src\ServiceRequestManagement.API\

dotnet ef database update $Migration --project .\src\ServiceRequestManagement.Infrastructure\ServiceRequestManagement.Infrastructure.csproj --startup-project .\src\ServiceRequestManagement.API\