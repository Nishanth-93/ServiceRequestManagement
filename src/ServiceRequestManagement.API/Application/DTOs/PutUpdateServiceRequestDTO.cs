using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ServiceRequestManagement.API.Application.DTOs
{
    /// <summary>
    /// The data transfer object for the PUT request to update a single service request.
    /// </summary>

    [DataContract]
    public class PutUpdateServiceRequestDTO
    {
        /// <summary>
        /// The individual making the modification to the service request.
        /// </summary>
        [DataMember]
        [Required]
        [MaxLength(32)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// The new building code to be updated on the service request.
        /// </summary>
        [DataMember]
        [MaxLength(32)]
        public string BuildingCode { get; set; }


        /// <summary>
        /// The new description to be updated on the service request.
        /// </summary>
        [DataMember]
        [MaxLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// The new current status to be updated on the service request.
        /// </summary>
        [DataMember]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CurrentStatus CurrentStatus { get; set; }
    }
}
