using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceRequestManagement.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRequestManagement.FunctionalTests
{
    public class ServiceRequestManagementTestsStartup : Startup
    {
        public ServiceRequestManagementTestsStartup(IConfiguration environment) : base(environment)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Added in to show how we can configure custom services.
            base.ConfigureServices(services);
        }
    }
}
