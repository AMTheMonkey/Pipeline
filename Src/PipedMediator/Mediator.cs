using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PipedMediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public Task Send<TInput>(TInput input, CancellationToken cancellationToken)
        {
            var pipeline = _serviceProvider.GetRequiredService<IPipeline<TInput>>();

            return pipeline.Execute(input, cancellationToken);
        }

        public Task<TOutput> Send<TInput, TOutput>(
            TInput input,
            CancellationToken cancellationToken
        )
        {
            var pipeline = _serviceProvider.GetRequiredService<IPipeline<TInput, TOutput>>();

            return pipeline.Execute(input, cancellationToken);
        }
    }
}
