using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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
        /// Sets the application configuration.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomControllers()
                .AddCustomDbContext(Configuration)
                .AddCustomSwagger()
                .AddCustomMediatR()
                .AddCustomRepositories();            
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

    /// <summary>
    /// Custom extensions for the IServiceCollection interface.
    /// </summary>
    static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds customized services for controllers.
        /// </summary>
        /// <param name="services">The IServiceCollection this method is extending.</param>
        /// <returns>The IServiceCollection this method is extending.</returns>
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(corsOptions => 
            {
                corsOptions.AddPolicy("AllowAll", policyBuilder =>
                    policyBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());
            });

            return services;
        }

        /// <summary>
        /// Adds customized Db context for services.
        /// </summary>
        /// <param name="services">The IServiceCollection this method is extending.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection this method is extending.</returns>
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Db context.
            services.AddDbContext<ServiceRequestManagementContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ServiceRequestManagement"));
            });

            return services;
        }

        /// <summary>
        /// Adds customized Swagger for services.
        /// </summary>
        /// <param name="services">The IServiceCollection this method is extending.</param>
        /// <returns>The IServiceCollection this method is extending.</returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
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

            return services;
        }

        /// <summary>
        /// Adds customized MediatR for services and adds pipeline behaviors.
        /// </summary>
        /// <param name="services">.</param>
        /// <returns>The IServiceCollection this method is extending.</returns>
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Application.Commands.CreateServiceRequestCommandHandler).Assembly);

            // Register pipeline behaviors in the order you want them to run in the pipeline.
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            return services;
        }

        /// <summary>
        /// Adds custom repositories for services.
        /// </summary>
        /// <param name="services">The IServiceCollection this method is extending</param>
        /// <returns>The IServiceCollection this method is extending</returns>
        public static IServiceCollection AddCustomRepositories(this IServiceCollection services)
        {
            services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();

            return services;
        }
    }
}
