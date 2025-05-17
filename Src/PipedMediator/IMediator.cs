using System.Threading;
using System.Threading.Tasks;

namespace PipedMediator
{
    public interface IMediator
    {
        Task Send<TInput>(TInput input, CancellationToken cancellationToken);

        Task<TOutput> Send<TInput, TOutput>(TInput input, CancellationToken cancellationToken);
    }
}
