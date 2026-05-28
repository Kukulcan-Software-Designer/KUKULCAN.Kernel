using ATLAS.Kernel.Domain.Result;
using FluentAssertions;
using System;
using Xunit;
using DomainResult = ATLAS.Kernel.Domain.Result.Result;

namespace ATLAS.Kernel.Tests.Domain.Result;

public class ResultTests
{
    [Fact]
    public void Ok_CreatesSuccessfulResult()
    {
        DomainResult result = DomainResult.Ok();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
        result.ToString().Should().Contain("IsSuccess = true");
    }

    [Fact]
    public void Fail_CreatesFailedResult()
    {
        Error error = Error.Conflict("Conflict", "message");

        DomainResult result = DomainResult.Fail(error);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
        result.ToString().Should().Contain("IsFailure = true");
    }

    [Fact]
    public void Fail_Throws_WhenErrorIsNone()
    {
        Action act = () => DomainResult.Fail(Error.None);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ImplicitErrorConversion_CreatesFailedResult()
    {
        DomainResult result = Error.NotFound("Missing", "message");

        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public void OnSuccess_OnFailure_AndMatch_InvokeExpectedCallbacks()
    {
        var successCalled = false;
        Error? failureError = null;

        string successMatch = DomainResult.Ok()
            .OnSuccess(() => successCalled = true)
            .OnFailure(e => failureError = e)
            .Match(() => "ok", e => e.Code);

        successCalled.Should().BeTrue();
        failureError.Should().BeNull();
        successMatch.Should().Be("ok");

        Error error = Error.Validation("Invalid", "message");
        string failureMatch = DomainResult.Fail(error)
            .OnSuccess(() => successCalled = false)
            .OnFailure(e => failureError = e)
            .Match(() => "ok", e => e.Code);

        successCalled.Should().BeTrue();
        failureError.Should().Be(error);
        failureMatch.Should().Be("Invalid");
    }
}
