using MediatR;
using Microsoft.Extensions.Logging;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRequestManagement.API.Application.Queries
{
    /// <summary>
    /// The handler for the query to retrieve all ServiceRequest entities.
    /// </summary>
    public class QueryAllServiceRequestsHandler : IRequestHandler<QueryAllServiceRequests, IEnumerable<ServiceRequest>>
    {
        private readonly ILogger<QueryAllServiceRequestsHandler> _logger;
        private readonly IServiceRequestRepository _serviceRequestRepository;

        /// <summary>
        /// Constructor for the QueryAllServiceRequestHandler class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceRequestRepository"></param>
        public QueryAllServiceRequestsHandler(ILogger<QueryAllServiceRequestsHandler> logger,
            IServiceRequestRepository serviceRequestRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceRequestRepository = serviceRequestRepository ?? throw new ArgumentNullException(nameof(serviceRequestRepository));
        }

        /// <summary>
        /// Handles the request to query all ServiceRequest entities.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of all ServiceRequests</returns>
        public async Task<IEnumerable<ServiceRequest>> Handle(QueryAllServiceRequests request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Querying All Service Requests - Request: {@Request}", request);

            var response = await _serviceRequestRepository.RetrieveAllAsync();

            _logger.LogInformation("Queried All Service Requests - ServiceRequests: {@ServiceRequests}", response);

            return response;
        }
    }
}
