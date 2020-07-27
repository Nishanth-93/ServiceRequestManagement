using Microsoft.Extensions.Logging;
using Moq;
using ServiceRequestManagement.API.Application.Commands;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Application.Commands
{
    public class DeleteServiceRequestByIdCommandHandlerTests
    {
        private readonly Mock<IServiceRequestRepository> _serviceRequestRepository;
        private readonly Mock<ILogger<DeleteServiceRequestByIdCommandHandler>> _logger;

        public DeleteServiceRequestByIdCommandHandlerTests()
        {
            _serviceRequestRepository = new Mock<IServiceRequestRepository>();
            _logger = new Mock<ILogger<DeleteServiceRequestByIdCommandHandler>>();
        }

        [Fact]
        public async void Given_DeleteServiceRequestByIdCommand_With_ServiceRequestId_When_HandlingDeleteServiceRequestByIdCommand_And_ServiceRequestExists_Then_ReturnsTrue()
        {
            // Arrange
            var serviceRequestToDelete = new ServiceRequest("123", "Turn up the heat!", "Aaron");

            var deleteServiceRequestCommand = new DeleteServiceRequestByIdCommand(Guid.Empty);

            _serviceRequestRepository
                .Setup(repo => repo.RetrieveByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(serviceRequestToDelete);

            var target = new DeleteServiceRequestByIdCommandHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act
            var actual = await target.Handle(deleteServiceRequestCommand, default);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async void Given_DeleteServiceRequestByIdCommand_With_ServiceRequestId_When_HandlingDeleteServiceRequestByIdCommand_And_ServiceRequestDoesNotExist_Then_ReturnsFalse()
        {
            // Arrange
            var deleteServiceRequestCommand = new DeleteServiceRequestByIdCommand(Guid.Empty);

            _serviceRequestRepository
                .Setup(repo => repo.RetrieveByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ServiceRequest)null);

            var target = new DeleteServiceRequestByIdCommandHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act
            var actual = await target.Handle(deleteServiceRequestCommand, default);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Given_NullLogger_When_ConstructingDeleteServiceRequestByIdCommandHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new DeleteServiceRequestByIdCommandHandler(null, _serviceRequestRepository.Object));
        }

        [Fact]
        public void Given_NullRepo_When_ConstructingDeleteServiceRequestByIdCommandHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new DeleteServiceRequestByIdCommandHandler(_logger.Object, null));
        }
    }
}
