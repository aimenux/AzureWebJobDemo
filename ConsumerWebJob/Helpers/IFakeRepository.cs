using System.Threading;
using System.Threading.Tasks;

namespace ConsumerWebJob.Helpers
{
    public interface IFakeRepository
    {
        Task SaveAsync(string message, long retry, CancellationToken cancellationToken = default);
    }
}
