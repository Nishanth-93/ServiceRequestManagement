using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ServiceRequestManagement.API.Application.DTOs
{
    /// <summary>
    /// The data transfer object for the POST request to create a single service request.
    /// </summary>
    [DataContract]
    public class PostCreateServiceRequestDTO
    {
        /// <summary>
        /// The building code of the ServiceRequest.
        /// </summary>
        [DataMember]
        [Required]
        [MaxLength(32)]

        public string BuildingCode { get; set; }

        /// <summary>
        /// The individual who created the ServiceRequest.
        /// </summary>
        [DataMember]
        [Required]
        [MaxLength(32)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// The description of the ServiceRequest.
        /// </summary>
        [DataMember]
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }
    }
}
