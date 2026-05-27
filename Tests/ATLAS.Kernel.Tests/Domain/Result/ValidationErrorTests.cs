using FluentAssertions;
using Xunit;
using ATLAS.Kernel.Domain.Result;

namespace ATLAS.Kernel.Tests.Domain.Result
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
