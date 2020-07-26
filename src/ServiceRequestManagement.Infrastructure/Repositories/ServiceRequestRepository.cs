using ServiceRequestManagement.Domain.Seeds;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;

namespace ServiceRequestManagement.Infrastructure.Repositories
{
    /// <summary>
    /// The repository implementation for the ServiceRequest entity.
    /// </summary>
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly ServiceRequestManagementContext _context;

        /// <summary>
        /// The context that our repository will perform transaction against.
        /// </summary>
        public IUnitOfWork UnitOfWork { get { return _context; } }

        /// <summary>
        /// The constructor for our ServiceRequestRepository.
        /// </summary>
        /// <param name="context"></param>
        public ServiceRequestRepository(ServiceRequestManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Creates a ServiceRequest entity.
        /// </summary>
        /// <param name="serviceRequest"></param>
        /// <returns>Created ServiceRequest entity.</returns>
        public ServiceRequest Create(ServiceRequest serviceRequest)
        {
            return _context
                .ServiceRequests
                .Add(serviceRequest)
                .Entity;
        }
    }
}
