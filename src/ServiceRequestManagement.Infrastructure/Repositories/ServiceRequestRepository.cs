using Microsoft.EntityFrameworkCore;
using ServiceRequestManagement.Domain.Seeds;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceRequestManagement.Infrastructure.Repositories
{
    /// <summary>
    /// The repository implementation for the ServiceRequest entity.
    /// </summary>
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly ServiceRequestManagementContext _context;

        /// <summary>
        /// The DbContext.
        /// </summary>
        public IUnitOfWork UnitOfWork { get { return _context; } }

        /// <summary>
        /// The constructor for the ServiceRequestRepository.
        /// </summary>
        /// <param name="context"></param>
        public ServiceRequestRepository(ServiceRequestManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Creates a single service request entity.
        /// </summary>
        /// <param name="serviceRequest">The service request entity to persist.</param>
        /// <returns>The persisted service request entity.</returns>
        public ServiceRequest Create(ServiceRequest serviceRequest)
        {
            return _context
                .ServiceRequests
                .Add(serviceRequest)
                .Entity;
        }

        /// <summary>
        /// Retrieves all service request entities.
        /// </summary>
        /// <returns>The list of all service request entities.</returns>
        public async Task<IEnumerable<ServiceRequest>> RetrieveAllAsync()
        {
            return await _context
                .ServiceRequests
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single expected service request entity by the Id.
        /// </summary>
        /// <param name="id">The Id of the expected service request entity.</param>
        /// <returns>The expected service request entity if it exists; otherwise, null</returns>
        public async Task<ServiceRequest> RetrieveByIdAsync(Guid id)
        {
            return await _context
                .ServiceRequests
                .FirstOrDefaultAsync(serviceRequest => serviceRequest.Id == id);
        }

        /// <summary>
        /// Updates a single service request entity.
        /// </summary>
        /// <param name="serviceRequest">The service request entity to update in its new state.</param>
        public void Update(ServiceRequest serviceRequest)
        {
            _context.Entry(serviceRequest).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes a single service request entity.
        /// </summary>
        /// <param name="serviceRequest">The service request entity to delete.</param>
        public void Delete(ServiceRequest serviceRequest)
        {
            _context.ServiceRequests.Remove(serviceRequest);
        }
    }
}
