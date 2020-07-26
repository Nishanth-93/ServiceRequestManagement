using MediatR;
using Microsoft.Extensions.Logging;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRequestManagement.API.Application.Commands
{
    /// <summary>
    /// The handler for the DeleteServiceRequestByIdCommand.
    /// </summary>
    public class DeleteServiceRequestByIdCommandHandler : IRequestHandler<DeleteServiceRequestByIdCommand, bool>
    {
        private readonly ILogger<DeleteServiceRequestByIdCommandHandler> _logger;
        private readonly IServiceRequestRepository _serviceRequestRepository;

        /// <summary>
        /// The constructor for the DeleteServiceRequestByIdCommandHandler class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceRequestRepository"></param>
        public DeleteServiceRequestByIdCommandHandler(ILogger<DeleteServiceRequestByIdCommandHandler> logger,
            IServiceRequestRepository serviceRequestRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceRequestRepository = serviceRequestRepository ?? throw new ArgumentNullException(nameof(serviceRequestRepository));
        }

        /// <summary>
        /// Handles the DeleteServiceRequestByIdCommand. 
        /// </summary>
        /// <param name="request">The DeleteServiceRequestByIdCommand being handled.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The created ServiceRequest entity</returns>
        public async Task<bool> Handle(DeleteServiceRequestByIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting Service Requests By Id {Id} - Request: {@Request}", request.Id, request);

            var serviceRequestToDelete = await _serviceRequestRepository.RetrieveByIdAsync(request.Id);

            if (serviceRequestToDelete is null)
            {
                _logger.LogInformation("Service Request With Id {Id} Not Found - ServiceRequests: {@ServiceRequests}", request.Id, false);

                return false;
            }

            _serviceRequestRepository.Delete(serviceRequestToDelete);

            _logger.LogInformation("Service Request With Id {Id} Deleted - ServiceRequests: {@ServiceRequests}", request.Id, true);

            return true;
        }
    }
}
