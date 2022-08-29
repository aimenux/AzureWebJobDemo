using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues.Models;
using ConsumerWebJob.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ConsumerWebJob
{
    public class Functions
    {
        private const string FunctionName = "Consumer";
        private const string TimeoutExpression = "00:00:30";

        private readonly IFakeRepository _repository;

        public Functions(IFakeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Timeout(TimeoutExpression)]
        [FunctionName(FunctionName)]
        public async Task RunAsync(
            [QueueTrigger("%Settings:AzureQueueName%")] QueueMessage message,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Processing message {text} (retry = {retry})", message.MessageText, message.DequeueCount);
            await _repository.SaveAsync(message.MessageText, cancellationToken);
        }
    }
}
