using System;
using Microsoft.Extensions.DependencyInjection;

namespace PipedMediator.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add IMediator to the dependency injection container
        /// </summary>
        /// <param name="serviceCollection">The service collection of you application</param>
        /// <param name="serviceLifetime">The service lifetime desired</param>
        /// <returns>The service collection with IMediator added</returns>
        public static IServiceCollection AddMediator(
            this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
        {
            return serviceCollection.Add<IMediator, Mediator>(serviceLifetime);
        }

        /// <summary>
        /// Add an pipeline, in the dependency injection container
        /// </summary>
        /// <typeparam name="TPipeline">The type of the pipeline's implementation</typeparam>
        /// <typeparam name="T1">The input type of the pipeline</typeparam>
        /// <typeparam name="T2">The output type of the pipeline</typeparam>
        /// <param name="serviceCollection">The service collection of your application</param>
        /// <param name="serviceLifetime">The service lifetime desired</param>
        /// <returns>The service collection with the IPipeline<T1,T2> added</returns>
        public static IServiceCollection AddPipeline<TPipeline, T1, T2>(
            this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
            where TPipeline : IPipeline<T1, T2>
        {
            return serviceCollection.Add<IPipeline<T1, T2>, TPipeline>(serviceLifetime);
        }

        /// <summary>
        /// Add an IPipeline<<typeparamref name="T1"/>>in the dependency injection container
        /// </summary>
        /// <typeparam name="TPipeline">The type of the pipeline's implementation</typeparam>
        /// <typeparam name="T1">The input type of the pipeline</typeparam>
        /// <param name="serviceCollection">The service collection of your application</param>
        /// <param name="serviceLifetime">The service lifetime desired</param>
        /// <returns>The service collection with the IPipeline<T1> added</returns>
        public static IServiceCollection AddPipeline<TPipeline, T1>(
            this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
            where TPipeline : IPipeline<T1>
        {
            return serviceCollection.Add<IPipeline<T1>, TPipeline>(serviceLifetime);
        }

        /// <summary>
        /// Add a IPipeline which take one Input and returns a Task
        /// </summary>
        /// <typeparam name="TInput">The input of the pipeline</typeparam>
        /// <param name="serviceCollection">The service collection of your application</param>
        /// <param name="func">A function builder to build an IPipeline</param>
        /// <param name="serviceLifetime"></param>
        /// <returns>The service lifetime desired</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when ServiceLifetime is out of range (Transient, Scoped, Singleton)</exception>
        public static IServiceCollection AddPipeline<TInput>(
            this IServiceCollection serviceCollection,
            Func<PipelineBuilder, IPipeline<TInput>> func,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
        {
            Func<IServiceProvider, IPipeline<TInput>> factory = (sp) =>
                func(new PipelineBuilder(sp));

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    return serviceCollection.AddSingleton(factory);
                case ServiceLifetime.Scoped:
                    return serviceCollection.AddScoped(factory);
                case ServiceLifetime.Transient:
                    return serviceCollection.AddTransient(factory);
            }

            throw new ArgumentOutOfRangeException(nameof(serviceLifetime));
        }

        /// <summary>
        /// Add a IPipeline which take one Input and returns an Output
        /// </summary>
        /// <typeparam name="TInput">The input of the pipeline</typeparam>
        /// <typeparam name="TOutput">The output of the pipeline</typeparam>
        /// <param name="serviceCollection">The service collection of your application</param>
        /// <param name="func">A function builder to build an IPipeline</param>
        /// <param name="serviceLifetime"></param>
        /// <returns>The service lifetime desired</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when ServiceLifetime is out of range (Transient, Scoped, Singleton)</exception>
        public static IServiceCollection AddPipeline<TInput, TOutput>(
            this IServiceCollection serviceCollection,
            Func<PipelineBuilder, IPipeline<TInput, TOutput>> func,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
        {
            Func<IServiceProvider, IPipeline<TInput, TOutput>> factory = (sp) =>
                func(new PipelineBuilder(sp));

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    return serviceCollection.AddSingleton(factory);
                case ServiceLifetime.Scoped:
                    return serviceCollection.AddScoped(factory);
                case ServiceLifetime.Transient:
                    return serviceCollection.AddTransient(factory);
            }

            throw new ArgumentOutOfRangeException(nameof(serviceLifetime));
        }

        private static IServiceCollection Add<TService, TImplementation>(
            this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
        {
            serviceCollection.Add(
                new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime)
            );

            return serviceCollection;
        }
    }
}
