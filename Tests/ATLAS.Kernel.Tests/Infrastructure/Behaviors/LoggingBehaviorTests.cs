using ATLAS.Kernel.Infrastructure.Behaviors;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ATLAS.Kernel.Tests.Infrastructure.Behaviors;

public class LoggingBehaviorTests
{
    [Fact]
    public void Type_IsSealed_Generic()
    {
        typeof(LoggingBehavior<,>).IsSealed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ReturnsNextResponse()
    {
        var behavior = new LoggingBehavior<Request, string>(NullLogger<LoggingBehavior<Request, string>>.Instance);

        string result = await behavior.Handle(new Request(), _ => Task.FromResult("ok"), CancellationToken.None);

        result.Should().Be("ok");
    }

    [Fact]
    public async Task Handle_PropagatesNextException()
    {
        var behavior = new LoggingBehavior<Request, string>(NullLogger<LoggingBehavior<Request, string>>.Instance);

        Func<Task> act = async () => await behavior.Handle(
            new Request(),
            _ => throw new InvalidOperationException("boom"),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("boom");
    }

    [Fact]
    public void SlowRequestThresholdMs_CanBeConfigured()
    {
        int original = LoggingBehavior<Request, string>.SlowRequestThresholdMs;

        try
        {
            LoggingBehavior<Request, string>.SlowRequestThresholdMs = 1;

            LoggingBehavior<Request, string>.SlowRequestThresholdMs.Should().Be(1);
        }
        finally
        {
            LoggingBehavior<Request, string>.SlowRequestThresholdMs = original;
        }
    }

    private sealed record Request : IRequest<string>;
}
