using ServiceRequestManagement.Domain.Seeds;
using System;

namespace ServiceRequestManagement.Domain.ServiceRequestAggregate
{
    /// <summary>
    /// The ServiceRequest class. This class will implement all the business logic behind service requests.
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

        /// <summary>
        /// Sets the building code on the service request.
        /// </summary>
        /// <remarks>Will not set the building code if the passed string is null or empty.</remarks>
        /// <param name="buildingCode">The new building code to update the service request with</param>
        public void SetBuildingCode(string buildingCode)
        {
            if (!string.IsNullOrWhiteSpace(buildingCode))
                _buildingCode = buildingCode;
        }

        /// <summary>
        /// Sets the description on the service request.
        /// </summary>
        /// <remarks>Will not set the description if the passed string is null or empty.</remarks>
        /// <param name="description">The new description to update the service request with</param>
        public void SetDescription(string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
                _description = description;
        }

        /// <summary>
        /// Sets the current status on the service request.
        /// </summary>
        /// <param name="currentStatus">The new status to update the service request with</param>
        /// <exception cref="ArgumentException"></exception>
        public void SetCurrentStatus(CurrentStatus currentStatus)
        {
            switch (currentStatus)
            {
                case CurrentStatus.Created:
                case CurrentStatus.Complete:
                case CurrentStatus.InProgress:
                case CurrentStatus.NotApplicable:
                case CurrentStatus.Canceled:
                    CurrentStatus = currentStatus;
                    break;
                default:
                    throw new ArgumentException("Valid values are Created, Complete, InProgess, NotApplicable, and Canceled.", nameof(currentStatus));
            }
        }

        /// <summary>
        /// Sets the individual whom last modified the service request.
        /// </summary>
        /// <param name="modifiedBy">The individual whom last modified the service request.</param>
        /// <exception cref="ArgumentException"></exception>
        public void SetLastModifiedBy(string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(modifiedBy))
                throw new ArgumentException("The value cannot be null or white space.", nameof(modifiedBy));
            
            _lastModifiedBy = modifiedBy;
        }

        /// <summary>
        /// Sets the date the service request was last modified.
        /// </summary>
        /// <param name="modifiedDate">The date the service request was last modified.</param>
        public void SetLastModifiedDate(DateTime modifiedDate)
        {
            _lastModifiedDate = modifiedDate;
        }
    }
}
