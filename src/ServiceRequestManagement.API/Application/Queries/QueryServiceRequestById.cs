using MediatR;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;

namespace ServiceRequestManagement.API.Application.Queries
{
    /// <summary>
    /// Qeury request for a single ServiceRequest entity by Id.
    /// </summary>
    public class QueryServiceRequestById : IRequest<ServiceRequest>
    {
        /// <summary>
        /// The Id of the expected ServiceRequest.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Constructor for the QueryServiceRequestById class.
        /// </summary>
        /// <param name="id"></param>
        public QueryServiceRequestById(Guid id)
        {
            Id = id;
        }
    }
}
