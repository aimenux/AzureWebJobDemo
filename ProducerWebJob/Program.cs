using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProducerWebJob.Configuration;
using ProducerWebJob.Extensions;
using ProducerWebJob.Helpers;
using Serilog;
using Serilog.Debugging;

namespace ProducerWebJob
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebJobs(builder =>
                {
                    builder.AddAzureStorageCoreServices();
                    builder.AddTimers();
                    builder.UseHostId();
                })
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddJsonFile();
                    config.AddUserSecrets();
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                    config.AddAzureWebJobsStorage();
                })
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddSerilog();
                    loggingBuilder.AddNonGenericLogger();
                    loggingBuilder.AddApplicationInsights(hostingContext.Configuration);
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    SelfLog.Enable(Console.Error);

                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.Configure<Settings>(hostingContext.Configuration.GetSection("Settings"));

                    services.AddTransient<IAzureQueueClient, AzureQueueClient>();

                    services
                        .AddAzureClients(builder =>
                        {
                            builder.AddClient<QueueClient, QueueClientOptions>((_, _, serviceProvider) =>
                            {
                                var settings = serviceProvider.GetRequiredService<IOptions<Settings>>().Value;
                                var connectionString = settings.AzureWebJobsStorage;
                                var queueName = settings.AzureQueueName;
                                var options = settings.QueueClientOptions;
                                return new QueueClient(connectionString, queueName, options);
                            });
                        });
                });
    }
}