using FluentAssertions;
using Xunit;
using ATLAS.Kernel.Domain.Result;
using System;

namespace ATLAS.Kernel.Tests.Domain.Result
{
    public class ErrorTests
    {
        [Fact]
        public void ValidationFactory_SetsTypeAndCode()
        {
            Error e = Error.Validation("Code","message");
            e.Type.Should().Be(ErrorType.Validation);
            e.Code.Should().Be("Code");
            e.ToString().Should().Contain("Validation");
        }

        [Theory]
        [InlineData(nameof(ErrorType.NotFound), ErrorType.NotFound)]
        [InlineData(nameof(ErrorType.Conflict), ErrorType.Conflict)]
        [InlineData(nameof(ErrorType.Forbidden), ErrorType.Forbidden)]
        [InlineData(nameof(ErrorType.Unauthorized), ErrorType.Unauthorized)]
        [InlineData(nameof(ErrorType.Unexpected), ErrorType.Unexpected)]
        public void Factories_SetExpectedType(string factory, ErrorType expected)
        {
            Error error = factory switch
            {
                nameof(ErrorType.NotFound) => Error.NotFound("Code", "message"),
                nameof(ErrorType.Conflict) => Error.Conflict("Code", "message"),
                nameof(ErrorType.Forbidden) => Error.Forbidden("Code", "message"),
                nameof(ErrorType.Unauthorized) => Error.Unauthorized("Code", "message"),
                nameof(ErrorType.Unexpected) => Error.Unexpected("Code", "message"),
                _ => throw new InvalidOperationException()
            };

            error.Type.Should().Be(expected);
            error.Code.Should().Be("Code");
            error.Message.Should().Be("message");
        }
    }
}
