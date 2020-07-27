using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceRequestManagement.Infrastructure;
using System;
using System.IO;
using System.Net;

namespace ServiceRequestManagement.API
{
    /// <summary>
    /// The program class.
    /// </summary>
    public class Program
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
        /// The main method of the application.
        /// </summary>
        /// <param name="args">Application arguments.</param>
        /// <returns>Exit code</returns>
        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = CreateWebHost(configuration, args);

                Log.Information("Applying Db migrations ({ApplicationContext})...", AppName);
                host.ApplyDbMigration<ServiceRequestManagementContext>();

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program encountered a fatal error ({ApplicationContext})", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Builds the application configuration.
        /// </summary>
        /// <returns>Built IConfiguration</returns>
        private static IConfiguration GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var builtConfig = configBuilder.Build();

            // Check the configuration for whether or not we want to use Azure Key Vault.
            if (builtConfig.GetValue<bool>("UseKeyVault", false))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();

                var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                            azureServiceTokenProvider.KeyVaultTokenCallback));

                configBuilder.AddAzureKeyVault(
                    $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                    keyVaultClient,
                    new DefaultKeyVaultSecretManager());
            }
            
            return configBuilder.Build();
        }

        /// <summary>
        /// Creates the Serilog logger.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>Created Serilog Logger</returns>
        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        /// <summary>
        /// Creates the web host.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="args">The application args.</param>
        /// <returns>Created WebHost.</returns>
        private static IWebHost CreateWebHost(IConfiguration configuration, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .ConfigureKestrel(kestrelOptions =>
                {
                    kestrelOptions.Listen(IPAddress.Any, 80, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });
                })
                .ConfigureAppConfiguration(configureDelegate => configureDelegate.AddConfiguration(configuration))
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();
    }

    /// <summary>
    /// Class that holds web host extensions methods.
    /// </summary>
    public static class WebHostExtensions
    {
        /// <summary>
        /// Applies a db migration.
        /// </summary>
        /// <typeparam name="T">DbContext</typeparam>
        /// <param name="webHost">The web host of the application being extended.</param>
        /// <returns>The web host of the application being extended.</returns>
        public static IWebHost ApplyDbMigration<T>(this IWebHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<T>>();
                var context = services.GetService<T>();

                try
                {
                    context.Database.Migrate();
                    logger.LogInformation("Db migration applied to context {DbContextName}", typeof(T).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured while performing a Db migration with the context {DbContextName}", typeof(T).Name);
                }

            }

            return webHost;
        }
    }
}
