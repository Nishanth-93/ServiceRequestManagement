using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using ServiceRequestManagement.API.Application.Behaviors;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using ServiceRequestManagement.Infrastructure;
using ServiceRequestManagement.Infrastructure.Repositories;
using System;
using System.IO;

namespace ServiceRequestManagement.API
{
    /// <summary>
    /// Class used to configure application specific items.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The namespace of the application.
        /// </summary>
        public static readonly string Namespace = typeof(Program).Namespace;

        /// <summary>
        /// The application name.
        /// </summary>
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        /// <summary>
        /// Constructor fot the Startup class.
        /// </summary>
        /// <param name="environment"></param>
        public Startup(IWebHostEnvironment environment)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var builtConfig = configBuilder.Build();

            

            if (builtConfig.GetValue<bool>("UseVault", false))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                
                var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                            azureServiceTokenProvider.KeyVaultTokenCallback));

                configBuilder.AddAzureKeyVault(
                    $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                    keyVaultClient,
                    new DefaultKeyVaultSecretManager()); ;
            }

            Configuration = configBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(corsOptions => corsOptions.AddPolicy("AllowAll", policyBuilder =>
            {
                policyBuilder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            }));

            // Add Swagger
            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cohesion Service Request Management RESTful API.",
                    Version = "v1",
                    Description = "Documentation for RESTful API for performing CRUD operations against a ServiceRequest entity."
                });

                var filePath = Path.Combine(AppContext.BaseDirectory, "ServiceRequestManagement.xml");
                
                swaggerOptions.IncludeXmlComments(filePath);
            });

            // TODO: Register Mediatr.
            services.AddMediatR(typeof(Application.Commands.CreateServiceRequestCommandHandler).Assembly);
            
            // Register Mediator behaviors in the order you want them to run in the pipeline.
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            // Register the Db context.
            services.AddDbContext<ServiceRequestManagementContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ServiceRequestManagement"));
            });

            // Register the repository
            services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ServiceRequestManagementContext>())
                {
                    context.Database.Migrate();
                }
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseSwagger();

            app.UseSwaggerUI(swaggerOptions =>
            {
                swaggerOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceRequest API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
