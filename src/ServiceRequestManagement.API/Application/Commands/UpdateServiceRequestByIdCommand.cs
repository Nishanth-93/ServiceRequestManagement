using MediatR;
using ServiceRequestManagement.API.Application.DTOs;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;

namespace ServiceRequestManagement.API.Application.Commands
{
    /// <summary>
    /// The command to update a single expected ServiceRequest entity by its id.
    /// </summary>
    public class UpdateServiceRequestByIdCommand : IRequest<ServiceRequest>
    {
        /// <summary>
        /// The Id of the expected ServiceRequest to update.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The request body of the expected ServiceRequest to update.
        /// </summary>
        public PutUpdateServiceRequestDTO RequestBody { get; private set; }

        /// <summary>
        /// THe constructor of the UpdateServiceRequestByIdCommand class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestBody"></param>
        public UpdateServiceRequestByIdCommand(Guid id, PutUpdateServiceRequestDTO requestBody)
        {
            Id = id;
            RequestBody = requestBody;
        }
    }
}
