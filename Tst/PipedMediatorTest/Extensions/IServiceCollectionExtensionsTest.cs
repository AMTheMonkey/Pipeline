using Microsoft.Extensions.DependencyInjection;
using PipedMediator;
using PipedMediator.Extensions;
using PipedMediatorTest.Fake;

namespace PipedMediatorTest.Extensions;

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
    public void AddPipeline_Should_InjectPipeline_ActionPipeline(ServiceLifetime lifetime)
    {
        //Arrange
        ServiceCollection sc = new();

        //Act
        sc.AddPipeline<FakeConsolePipeline, int>(lifetime);

        //Assert
        using var serviceProvider = sc.BuildServiceProvider();
        Assert.DoesNotThrow(() => serviceProvider.GetRequiredService<IPipeline<int>>());
    }

    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public void AddPipeline_Should_InjectPipeline_FunctionPipeline(ServiceLifetime lifetime)
    {
        //Arrange
        ServiceCollection sc = new();

        //Act
        sc.AddPipeline<FakeSquarePipeline, int, int>(lifetime);

        //Assert
        using var serviceProvider = sc.BuildServiceProvider();
        Assert.DoesNotThrow(() => serviceProvider.GetRequiredService<IPipeline<int, int>>());
    }
}
