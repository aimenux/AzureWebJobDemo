using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerWebJob.Helpers;

public class FakeRepository : IFakeRepository
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(1);

    public async Task SaveAsync(string message, CancellationToken cancellationToken = default)
    {
        await Task.Delay(Delay, cancellationToken);
    }
}