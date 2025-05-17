using PipedMediator;

namespace SimpleConsole
{
    /// <summary>
    /// Static class that helps to create different kind of simple pipeline easily
    /// </summary>
    public static class Pipeline
    {
        /// <summary>
        /// Create a new pipeline from a delegate
        /// </summary>
        /// <typeparam name="T1">The input of the pipeline</typeparam>
        /// <typeparam name="T2">The output of the pipeline</typeparam>
        /// <param name="func">The delegate that is wrapped by the pipeline</param>
        /// <returns>A pipeline that wrap a function which from an input <see cref="T1"/> returns an output <see cref="T2"/></returns>
        public static Pipeline<T1, T2> Pipe<T1, T2>(Func<T1, CancellationToken, Task<T2>> func)
        {
            return new Pipeline<T1, T2>(func);
        }

        /// <summary>
        /// Create a pipeline from an asynchronous delegate
        /// </summary>
        /// <typeparam name="T">The input of the delegate</typeparam>
        /// <param name="func">The delegate wrapped by the pipeline</param>
        /// <returns>A pipeline that wrap a function which take an input <see cref="T"/></returns>
        public static Pipeline<T> Pipe<T>(Func<T, CancellationToken, Task> func)
        {
            return new Pipeline<T>(func);
        }
    }

    /// <summary>
    /// A pipeline which wrap a function that take an input <see cref="TInput"/> and return a Task
    /// </summary>
    /// <typeparam name="TInput">The input of the inner function</typeparam>
    public class Pipeline<TInput> : IPipeline<TInput>
    {
        private readonly Func<TInput, CancellationToken, Task> _func;

        public Pipeline(Func<TInput, CancellationToken, Task> func)
        {
            _func = func;
        }

        public Task Execute(TInput input, CancellationToken cancellationToken)
        {
            return _func(input, cancellationToken);
        }
    }

    /// <summary>
    /// A pipeline which wrap a function that take an input <see cref="TInput"/> and return a <see cref="TOutput"/>
    /// </summary>
    /// <typeparam name="TInput">The input of the pipeline</typeparam>
    /// <typeparam name="TOutput">The output of the pipeline</typeparam>
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

        public Task<TOutput> Execute(TInput input, CancellationToken cancellationToken)
        {
            return _func(input, cancellationToken);
        }
    }
}
