using ATLAS.Kernel.Abstractions.Interfaces.Infrastructure;
using ATLAS.Kernel.Domain.Result;
using ATLAS.Kernel.Infrastructure.Behaviors;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ATLAS.Kernel.Tests.Infrastructure.Behaviors;

public class TenantBehaviorTests
{
    [Fact]
    public void Type_IsSealed_Generic()
    {
        typeof(TenantBehavior<,>).IsSealed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_CallsNext_WhenTenantIsResolved()
    {
        var behavior = new TenantBehavior<ResultRequest, Result<string>>(
            new TenantContextStub(isResolved: true),
            NullLogger<TenantBehavior<ResultRequest, Result<string>>>.Instance);

        Result<string> result = await behavior.Handle(
            new ResultRequest(),
            _ => Task.FromResult(Result<string>.Ok("ok")),
            CancellationToken.None);

        result.Value.Should().Be("ok");
    }

    [Fact]
    public async Task Handle_ReturnsTypedFailure_WhenTenantIsNotResolved()
    {
        var behavior = new TenantBehavior<ResultRequest, Result<string>>(
            new TenantContextStub(isResolved: false),
            NullLogger<TenantBehavior<ResultRequest, Result<string>>>.Instance);
        var nextCalls = 0;

        Result<string> result = await behavior.Handle(
            new ResultRequest(),
            _ =>
            {
                nextCalls++;
                return Task.FromResult(Result<string>.Ok("ok"));
            },
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Tenant.NotResolved");
        result.Error.Type.Should().Be(ErrorType.Unauthorized);
        nextCalls.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ReturnsNonGenericFailure_WhenTenantIsNotResolved()
    {
        var behavior = new TenantBehavior<VoidRequest, Result>(
            new TenantContextStub(isResolved: false),
            NullLogger<TenantBehavior<VoidRequest, Result>>.Instance);

        Result result = await behavior.Handle(
            new VoidRequest(),
            _ => Task.FromResult(Result.Ok()),
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Tenant.NotResolved");
    }

    [Fact]
    public async Task Handle_ThrowsUnauthorizedAccessException_ForNonResultResponse()
    {
        var behavior = new TenantBehavior<StringRequest, string>(
            new TenantContextStub(isResolved: false),
            NullLogger<TenantBehavior<StringRequest, string>>.Instance);

        Func<Task> act = async () => await behavior.Handle(
            new StringRequest(),
            _ => Task.FromResult("ok"),
            CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*tenant identity was not established*");
    }

    private sealed record ResultRequest : IRequest<Result<string>>;
    private sealed record VoidRequest : IRequest<Result>;
    private sealed record StringRequest : IRequest<string>;

    private sealed class TenantContextStub(bool isResolved) : ITenantContext
    {
        public Guid TenantId { get; } = Guid.NewGuid();
        public string TenantCode { get; } = "TENANT";
        public string Locale { get; } = "es-ES";
        public string TimeZoneId { get; } = "Europe/Madrid";
        public string DefaultCurrencyCode { get; } = "EUR";
        public bool IsResolved { get; } = isResolved;
    }
}
