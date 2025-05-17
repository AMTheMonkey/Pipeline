using System.Threading;
using System.Threading.Tasks;

namespace PipedMediator
{
    /// <summary>
    /// Describe a pipeline wrapping a function taking <see cref="TInput"/> and returning a Task
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public interface IPipeline<in TInput>
    {
        /// <summary>
        /// Execute the pipeline
        /// </summary>
        /// <param name="input">The input of the pipeline</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task</returns>
        public Task Execute(TInput input, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Describe a pipeline wrapping a function taking <see cref="TInput"/> and returning <see cref="TOutput"/>
    /// </summary>
    /// <typeparam name="TInput">The input of the pipeline</typeparam>
    /// <typeparam name="TOutput">The ouput of the pipeline</typeparam>
    public interface IPipeline<in TInput, TOutput>
    {
        /// <summary>
        /// Execute the pipeline
        /// </summary>
        /// <param name="input">The input of the pipeline</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task of <see cref="TOutput"/></returns>
        public Task<TOutput> Execute(TInput input, CancellationToken cancellationToken);
    }
}
