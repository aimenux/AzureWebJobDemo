using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProducerWebJob.Helpers;

namespace ProducerWebJob
{
    public class Functions
    {
        private const string FunctionName = "Producer";
        private const string TimeoutExpression = "00:00:30";
        private const string ScheduleExpression = "*/5 * * * *";

        private readonly IAzureQueueClient _client;

        public Functions(IAzureQueueClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        [Singleton]
        [Timeout(TimeoutExpression)]
        [FunctionName(FunctionName)]
        public async Task RunAsync(
            [TimerTrigger(ScheduleExpression, RunOnStartup = true)]
            TimerInfo timer,
            CancellationToken cancellationToken,
            ILogger logger)
        {
            var message = $"Ping {Random.Shared.NextInt64()}";
            await _client.EnqueueMessageAsync(message, cancellationToken);
        }
    }
}
