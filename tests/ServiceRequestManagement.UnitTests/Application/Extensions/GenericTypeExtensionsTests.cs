using ServiceRequestManagement.API;
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
            var nonGenericClass = new WeatherForecast();

            // Act
            var actual = nonGenericClass.GetGenericTypeName();

            // Assert
            Assert.Equal("WeatherForecast", actual);
        }
    }
}
