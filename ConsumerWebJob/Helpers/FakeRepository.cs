using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ConsumerWebJob.Helpers;

public class FakeRepository : IFakeRepository
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(1);

    private readonly ILogger _logger;

    public FakeRepository(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SaveAsync(string message, long retry, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Message '{text}' (retry = {retry}) saved to the database", message, retry);
        await Task.Delay(Delay, cancellationToken);
    }
}