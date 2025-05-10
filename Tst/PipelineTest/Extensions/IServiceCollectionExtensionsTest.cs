using Microsoft.Extensions.DependencyInjection;
using Pipeline;
using Pipeline.Extensions;

namespace PipelineTest.Extensions;

public class IServiceCollectionExtensionsTest
{
    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public void AddMediator_Should_InjectMediator(ServiceLifetime lifetime)
    {
        //Arrange
        ServiceCollection sc = new();

        //Act
        sc.AddMediator(lifetime);

        //Assert
        using var serviceProvider = sc.BuildServiceProvider();
        Assert.DoesNotThrow(() => serviceProvider.GetRequiredService<IMediator>());
    }

    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public async Task AddPipeline_Should_Add_Pipeline_And_Be_Functionnal(ServiceLifetime lifetime)
    {
        //Arrange
        var pipeline = Pipeline
            .Pipeline.Pipe((int n, CancellationToken cancellationToken) => Task.FromResult(n + 5))
            .Pipe((n, cancellationToken) => Task.FromResult(n + 10));
        ServiceCollection sc = new();

        //Act
        sc.AddPipeline(pipeline, lifetime);

        //Assert
        using var serviceProvider = sc.BuildServiceProvider();
        Assert.That(
            await serviceProvider
                .GetService<IPipeline<int, int>>()!
                .Execute(5, CancellationToken.None),
            Is.EqualTo(20)
        );
    }

    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public async Task AddPipeline_With_NoReturningValue_Should_Add_Pipeline_And_Be_Functionnal(
        ServiceLifetime lifetime
    )
    {
        //Arrange
        var hasBeenCalled = false;
        var receivedValue = 0;
        var pipeline = Pipeline.Pipeline.Pipe(
            (int n, CancellationToken cancellationToken) =>
            {
                hasBeenCalled = true;
                receivedValue = n;
                return Task.CompletedTask;
            }
        );
        ServiceCollection sc = new();

        //Act
        sc.AddPipeline(pipeline, lifetime);

        //Assert
        using var serviceProvider = sc.BuildServiceProvider();
        var retrievedPipeline = serviceProvider.GetService<IPipeline<int>>();
        await retrievedPipeline!.Execute(65, CancellationToken.None);
        Assert.That(hasBeenCalled, Is.True);
        Assert.That(receivedValue, Is.EqualTo(65));
    }

    [Test]
    public void AddPipeline_With_BadLifetime_should_throw_ArgumentOutOfRange()
    {
        //Arrange
        var pipeline = Pipeline
            .Pipeline.Pipe((int n, CancellationToken cancellationToken) => Task.FromResult(n + 5))
            .Pipe((n, cancellationToken) => Task.FromResult(n + 10));
        ServiceCollection sc = new();

        //Act
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            sc.AddPipeline(pipeline, (ServiceLifetime)5)
        );
    }

    [Test]
    public void AddPipeline_With_NoReturningValue_With_BadLifetime_should_throw_ArgumentOutOfRange()
    {
        //Arrange
        var pipeline = Pipeline.Pipeline.Pipe(
            (int n, CancellationToken cancellationToken) => Task.CompletedTask
        );
        ServiceCollection sc = new();

        //Act
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            sc.AddPipeline(pipeline, (ServiceLifetime)5)
        );
    }

    [Test]
    public void AddMediator_With_BadLifetime_should_throw_ArgumentOutOfRange()
    {
        //Arrange
        ServiceCollection sc = new();

        //Act
        Assert.Throws<ArgumentOutOfRangeException>(() => sc.AddMediator((ServiceLifetime)5));
    }

    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public async Task AddPipeline_With_Factory_Should_Add_Pipeline_And_Be_Functionnal(
        ServiceLifetime lifetime
    )
    {
        //Arrange
        ServiceCollection sc = new();

        //Act
        sc.AddTransient<Handler1>();
        sc.AddTransient<Handler2>();
        sc.AddPipeline(
            (factory) =>
                Pipeline
                    .Pipeline.Pipe<int, int>(factory.GetService<Handler1>()!.Execute)
                    .Pipe(factory.GetService<Handler2>()!.Execute),
            lifetime
        );

        //Assert
        using var serviceProvider = sc.BuildServiceProvider();
        Assert.That(
            await serviceProvider
                .GetService<IPipeline<int, int>>()!
                .Execute(5, CancellationToken.None),
            Is.EqualTo(60)
        );
    }

    private class Handler1 : IPipeline<int, int>
    {
        public Task<int> Execute(int input, CancellationToken cancellationToken)
        {
            return Task.FromResult(input * 2);
        }
    }

    private class Handler2 : IPipeline<int, int>
    {
        public Task<int> Execute(int input, CancellationToken cancellationToken)
        {
            return Task.FromResult(input * 6);
        }
    }
}
