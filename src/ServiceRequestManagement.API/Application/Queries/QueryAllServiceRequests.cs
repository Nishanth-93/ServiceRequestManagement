using MediatR;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System.Collections.Generic;

namespace ServiceRequestManagement.API.Application.Queries
{
    /// <summary>
    /// The query to retrieve all ServiceRequest entities.
    /// </summary>
    public class QueryAllServiceRequests : IRequest<IEnumerable<ServiceRequest>>
    {
    }
}
