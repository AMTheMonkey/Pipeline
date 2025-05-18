using PipedMediator;

namespace PipedMediatorTest;

public class Tests
{
    [Test]
    public async Task Pipe_Should_CorrectlyCreateBasicPipeline()
    {
        //Arrange
        var pipeline = 
            Pipeline.Pipe((int a, CancellationToken cancellationToken) => Task.FromResult(a + 2))
            .Pipe((b, cancellationToken) => Task.FromResult(b * 6));

        //Act
        var result = await pipeline.Execute(2, CancellationToken.None);

        //Arrange
        Assert.That(result, Is.EqualTo(24));
    }

    [Test]
    public async Task Pipe_Should_CorrectlyCreatePipelineChangingType()
    {
        //Arrange
        var pipeline = Pipeline
            .Pipe(
                (string value, CancellationToken cancellationToken) =>
                    Task.FromResult(double.Parse(value))
            )
            .Pipe((elem, cancellationToken) => Task.FromResult(elem * 3.0));

        //Act
        var result = await pipeline.Execute("3.0", CancellationToken.None);

        //Arrange
        Assert.That(result, Is.EqualTo(9.0));
    }

    [Test]
    public async Task Pipe_Should_CorrectlyCreateSimplePipelineReturningOnlyTask()
    {
        //Arrange
        var hasBeenCalled = false;
        string receivedValue = string.Empty;
        var pipeline = Pipeline.Pipeline.Pipe(
            (string value, CancellationToken cancellationToken) =>
            {
                hasBeenCalled = true;
                receivedValue = value;
                return Task.CompletedTask;
            }
        );

        Assert.That(hasBeenCalled, Is.False);

        //Act
        await pipeline.Execute("3.0", CancellationToken.None);

        //Assert
        Assert.That(hasBeenCalled, Is.True);
        Assert.That(receivedValue, Is.EqualTo("3.0"));
    }

    [Test]
    public async Task Pipe_Should_CorrectlyCreateComplexPipelineReturningOnlyTask()
    {
        //Arrange
        var hasBeenCalled = false;
        var intermerdiateValue = 0.0;
        var pipeline = Pipeline
            .Pipe(
                (string value, CancellationToken cancellationToken) =>
                    Task.FromResult(double.Parse(value))
            )
            .Pipe((elem, cancellationToken) => Task.FromResult(elem * 3.0))
            .Pipe(
                (elem, cancellationToken) =>
                {
                    hasBeenCalled = true;
                    intermerdiateValue = elem;
                    return Task.CompletedTask;
                }
            );

        Assert.That(hasBeenCalled, Is.False);

        //Act
        await pipeline.Execute("3.0", CancellationToken.None);

        //Assert
        Assert.That(hasBeenCalled, Is.True);
        Assert.That(intermerdiateValue, Is.EqualTo(9.0));
    }
}