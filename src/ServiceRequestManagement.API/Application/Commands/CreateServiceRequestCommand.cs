using MediatR;
using ServiceRequestManagement.API.Application.DTOs;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;

namespace ServiceRequestManagement.API.Application.Commands
{
    /// <summary>
    /// CreateServiceRequestCommand class.
    /// </summary>
    public class CreateServiceRequestCommand : IRequest<ServiceRequest>
    {
        /// <summary>
        /// The request body of the expected ServiceRequest to create.
        /// </summary>
        public PostCreateServiceRequestDTO RequestBody { get; private set; }

        /// <summary>
        /// The constructor for the CreateServiceRequestCommandClass.
        /// </summary>
        /// <param name="requestBody">The request body of the expected ServiceRequest to create.</param>
        public CreateServiceRequestCommand(PostCreateServiceRequestDTO requestBody)
        {
            RequestBody = requestBody;
        }
    }
}
