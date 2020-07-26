using ServiceRequestManagement.Domain.Seeds;

namespace ServiceRequestManagement.Domain.ServiceRequestAggregate
{
    /// <summary>
    /// Interface for the ServiceRequest entity repository.
    /// </summary>
    public interface IServiceRequestRepository : IRepository<ServiceRequest>
    {
        ServiceRequest Create(ServiceRequest serviceRequest);
    }
}
