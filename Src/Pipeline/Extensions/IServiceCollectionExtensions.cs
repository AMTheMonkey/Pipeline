using System;
using Microsoft.Extensions.DependencyInjection;

namespace Pipeline.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(
            this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient
        )
        {
            return serviceLifetime switch
            {
                ServiceLifetime.Transient => serviceCollection.AddTransient<IMediator, Mediator>(),
                ServiceLifetime.Scoped => serviceCollection.AddScoped<IMediator, Mediator>(),
                ServiceLifetime.Singleton => serviceCollection.AddSingleton<IMediator, Mediator>(),
                _ => throw new ArgumentOutOfRangeException(nameof(serviceLifetime)),
            };
        }

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
            return serviceLifetime switch
            {
                ServiceLifetime.Transient => serviceCollection.AddTransient(factory),
                ServiceLifetime.Scoped => serviceCollection.AddScoped(factory),
                ServiceLifetime.Singleton => serviceCollection.AddSingleton(factory),
                _ => throw new ArgumentOutOfRangeException(nameof(serviceLifetime)),
            };
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
            return serviceLifetime switch
            {
                ServiceLifetime.Transient => serviceCollection.AddTransient(factory),
                ServiceLifetime.Scoped => serviceCollection.AddScoped(factory),
                ServiceLifetime.Singleton => serviceCollection.AddSingleton(factory),
                _ => throw new ArgumentOutOfRangeException(nameof(serviceLifetime)),
            };
        }
    }
}
