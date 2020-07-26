using MediatR;
using Microsoft.Extensions.Logging;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRequestManagement.API.Application.Commands
{
    public class UpdateServiceRequestByIdCommandHandler : IRequestHandler<UpdateServiceRequestByIdCommand, ServiceRequest>
    {
        private readonly ILogger<UpdateServiceRequestByIdCommandHandler> _logger;
        private readonly IServiceRequestRepository _serviceRequestRepository;

        public UpdateServiceRequestByIdCommandHandler(ILogger<UpdateServiceRequestByIdCommandHandler> logger,
            IServiceRequestRepository serviceRequestRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceRequestRepository = serviceRequestRepository ?? throw new ArgumentNullException(nameof(serviceRequestRepository));
        }

        public async Task<ServiceRequest> Handle(UpdateServiceRequestByIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating Service Requests By Id {Id} - Request: {@Request}", request.Id, request);

            var serviceRequest = await _serviceRequestRepository.RetrieveByIdAsync(request.Id);

            if (serviceRequest is null)
            {
                _logger.LogInformation("Service Request With Id {Id} Not Found - ServiceRequests: {@ServiceRequests}", request.Id, false);

                return null;
            }

            // Let the ServiceRequestAggregate handle the business logic behind updating the fields.
            serviceRequest.SetLastModifiedBy(request.RequestBody.ModifiedBy);
            serviceRequest.SetLastModifiedDate(DateTime.UtcNow);
            serviceRequest.SetBuildingCode(request.RequestBody.BuildingCode);
            serviceRequest.SetDescription(request.RequestBody.Description);
            serviceRequest.SetCurrentStatus(request.RequestBody.CurrentStatus);

            _serviceRequestRepository.Update(serviceRequest);

            _logger.LogInformation("Service Request With Id {Id} Deleted - ServiceRequests: {@ServiceRequests}", request.Id, true);

            return serviceRequest;
        }
    }
}
