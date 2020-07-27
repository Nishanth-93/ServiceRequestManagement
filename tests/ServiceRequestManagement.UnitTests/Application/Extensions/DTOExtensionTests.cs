using ServiceRequestManagement.API.Application.Extensions;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Application.Extensions
{
    public class DTOExtensionTests
    {
        [Fact]
        public void Given_ServiceRequest_When_ExtendingAsServiceRequestDTO_Then_ReturnsServiceRequestDTO()
        {
            // Arrange
            var target = new ServiceRequest("123", "Turn up the heat!", "Aaron Jaeger");

            // Act
            var actual = target.AsServiceRequestDTO();

            // Assert
            Assert.Equal(target.Id, actual.Id);
            Assert.Equal(target.CurrentStatus, actual.CurrentStatus);
            Assert.Equal(target.GetBuildingCode, actual.BuildingCode);
            Assert.Equal(target.GetDescription, actual.Description);
            Assert.Equal(target.GetCreatedBy, actual.CreatedBy);
            Assert.Equal(target.GetCreatedDate, actual.CreatedDate);
            Assert.Equal(target.GetLastModifiedBy, actual.LastModifiedBy);
            Assert.Equal(target.GetLastModifiedDate, actual.LastModifiedDate);
        }
    }
}
