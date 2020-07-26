using MediatR;
using Microsoft.Extensions.Logging;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRequestManagement.API.Application.Queries
{
    /// <summary>
    /// The handler for the querying a single ServiceRequest entity.
    /// </summary>
    public class QueryServiceRequestByIdHandler : IRequestHandler<QueryServiceRequestById, ServiceRequest>
    {
        private readonly ILogger<QueryServiceRequestByIdHandler> _logger;
        private readonly IServiceRequestRepository _serviceRequestRepository;

        /// <summary>
        /// The constructor for the QueryServiceRequestsByIdHandler class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceRequestRepository"></param>
        public QueryServiceRequestByIdHandler(ILogger<QueryServiceRequestByIdHandler> logger,
            IServiceRequestRepository serviceRequestRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceRequestRepository = serviceRequestRepository ?? throw new ArgumentNullException(nameof(serviceRequestRepository));
        }

        /// <summary>
        /// Handles the request to query a single ServiceRequest entity by Id.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns expected ServiceRequest entity if exists; otherwise, null</returns>
        public async Task<ServiceRequest> Handle(QueryServiceRequestById request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Querying Service Requests By Id {Id} - Request: {@Request}", request.Id, request);

            var response = await _serviceRequestRepository.RetrieveByIdAsync(request.Id);

            _logger.LogInformation("Queried Request By Id {Id} - ServiceRequests: {@ServiceRequests}", request.Id, response);

            return response;
        }
    }
}
