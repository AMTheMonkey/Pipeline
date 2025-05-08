using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pipeline
{
    public static class Pipeline
    {
        public static Pipeline<T1, T2> Pipe<T1, T2>(Func<T1, CancellationToken, Task<T2>> func)
        {
            return new Pipeline<T1, T2>(func);
        }

        public static Pipeline<T> Pipe<T>(Func<T, CancellationToken, Task> func)
        {
            return new Pipeline<T>(func);
        }
    }

    public class Pipeline<TInput> : IPipeline<TInput>
    {
        private readonly Func<TInput, CancellationToken, Task> _func;

        public Pipeline(Func<TInput, CancellationToken, Task> func)
        {
            _func = func;
        }

        public async Task Execute(TInput input, CancellationToken cancellationToken)
        {
            await _func(input, cancellationToken);
        }
    }

    public class Pipeline<TInput, TOutput> : IPipeline<TInput, TOutput>
    {
        private readonly Func<TInput, CancellationToken, Task<TOutput>> _func;

        public Pipeline(Func<TInput, CancellationToken, Task<TOutput>> func)
        {
            _func = func;
        }

        public Pipeline<TInput, TNextOutput> Pipe<TNextOutput>(
            Func<TOutput, CancellationToken, Task<TNextOutput>> func
        )
        {
            return new Pipeline<TInput, TNextOutput>(
                async (TInput input, CancellationToken cancellationToken) =>
                    await func(await this._func(input, cancellationToken), cancellationToken)
            );
        }

        public Pipeline<TInput> Pipe(Func<TOutput, CancellationToken, Task> func)
        {
            return new Pipeline<TInput>(
                async (TInput input, CancellationToken cancellationToken) =>
                    await func(await this._func(input, cancellationToken), cancellationToken)
            );
        }

        public async Task<TOutput> Execute(TInput input, CancellationToken cancellationToken)
        {
            return await _func(input, cancellationToken);
        }
    }
}
