﻿using Microsoft.Extensions.Logging;
using Moq;
using ServiceRequestManagement.API.Application.Commands;
using ServiceRequestManagement.API.Application.DTOs;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Application.Commands
{
    public class CreateServiceRequestCommandHanderTests
    {
        private readonly Mock<IServiceRequestRepository> _serviceRequestRepository;
        private readonly Mock<ILogger<CreateServiceRequestCommandHandler>> _logger;

        public CreateServiceRequestCommandHanderTests()
        {
            _serviceRequestRepository = new Mock<IServiceRequestRepository>();
            _logger = new Mock<ILogger<CreateServiceRequestCommandHandler>>();
        }

        [Fact]
        public async void Given_CreateServiceCommand_When_HandlingCreateServiceCommand_Then_ReturnsServiceRequestEntity()
        {
            // Arrange
            _serviceRequestRepository
               .Setup(serviceRequestRepo => serviceRequestRepo.Create(It.IsAny<ServiceRequest>()))
               .Returns<ServiceRequest>(serviceRequest => serviceRequest);

            var requestDTO = new PostCreateServiceRequestDTO
            {
                BuildingCode = "123",
                CreatedBy = "Aaron Jaeger",
                Description = "Turn up the heat!"
            };

            var createServiceRequestCommand = new CreateServiceRequestCommand(requestDTO);

            var target = new CreateServiceRequestCommandHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act
            var actual = await target.Handle(createServiceRequestCommand, default);

            // Assert
            Assert.Equal(createServiceRequestCommand.RequestBody.BuildingCode, actual.GetBuildingCode);
            Assert.Equal(createServiceRequestCommand.RequestBody.CreatedBy, actual.GetCreatedBy);
            Assert.Equal(createServiceRequestCommand.RequestBody.Description, actual.GetDescription);
            Assert.Equal(CurrentStatus.Created, actual.CurrentStatus);
            Assert.InRange(actual.GetCreatedDate, DateTime.UtcNow.AddSeconds(-30), DateTime.UtcNow);
            Assert.Null(actual.GetLastModifiedBy);
        }

        [Fact]
        public void Given_NullLogger_When_ConstructingCreateServiceCommandHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new CreateServiceRequestCommandHandler(null, _serviceRequestRepository.Object));
        }

        [Fact]
        public void Given_NullRepo_When_ConstructingCreateServiceCommandHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new CreateServiceRequestCommandHandler(_logger.Object, null));
        }
    }
}
