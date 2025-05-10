using Microsoft.Extensions.DependencyInjection;
using Pipeline;
using Pipeline.Extensions;

namespace PipelineTest
{
    public class MediatorTest
    {
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
        public async Task Send_Should_CorrectlyApplyPipeline_When_SimplePipeline()
        {
            //Arrange
            var hasBeenCalled = false;
            var appliedOperationResult = 0;
            using var serviceProvider = new ServiceCollection()
                .AddPipeline(
                    Pipeline.Pipeline.Pipe(
                        (int n, CancellationToken cancellationToken) =>
                        {
                            hasBeenCalled = true;
                            appliedOperationResult = n * 3;
                            return Task.CompletedTask;
                        }
                    )
                )
                .BuildServiceProvider();
            var mediator = new Mediator(serviceProvider);

            //Act
            await mediator.Send(3, CancellationToken.None);

            //Assert
            Assert.That(hasBeenCalled, Is.True);
            Assert.That(appliedOperationResult, Is.EqualTo(9));
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

        [Test]
        public async Task Send_Should_CorrectlyApplyPipeline_When_ComplexPipeline()
        {
            //Arrange
            using var serviceProvider = new ServiceCollection()
                .AddPipeline(
                    Pipeline.Pipeline.Pipe(
                        (int n, CancellationToken cancellationToken) => Task.FromResult(n * 3)
                    )
                )
                .BuildServiceProvider();
            var mediator = new Mediator(serviceProvider);

            //Act
            var result = await mediator.Send<int, int>(3, CancellationToken.None);

            //Assert
            Assert.That(result, Is.EqualTo(9));
        }
    }
}
