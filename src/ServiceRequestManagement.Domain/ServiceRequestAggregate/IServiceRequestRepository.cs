﻿using ServiceRequestManagement.Domain.Seeds;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceRequestManagement.Domain.ServiceRequestAggregate
{
    /// <summary>
    /// Interface for the ServiceRequest entity repository.
    /// </summary>
    public interface IServiceRequestRepository : IRepository<ServiceRequest>
    {
        ServiceRequest Create(ServiceRequest serviceRequest);
        Task<IEnumerable<ServiceRequest>> RetrieveAllAsync();
        Task<ServiceRequest> RetrieveByIdAsync(Guid id);
        void Update(ServiceRequest serviceRequest);
        void Delete(ServiceRequest serviceRequest);
    }
}
