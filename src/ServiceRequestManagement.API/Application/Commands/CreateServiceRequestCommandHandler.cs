using MediatR;
using Microsoft.Extensions.Logging;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRequestManagement.API.Application.Commands
{
    /// <summary>
    /// The handler for the CreateServiceRequestCommand.
    /// </summary>
    public class CreateServiceRequestCommandHandler : IRequestHandler<CreateServiceRequestCommand, ServiceRequest>
    {
        private readonly ILogger<CreateServiceRequestCommandHandler> _logger;
        private readonly IServiceRequestRepository _serviceRequestRepository;

        /// <summary>
        /// The constructor for the CreateServiceRequestCommandHandler.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceRequestRepository"></param>
        public CreateServiceRequestCommandHandler(ILogger<CreateServiceRequestCommandHandler> logger,
            IServiceRequestRepository serviceRequestRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceRequestRepository = serviceRequestRepository ?? throw new ArgumentNullException(nameof(serviceRequestRepository));
        }

        /// <summary>
        /// Handles the CreateServiceRequestCommand. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The created ServiceRequest entity</returns>
        public Task<ServiceRequest> Handle(CreateServiceRequestCommand request, CancellationToken cancellationToken)
        {
            var serviceRequest = new ServiceRequest(request.RequestBody.BuildingCode, request.RequestBody.Description, request.RequestBody.CreatedBy);

            _logger.LogInformation("Creating Service Request - ServiceRequest: {@ServiceRequest}", serviceRequest);

            var response = _serviceRequestRepository.Create(serviceRequest);

            _logger.LogInformation("Created Service Request - ServiceRequest: {@ServiceRequest}", response);

            return Task.FromResult(response);
        }
    }
}
