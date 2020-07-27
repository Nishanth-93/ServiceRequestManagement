using Microsoft.Extensions.Logging;
using Moq;
using ServiceRequestManagement.API.Application.Queries;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using System.Collections.Generic;
using Xunit;

namespace ServiceRequestManagement.UnitTests.Application.Queries
{
    public class QueryAllServiceRequestsHandlerTests
    {
        private readonly Mock<IServiceRequestRepository> _serviceRequestRepository;
        private readonly Mock<ILogger<QueryAllServiceRequestsHandler>> _logger;

        public QueryAllServiceRequestsHandlerTests()
        {
            _serviceRequestRepository = new Mock<IServiceRequestRepository>();
            _logger = new Mock<ILogger<QueryAllServiceRequestsHandler>>();
        }

        [Fact]
        public async void Given_RequestToQueryAllServiceRequests_When_RepositoryIsNotEmpty_Then_ReturnsListOfServiceRequests()
        {
            // Arrange
            var expectedServiceRequest1 = new ServiceRequest("123", "Turn up the heat!", "Aaron");
            var expectedServiceRequest2 = new ServiceRequest("456", "It's too hot!", "Bobbi");

            var serviceRequests = new List<ServiceRequest>
            {
                expectedServiceRequest1,
                expectedServiceRequest2
            };
            
            _serviceRequestRepository
                .Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(serviceRequests);

            var query = new QueryAllServiceRequests();

            var target = new QueryAllServiceRequestsHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act
            var actual = await target.Handle(query, default);

            // Assert
            Assert.NotEmpty(actual);
            Assert.Contains(expectedServiceRequest1, actual);
            Assert.Contains(expectedServiceRequest2, actual);
        }

        [Fact]
        public async void Given_RequestToQueryAllServiceRequests_When_RepositoryIsEmpty_Then_ReturnsEmptyList()
        {
            // Arrange
            _serviceRequestRepository
                .Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(new List<ServiceRequest>());

            var query = new QueryAllServiceRequests();

            var target = new QueryAllServiceRequestsHandler(_logger.Object, _serviceRequestRepository.Object);

            // Act
            var actual = await target.Handle(query, default);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void Given_NullLogger_When_ConstructingQueryAllServiceRequestsHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new QueryAllServiceRequestsHandler(null, _serviceRequestRepository.Object));
        }

        [Fact]
        public void Given_NullRepo_When_ConstructingQueryAllServiceRequestsHandler_Then_ThrowsArgumentNullException()
        {   // Arrange/ Act/ Assert
            Assert.Throws<ArgumentNullException>(() => new QueryAllServiceRequestsHandler(_logger.Object, null));
        }
    }
}
