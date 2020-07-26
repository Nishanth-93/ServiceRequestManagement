using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using ServiceRequestManagement.API.Application.Behaviors;
using System;
using System.IO;

namespace ServiceRequestManagement.API
{
    public class Startup
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public Startup(IWebHostEnvironment environment)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            // TODO: Register Mediatr. Need to use reflection to do this. Don't have one yet.

            // Register Mediator behaviors in the order you want them to run in the pipeline.
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
}
