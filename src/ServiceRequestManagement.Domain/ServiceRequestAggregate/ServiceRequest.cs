using ServiceRequestManagement.Domain.Seeds;
using System;

namespace ServiceRequestManagement.Domain.ServiceRequestAggregate
{
    /// <summary>
    /// The ServiceRequest entity. This class will implement all the business logic behind service requests.
    /// </summary>
    public class ServiceRequest : Entity, IAggregateRoot
    {
        private string _buildingCode;
        public string GetBuildingCode => _buildingCode;
        private string _description;
        public string GetDescription => _description;
        public CurrentStatus CurrentStatus { get; private set; }
        private string _createdBy;
        public string GetCreatedBy => _createdBy;
        private DateTime _createdDate;
        public DateTime GetCreatedDate => _createdDate;
        private string _lastModifiedBy;
        public string GetLastModifiedBy => _lastModifiedBy;
        private DateTime? _lastModifiedDate;
        public DateTime? GetLastModifiedDate => _lastModifiedDate;

        /// <summary>
        /// The constructor for the ServiceRequest entity. This will create a transient ServiceRequest entity.
        /// </summary>
        /// <param name="buildingCode"></param>
        /// <param name="description"></param>
        /// <param name="createdBy"></param>
        /// <param name="currentStatus"></param>
        public ServiceRequest(string buildingCode, string description, string createdBy, CurrentStatus currentStatus = CurrentStatus.Created)
        {
            _buildingCode = buildingCode;
            _description = description;
            CurrentStatus = currentStatus;
            _createdBy = createdBy;
            _createdDate = DateTime.UtcNow;
        }
    }
}
