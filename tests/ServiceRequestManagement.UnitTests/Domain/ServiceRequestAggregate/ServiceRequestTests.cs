using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Domain.ServiceRequestAggregate
{
    public class ServiceRequestTests
    {
        [Fact]
        public void Given_RequiredParameters_When_Constructing_ServiceRequest_Then_FieldsSetCorrectly()
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

        [Fact]
        public void Given_NonEmptyBuildingCode_When_UpdatingBuildingCode_Then_UpdatesBuildingCode()
        {
            // Arrange
            var target = new ServiceRequest("abc", "Turn up the heat!", "Aaron");
            var expectedBuildingCode = "def";

            // Act
            target.SetBuildingCode(expectedBuildingCode);

            // Assert
            Assert.Equal(expectedBuildingCode, target.GetBuildingCode);
        }

        [Theory()]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void Given_NullOrEmptyBuildingCode_When_UpdatingBuildingCode_Then_DoesNotUpdateBuildingCode(string buildingCode)
        {
            // Arrange
            var expectedBuildingCode = "abc";
            var target = new ServiceRequest(expectedBuildingCode, "Turn up the heat!", "Aaron");

            // Act
            target.SetBuildingCode(buildingCode);
            
            // Assert
            Assert.Equal(expectedBuildingCode, target.GetBuildingCode);
        }

        [Fact]
        public void Given_NonEmptyDescription_When_UpdatingDescription_Then_UpdatesDescription()
        {
            // Arrange
            var target = new ServiceRequest("abc", "Turn up the heat!", "Aaron");
            var expectedDescription = "Now it's too hot!";

            // Act
            target.SetDescription(expectedDescription);

            // Assert
            Assert.Equal(expectedDescription, target.GetDescription);
        }

        [Theory()]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void Given_NullOrEmptyDescription_When_UpdatingDescription_Then_DoesNotUpdateDescription(string description)
        {
            // Arrange
            var expectedDescription = "Turn up the heat!";
            var target = new ServiceRequest("abc", expectedDescription, "Aaron");

            // Act
            target.SetDescription(description);

            // Assert
            Assert.NotNull(target.GetDescription);
            Assert.Equal(expectedDescription, target.GetDescription);
        }

        [Theory()]
        [InlineData(CurrentStatus.Canceled)]
        [InlineData(CurrentStatus.Complete)]
        [InlineData(CurrentStatus.Created)]
        [InlineData(CurrentStatus.InProgress)]
        [InlineData(CurrentStatus.NotApplicable)]
        public void Given_ValidCurrentStatus_When_UpdatingCurrentStatus_Then_UpdatesCurrentStatus(CurrentStatus expectedCurrentStatus)
        {
            // Arrange
            var target = new ServiceRequest("abc", "Turn up the heat!", "Aaron");

            // Act
            target.SetCurrentStatus(expectedCurrentStatus);

            // Assert
            Assert.Equal(expectedCurrentStatus, target.CurrentStatus);
        }

        [Theory()]
        [InlineData((CurrentStatus)45)]
        [InlineData((CurrentStatus)(-1))]
        public void Given_InValidCurrentStatus_When_UpdatingCurrentStatus_Then_ThrowArgumentException(CurrentStatus expectedCurrentStatus)
        {
            // Arrange
            var target = new ServiceRequest("abc", "Turn up the heat!", "Aaron");

            // Act/ Assert
            Assert.Throws<ArgumentException>(() => target.SetCurrentStatus(expectedCurrentStatus));
        }

        [Fact]
        public void Given_NonNullOrEmptyModifiedBy_When_SettingLastModifiedBy_Then_UpdatesLastModifiedBy()
        {
            // Arrange
            var target = new ServiceRequest("abc", "Turn up the heat!", "Aaron");
            var expectedLastModifiedBy = "Aaron Jaeger";

            // Act
            target.SetLastModifiedBy(expectedLastModifiedBy);

            // Assert
            Assert.Equal(expectedLastModifiedBy, target.GetLastModifiedBy);
        }

        [Theory()]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void Given_NullOrEmptyModifiedBy_When_UpdatingLastModifiedBy_Then_ThrowsArgumentException(string modifiedBy)
        {
            // Arrange
            var target = new ServiceRequest("abc", "Turn up the heat!", "Aaron");

            // Act/ Assert
            Assert.Throws<ArgumentException>(() => target.SetLastModifiedBy(modifiedBy));
        }

        [Fact]
        public void Given_ModifiedDate_When_UpdatingLastModifiedDate_Then_UpdatesLastModifiedDate()
        {
            // Arrange
            var target = new ServiceRequest("abc", "Turn up the heat!", "Aaron");
            var expectedLastModifiedDate = DateTime.UtcNow;

            // Act
            target.SetLastModifiedDate(expectedLastModifiedDate);

            // Assert
            Assert.Equal(expectedLastModifiedDate, target.GetLastModifiedDate);
        }
    }
}
