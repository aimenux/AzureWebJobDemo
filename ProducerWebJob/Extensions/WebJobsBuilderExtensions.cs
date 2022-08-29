using System;
using Microsoft.Azure.WebJobs;

namespace ProducerWebJob.Extensions
{
    public static class WebJobsBuilderExtensions
    {
        public static IWebJobsBuilder UseHostId(this IWebJobsBuilder builder)
        {
#if (DEBUG)
            var hostId = Environment.MachineName.ToLowerInvariant();
            builder.UseHostId(hostId);
#endif
            return builder;
        }
    }
}
