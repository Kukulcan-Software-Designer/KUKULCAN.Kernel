using ATLAS.Kernel.Domain.Result;
using Xunit;
using FluentAssertions;
using ATLAS.Kernel.Domain.ValueObjects;

namespace ATLAS.Kernel.Tests.Domain.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_ValidEmail_Succeeds()
    {
        Result<Email> r = Email.Create("User@Example.com");
        r.IsSuccess.Should().BeTrue();
        r.Value.Value.Should().Be("user@example.com");
        r.Value.ToString().Should().Be("user@example.com");
        string value = r.Value;
        value.Should().Be("user@example.com");
    }

    [Theory]
    [InlineData(null, "Email.Empty")]
    [InlineData("", "Email.Empty")]
    [InlineData("not-an-email", "Email.Format.Invalid")]
    public void Create_InvalidEmail_Fails(string? email, string expectedCode)
    {
        Result<Email> r = Email.Create(email);
        r.IsFailure.Should().BeTrue();
        r.Error.Code.Should().Be(expectedCode);
    }

    [Fact]
    public void Create_TooLongEmail_Fails()
    {
        string email = $"{new string('a', 309)}@example.com";

        Result<Email> result = Email.Create(email);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Email.TooLong");
    }
}
