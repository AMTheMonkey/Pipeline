using System.Threading;
using System.Threading.Tasks;

namespace Pipeline
{
    public interface IPipeline<in TInput>
    {
        public Task Execute(TInput input, CancellationToken cancellationToken);
    }

    public interface IPipeline<in TInput, TOutput>
    {
        public Task<TOutput> Execute(TInput input, CancellationToken cancellationToken);
    }
}
