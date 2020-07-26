using MediatR;
using System;

namespace ServiceRequestManagement.API.Application.Commands
{
    /// <summary>
    /// DeleteServiceRequestByIdCommand class
    /// </summary>
    public class DeleteServiceRequestByIdCommand : IRequest<bool>
    {
        /// <summary>
        /// The Id of the expected service request to delete.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Constructor for the DeleteServiceRequestByIdCommand class.
        /// </summary>
        /// <param name="id">The Id of the expected service request to delete.</param>
        public DeleteServiceRequestByIdCommand(Guid id)
        {
            Id = id;
        }
    }
}
