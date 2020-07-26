using ServiceRequestManagement.API.Application.Commands;
using ServiceRequestManagement.API.Application.Extensions;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Application.Extensions
{

    public class GenericTypeExtensionsTests
    {
        [Fact]
        public void Given_NonGenericTypedClass_When_CallingGetGenericTypeNameExtension_Then_ReturnsTypeName()
        {
            // Arrange
            var nonGenericClass = new CreateServiceRequestCommand
            {
                BuildingCode = "123",
                CreatedBy = "Aaron",
                Description = "Turn up the heat!"
            };

            // Act
            var actual = nonGenericClass.GetGenericTypeName();

            // Assert
            Assert.Equal("CreateServiceRequestCommand", actual);
        }
    }
}
