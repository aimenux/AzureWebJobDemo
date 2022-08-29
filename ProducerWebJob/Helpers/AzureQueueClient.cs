using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;

namespace ProducerWebJob.Helpers;

public class AzureQueueClient : IAzureQueueClient
{
    private readonly QueueClient _client;
    private readonly ILogger _logger;

    public string QueueName => _client.Name;

    public AzureQueueClient(QueueClient client, ILogger logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task EnqueueMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        var response = await _client.SendMessageAsync(message, cancellationToken);
        if (string.IsNullOrEmpty(response?.Value?.MessageId)) throw new Exception($"Failed to send message '{message}'");
        _logger.LogInformation("Message '{text}' added to the queue '{name}'", message, QueueName);
    }
}