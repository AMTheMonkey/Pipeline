using PipedMediator;

namespace PipedMediatorTest.Fake;

public class FakeConsolePipeline : IPipeline<int>
{
    public bool HasBeenCalled { get; private set; } = false;
    public int? InnerResult { get; private set; } = null;

    public Task Execute(int input, CancellationToken cancellationToken)
    {
        HasBeenCalled = true;
        InnerResult = input * 3;

        Console.WriteLine($"result is : {InnerResult}");

        return Task.CompletedTask;
    }
}

public class FakeSquarePipeline : IPipeline<int, int>
{
    public bool HasBeenCalled { get; private set; } = false;

    public Task<int> Execute(int input, CancellationToken cancellationToken)
    {
        return Task.FromResult(input * input);
    }
}
