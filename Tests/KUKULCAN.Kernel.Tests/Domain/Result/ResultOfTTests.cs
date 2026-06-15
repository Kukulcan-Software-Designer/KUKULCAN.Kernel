using System;
using FluentAssertions;
using KUKULCAN.Kernel.Domain.Result;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.Result;

public class ResultOfTTests
{
    [Fact]
    public void Ok_CarriesValue_And_MapBindMatch_Work()
    {
        Result<int> r = Result<int>.Ok(5);
        r.IsSuccess.Should().BeTrue();
        r.Value.Should().Be(5);

        Result<int> mapped = r.Map(x => x * 2);
        mapped.Value.Should().Be(10);

        Result<string> bound = r.Bind(x => Result<string>.Ok((x * 2).ToString()));
        bound.Value.Should().Be("10");

        string matched = r.Match(x => x.ToString(), err => "err");
        matched.Should().Be("5");
    }

    [Fact]
    public void Fail_CarriesError_AndValueThrows()
    {
        Error error = Error.NotFound("Missing", "message");

        Result<int> result = Result<int>.Fail(error);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
        Action readValue = () => _ = result.Value;
        readValue.Should().Throw<InvalidOperationException>().WithMessage("*Cannot access Value*");
    }

    [Fact]
    public void Fail_Throws_WhenErrorIsNone()
    {
        Action act = () => Result<int>.Fail(Error.None);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ImplicitConversions_CreateSuccessAndFailure()
    {
        Result<int> success = 42;
        Result<int> failure = Error.Forbidden("Forbidden", "message");

        success.Value.Should().Be(42);
        failure.IsFailure.Should().BeTrue();
        failure.Error.Type.Should().Be(ErrorType.Forbidden);
    }

    [Fact]
    public void MapBindMatchAndCallbacks_PropagateFailureWithoutInvokingSuccessDelegates()
    {
        Error error = Error.Unauthorized("Unauthorized", "message");
        Result<int> failure = Result<int>.Fail(error);
        var successCalled = false;
        Error? failureError = null;

        Result<string> mapped = failure.Map(x =>
        {
            successCalled = true;
            return x.ToString();
        });
        Result<string> bound = failure.Bind(x =>
        {
            successCalled = true;
            return Result<string>.Ok(x.ToString());
        });
        string matched = failure.Match(_ => "ok", e => e.Code);

        failure
            .OnSuccess(_ => successCalled = true)
            .OnFailure(e => failureError = e);

        mapped.IsFailure.Should().BeTrue();
        bound.IsFailure.Should().BeTrue();
        matched.Should().Be("Unauthorized");
        successCalled.Should().BeFalse();
        failureError.Should().Be(error);
    }

    [Fact]
    public void ToResult_ConvertsGenericResultToNonGenericResult()
    {
        Result<int>.Ok(5).ToResult().IsSuccess.Should().BeTrue();

        Result<int>.Fail(Error.Unexpected("Unexpected", "message"))
            .ToResult()
            .Error.Type.Should().Be(ErrorType.Unexpected);
    }
}
