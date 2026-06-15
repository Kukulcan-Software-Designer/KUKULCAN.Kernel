using FluentAssertions;
using KUKULCAN.Kernel.Domain.Result;
using Xunit;

namespace KUKULCAN.Kernel.Tests.Domain.Result
{
    public class ValidationErrorTests
    {
        [Fact]
        public void Record_Stores_Values()
        {
            var ve = new ValidationError("Name", "Required", "Name.Required", "");
            ve.PropertyName.Should().Be("Name");
            ve.ErrorMessage.Should().Be("Required");
            ve.ErrorCode.Should().Be("Name.Required");
        }
    }
}
