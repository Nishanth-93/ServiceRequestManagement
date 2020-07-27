using Microsoft.Extensions.Logging;
using Moq;
using ServiceRequestManagement.API.Application.Commands;
using ServiceRequestManagement.API.Application.DTOs;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Application.Commands
{
    public class UpdateServiceRequestByIdCommandHandlerTests
    {
        private readonly Mock<IServiceRequestRepository> _serviceRequestRepository;
        private readonly Mock<ILogger<UpdateServiceRequestByIdCommandHandler>> _logger;

        public UpdateServiceRequestByIdCommandHandlerTests()
        {
            _serviceRequestRepository = new Mock<IServiceRequestRepository>();
            _logger = new Mock<ILogger<UpdateServiceRequestByIdCommandHandler>>();
        }

        [Fact]
        public async void Given_UpdateServiceRequestByIdCommand_And_ServiceRequestId_When_HandlingUpdateServiceRequestByIdCommand_AndServiceRequestExists_Then_ReturnsUpdatedServiceRequestEntity()
        {
            // Arrange
            var expectedBuildingCode = "456";
            var expectedLastModifiedBy = "Aaron Jaeger";
            var expectedCurrentStatus = CurrentStatus.Created;
            var expectedDescription = "Now it's too hot!";

            var updateDTO = new PutUpdateServiceRequestDTO
            {
                BuildingCode = expectedBuildingCode,
                ModifiedBy = expectedLastModifiedBy,
                CurrentStatus = expectedCurrentStatus,
                Description = expectedDescription
            };

            var expectedCreatedBy = "Aaron";

            var serviceRequestToUpdate = new ServiceRequest("123", "Turn up the heat!", expectedCreatedBy);

            var updateServiceRequestCommand = new UpdateServiceRequestByIdCommand(Guid.Empty, updateDTO);

            _serviceRequestRepository
                .Setup(repo => repo.RetrieveByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(serviceRequestToUpdate);

            var target = new UpdateServiceRequestByIdCommandHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act
            var actual = await target.Handle(updateServiceRequestCommand, default);

            // Assert
            Assert.Equal(expectedBuildingCode, actual.GetBuildingCode);
            Assert.Equal(expectedDescription, actual.GetDescription);
            Assert.Equal(expectedCurrentStatus, actual.CurrentStatus);
            Assert.Equal(expectedLastModifiedBy, actual.GetLastModifiedBy);
            Assert.Equal(expectedCreatedBy, actual.GetCreatedBy);
            Assert.InRange((DateTime)actual.GetLastModifiedDate, DateTime.UtcNow.AddSeconds(-30), DateTime.UtcNow);
        }

        [Fact]
        public async void Given_UpdateServiceRequestByIdCommand_And_ServiceRequestId_When_HandlingUpdateServiceRequestByIdCommand_AndServiceRequestDoesNotExist_Then_ReturnsNull()
        {
            // Arrange
            var updateDTO = new PutUpdateServiceRequestDTO
            {
                BuildingCode = "456",
                ModifiedBy = "Aaron Jaeger",
                CurrentStatus = CurrentStatus.Created,
                Description = "Now it's too hot!"
            };

            var updateServiceRequestCommand = new UpdateServiceRequestByIdCommand(Guid.Empty, updateDTO);

            _serviceRequestRepository
                .Setup(repo => repo.RetrieveByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ServiceRequest)null);

            var target = new UpdateServiceRequestByIdCommandHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act
            var actual = await target.Handle(updateServiceRequestCommand, default);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async void Given_UpdateServiceRequestByIdCommand_And_ServiceRequestId_When_HandlingUpdateServiceRequestByIdCommand_With_InvalidData_AndServiceRequestExists_Then_ThrowsArgumentException()
        {
            // Arrange
            var updateDTO = new PutUpdateServiceRequestDTO
            {
                BuildingCode = "456",
                ModifiedBy = "       ",
                CurrentStatus = (CurrentStatus)43,
                Description = "Now it's too hot!"
            };

            var serviceRequestToUpdate = new ServiceRequest("123", "Turn up the heat!", "Aaron");

            var updateServiceRequestCommand = new UpdateServiceRequestByIdCommand(Guid.Empty, updateDTO);

            _serviceRequestRepository
                .Setup(repo => repo.RetrieveByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(serviceRequestToUpdate);

            var target = new UpdateServiceRequestByIdCommandHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act /Assert
            await Assert.ThrowsAsync<ArgumentException>(() => target.Handle(updateServiceRequestCommand, default));
        }

        [Fact]
        public void Given_NullLogger_When_ConstructingUpdateServiceRequestByIdCommandHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new UpdateServiceRequestByIdCommandHandler(null, _serviceRequestRepository.Object));
        }

        [Fact]
        public void Given_NullRepo_When_ConstructingUpdateServiceRequestByIdCommandHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new UpdateServiceRequestByIdCommandHandler(_logger.Object, null));
        }
    }
}
