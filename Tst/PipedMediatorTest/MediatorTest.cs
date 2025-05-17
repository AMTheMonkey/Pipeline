using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PipedMediator;
using PipedMediator.Extensions;
using PipedMediatorTest.Fake;

namespace PipedMediatorTest
{
    public class MediatorTest
    {
        [Test]
        public async Task Send_Should_RetrievePipelineAndExecute_ActionPipeline()
        {
            //Arrange
            var consolePipeline = new FakeConsolePipeline();
            using var serviceProvider = new ServiceCollection()
                .AddSingleton<IPipeline<int>>(consolePipeline)
                .BuildServiceProvider();

            var mediator = new Mediator(serviceProvider);

            //Act
            await mediator.Send(10, CancellationToken.None);

            //Assert
            Assert.That(consolePipeline.HasBeenCalled, Is.True);
            Assert.That(consolePipeline.InnerResult, Is.EqualTo(30));
        }

        [Test]
        public void Send_Should_ThrowInvalidOperation_When_SimplePipelineNotRegisteres()
        {
            //Arrange
            using var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var mediator = new Mediator(serviceProvider);

            // Act/Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                mediator.Send(3, CancellationToken.None)
            );
        }

        [Test]
        public async Task Send_Should_RetrievePipelineAndExecute_FunctionPipeline()
        {
            //Arrange
            using var serviceProvider = new ServiceCollection()
                .AddTransient<IPipeline<int, int>, FakeSquarePipeline>()
                .BuildServiceProvider();

            var mediator = new Mediator(serviceProvider);

            //Act
            var result = await mediator.Send<int, int>(10, CancellationToken.None);

            //Assert
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void Send_Should_ThrowInvalidOperation_When_ComplexPipelineNotRegisteres()
        {
            //Arrange
            using var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var mediator = new Mediator(serviceProvider);

            // Act/Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                mediator.Send<int, string>(3, CancellationToken.None)
            );
        }
    }
}
