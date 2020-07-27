using Microsoft.Extensions.Logging;
using Moq;
using ServiceRequestManagement.API.Application.Queries;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Application.Queries
{
    public class QueryServiceRequestByIdHandlerTests
    {
        private readonly Mock<IServiceRequestRepository> _serviceRequestRepository;
        private readonly Mock<ILogger<QueryServiceRequestByIdHandler>> _logger;

        public QueryServiceRequestByIdHandlerTests()
        {
            _serviceRequestRepository = new Mock<IServiceRequestRepository>();
            _logger = new Mock<ILogger<QueryServiceRequestByIdHandler>>();
        }

        [Fact]
        public async void Given_ServiceRequestByIdQuery_When_ExpectedServiceRequestExists_Then_ReturnsExpectedServiceRequest()
        {
            // Arrange
            var expectedServiceRequest = new ServiceRequest("123", "Turn up the heat!", "Aaron");

            _serviceRequestRepository
                .Setup(repo => repo.RetrieveByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedServiceRequest);

            var query = new QueryServiceRequestById(new Guid());

            var target = new QueryServiceRequestByIdHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act
            var actual = await target.Handle(query, default);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedServiceRequest, actual);
        }

        [Fact]
        public async void Given_ServiceRequestByIdQuery_When_ExpectedServiceRequestDoesNotExists_Then_ReturnsNull()
        {
            // Arrange
            _serviceRequestRepository
                .Setup(repo => repo.RetrieveByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ServiceRequest)null);

            var query = new QueryServiceRequestById(new Guid());

            var target = new QueryServiceRequestByIdHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act
            var actual = await target.Handle(query, default);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Given_NullLogger_When_ConstructingQueryServiceRequestByIdHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new QueryServiceRequestByIdHandler(null, _serviceRequestRepository.Object));
        }

        [Fact]
        public void Given_NullRepo_When_ConstructingQueryServiceRequestByIdHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new QueryServiceRequestByIdHandler(_logger.Object, null));
        }
    }
}
