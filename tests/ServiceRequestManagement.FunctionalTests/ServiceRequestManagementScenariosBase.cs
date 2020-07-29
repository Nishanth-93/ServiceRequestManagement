using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using ServiceRequestManagement.API;
using ServiceRequestManagement.Infrastructure;
using System;
using System.IO;
using System.Reflection;

namespace ServiceRequestManagement.FunctionalTests
{
    public class ServiceRequestManagementScenariosBase
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(ServiceRequestManagementScenariosBase))
                .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(configBuilder =>
                {
                    configBuilder.AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables();
                })
                .UseStartup<ServiceRequestManagementTestsStartup>();

            var testServer = new TestServer(hostBuilder);

            testServer.Host.ApplyDbMigration<ServiceRequestManagementContext>();

            return testServer;
        }

        public static class Get
        {
            public static string ServiceRequests = "api/servicerequest";

            public static string ServiceRequestById(Guid id)
            {
                return $"api/servicerequest/{id}";
            }
        }

        public static class Put
        {
            public static string ServiceRequestById(Guid id)
            {
                return $"api/servicerequest/{id}";
            }
        }

        public static class Post
        {
            public static string ServiceRequest = "api/servicerequest";
        }

        public static class Delete
        {
            public static string ServiceRequestById(Guid id)
            {
                return $"api/servicerequest/{id}";
            }
        }
    }
}
