using System;
using System.Threading.Tasks;
using ConsumerWebJob.Extensions;
using ConsumerWebJob.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;

namespace ConsumerWebJob
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
                    builder.AddAzureStorageQueues(options =>
                    {
                        options.BatchSize = 1;
                        options.MaxDequeueCount = 3;
                    });
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile();
                    config.AddUserSecrets();
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                    config.AddAzureWebJobsStorage();
                })
                .ConfigureLogging((_, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddNonGenericLogger();
                    loggingBuilder.AddSerilog();
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    SelfLog.Enable(Console.Error);

                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddSingleton<IFakeRepository, FakeRepository>();
                });
    }
}