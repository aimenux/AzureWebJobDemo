using System.Threading;
using System.Threading.Tasks;

namespace ProducerWebJob.Helpers
{
    public interface IAzureQueueClient
    {
        Task EnqueueMessageAsync(string message, CancellationToken cancellationToken = default);
    }
}
