using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Pipeline.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPipeline<TInput>(
            this IServiceCollection serviceCollection,
            IPipeline<TInput> pipeline,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
        {
            return serviceCollection.AddPipeline(_ => pipeline, serviceLifetime);
        }

        public static IServiceCollection AddPipeline<TInput>(
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, IPipeline<TInput>> factory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Transient:
                    return serviceCollection.AddTransient(factory);
                case ServiceLifetime.Scoped:
                    return serviceCollection.AddScoped(factory);
                case ServiceLifetime.Singleton:
                    return serviceCollection.AddSingleton(factory);
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime));
            }
        }

        public static IServiceCollection AddPipeline<TInput, TOutput>(
            this IServiceCollection serviceCollection,
            IPipeline<TInput, TOutput> pipeline,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
        {
            return serviceCollection.AddPipeline(_ => pipeline, serviceLifetime);
        }

        public static IServiceCollection AddPipeline<TInput, TOutput>(
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, IPipeline<TInput, TOutput>> factory,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Transient:
                    return serviceCollection.AddTransient(factory);
                case ServiceLifetime.Scoped:
                    return serviceCollection.AddScoped(factory);
                case ServiceLifetime.Singleton:
                    return serviceCollection.AddSingleton(factory);
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime));
            }
        }
    }
}
