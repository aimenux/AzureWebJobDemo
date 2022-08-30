using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsumerWebJob.Extensions;

public static class LoggingBuilderExtensions
{
    public static void AddNonGenericLogger(this ILoggingBuilder loggingBuilder)
    {
        var categoryName = typeof(Program).Namespace;
        var services = loggingBuilder.Services;
        services.AddSingleton(serviceProvider =>
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            return loggerFactory.CreateLogger(categoryName!);
        });
    }

    public static void AddApplicationInsights(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        var instrumentationKey = configuration["Serilog:WriteTo:2:Args:instrumentationKey"];
        if (!string.IsNullOrWhiteSpace(instrumentationKey))
        {
            loggingBuilder.AddApplicationInsightsWebJobs(options => options.InstrumentationKey = instrumentationKey);
        }
    }
}