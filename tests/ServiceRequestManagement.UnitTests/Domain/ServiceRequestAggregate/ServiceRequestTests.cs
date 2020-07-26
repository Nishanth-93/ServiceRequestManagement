using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Domain.ServiceRequestAggregate
{
    public class ServiceRequestTests
    {
        [Fact]
        public void Given_RequiredParameters_When_Contstructing_ServiceRequest_Then_FieldsSetCorrectly()
        {
            // Arrange
            var expectedBuildingCode = "ABC";
            var expectedDescription = "DEFGHIJKLMNOPQRSTUVWXYZ";
            var expectedCreatedBy = "Aaron";

            // Act
            var actual = new ServiceRequest(expectedBuildingCode, expectedDescription, expectedCreatedBy);

            // Assert
            Assert.Equal(expectedBuildingCode, actual.GetBuildingCode);
            Assert.Equal(expectedDescription, actual.GetDescription);
            Assert.Equal(expectedCreatedBy, actual.GetCreatedBy);
            Assert.InRange(actual.GetCreatedDate, DateTime.UtcNow.AddSeconds(-30), DateTime.UtcNow);
            Assert.Equal(CurrentStatus.Created, actual.CurrentStatus);
        }
    }
}
