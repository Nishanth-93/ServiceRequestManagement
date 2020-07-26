using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using System.Runtime.Serialization;

namespace ServiceRequestManagement.API.Application.DTOs
{
    /// <summary>
    /// The ServiceRequest data transfer object implementation used as a response to ServiceRequest API request. This class decouples our response contracts from our ServiceRequest entity.
    /// </summary>
    [DataContract]
    public class ServiceRequestDTO
    {
        /// <summary>
        /// The Id of the ServiceRequest.
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }
        
        /// <summary>
        /// The building code of the ServiceRequest.
        /// </summary>
        [DataMember]
        public string BuildingCode { get; set; }
        
        /// <summary>
        /// The description of the ServiceRequest.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
        /// <summary>
        /// The current status of the ServiceRequest.
        /// </summary>
        [DataMember]
        public CurrentStatus CurrentStatus { get; set; }
        
        /// <summary>
        /// The individual who created the ServiceRequest.
        /// </summary>
        [DataMember]
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// The date the ServiceRequest was created.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// The last person to modify the ServiceRequest after creation.
        /// </summary>
        [DataMember]
        public string LastModifiedBy { get; set; }
        
        /// <summary>
        /// The last date the ServiceRequest was modifed after creation.
        /// </summary>
        [DataMember]
        public DateTime? LastModifiedDate { get; set; }
    }
}
