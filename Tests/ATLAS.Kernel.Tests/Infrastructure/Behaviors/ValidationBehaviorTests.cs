using ATLAS.Kernel.Domain.Result;
using ATLAS.Kernel.Infrastructure.Behaviors;
using FluentAssertions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace ATLAS.Kernel.Tests.Infrastructure.Behaviors;

public class ValidationBehaviorTests
{
    [Fact]
    public void Type_IsSealed_Generic()
    {
        typeof(ValidationBehavior<,>).IsSealed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_CallsNext_WhenNoValidatorsExist()
    {
        var behavior = new ValidationBehavior<ResultRequest, Result<string>>([]);
        var nextCalls = 0;

        Result<string> result = await behavior.Handle(
            new ResultRequest(""),
            _ =>
            {
                nextCalls++;
                return Task.FromResult(Result<string>.Ok("ok"));
            },
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("ok");
        nextCalls.Should().Be(1);
    }

    [Fact]
    public async Task Handle_CallsNext_WhenValidatorsPass()
    {
        var behavior = new ValidationBehavior<ResultRequest, Result<string>>(
            [new ResultRequestValidator()]);

        Result<string> result = await behavior.Handle(
            new ResultRequest("valid"),
            _ => Task.FromResult(Result<string>.Ok("ok")),
            CancellationToken.None);

        result.Value.Should().Be("ok");
    }

    [Fact]
    public async Task Handle_ReturnsTypedResultFailure_WhenValidationFails()
    {
        var behavior = new ValidationBehavior<ResultRequest, Result<string>>(
            [new ResultRequestValidator()]);
        var nextCalls = 0;

        Result<string> result = await behavior.Handle(
            new ResultRequest(""),
            _ =>
            {
                nextCalls++;
                return Task.FromResult(Result<string>.Ok("ok"));
            },
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Validation.Failed");
        nextCalls.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ReturnsNonGenericResultFailure_WhenValidationFails()
    {
        var behavior = new ValidationBehavior<VoidRequest, Result>(
            [new VoidRequestValidator()]);

        Result result = await behavior.Handle(
            new VoidRequest(""),
            _ => Task.FromResult(Result.Ok()),
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_WhenResponseIsNotResultType()
    {
        var behavior = new ValidationBehavior<StringRequest, string>(
            [new StringRequestValidator()]);

        Func<Task> act = async () => await behavior.Handle(
            new StringRequest(""),
            _ => Task.FromResult("ok"),
            CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    private sealed record ResultRequest(string Name) : IRequest<Result<string>>;
    private sealed record VoidRequest(string Name) : IRequest<Result>;
    private sealed record StringRequest(string Name) : IRequest<string>;

    private sealed class ResultRequestValidator : AbstractValidator<ResultRequest>
    {
        public ResultRequestValidator() => RuleFor(x => x.Name).NotEmpty();
    }

    private sealed class VoidRequestValidator : AbstractValidator<VoidRequest>
    {
        public VoidRequestValidator() => RuleFor(x => x.Name).NotEmpty();
    }

    private sealed class StringRequestValidator : AbstractValidator<StringRequest>
    {
        public StringRequestValidator() => RuleFor(x => x.Name).NotEmpty();
    }
}
