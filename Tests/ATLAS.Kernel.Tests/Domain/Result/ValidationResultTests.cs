using ATLAS.Kernel.Domain.Result;
using FluentAssertions;
using System;
using Xunit;
using DomainResult = ATLAS.Kernel.Domain.Result.Result;

namespace ATLAS.Kernel.Tests.Domain.Result;

public class ValidationResultTests
{
    [Fact]
    public void Success_ReturnsReusableValidResult()
    {
        ValidationResult first = ValidationResult.Success();
        ValidationResult second = ValidationResult.Success();

        first.Should().BeSameAs(second);
        first.IsValid.Should().BeTrue();
        first.Errors.Should().BeEmpty();
        first.ToResult().IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void WithErrors_CreatesInvalidResultAndConvertsToFailure()
    {
        var errors = new[]
        {
            new ValidationError("Name", "Required", "Required")
        };

        ValidationResult result = ValidationResult.WithErrors(errors);
        DomainResult failure = result.ToResult();
        Result<int> typedFailure = result.ToResult<int>();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Equal(errors);
        failure.IsFailure.Should().BeTrue();
        failure.Error.Code.Should().Be("Validation.Failed");
        failure.Error.Message.Should().Contain("Required");
        typedFailure.IsFailure.Should().BeTrue();
        typedFailure.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public void WithErrors_Throws_WhenErrorsAreNullOrEmpty()
    {
        Action nullErrors = () => ValidationResult.WithErrors(null!);
        Action emptyErrors = () => ValidationResult.WithErrors([]);

        nullErrors.Should().Throw<ArgumentNullException>();
        emptyErrors.Should().Throw<ArgumentException>().WithParameterName("errors");
    }

    [Fact]
    public void ToResultOfT_Throws_WhenValidationIsSuccessful()
    {
        Action act = () => ValidationResult.Success().ToResult<int>();

        act.Should().Throw<InvalidOperationException>();
    }
}
