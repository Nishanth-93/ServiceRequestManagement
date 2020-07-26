using ServiceRequestManagement.API.Application.DTOs;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;

namespace ServiceRequestManagement.API.Application.Extensions
{
    /// <summary>
    /// Extensions for DTOs
    /// </summary>
    public static class DTOExtensions
    {
        /// <summary>
        /// Extension method that extends ServiceRequest entities into ServiceRequestDTOs
        /// </summary>
        /// <param name="input">The ServiceRequest entity to extend to a ServiceRequestDTO</param>
        /// <returns>ServiceRequestDTO</returns>
        public static ServiceRequestDTO AsServiceRequestDTO(this ServiceRequest input)
        {
            return new ServiceRequestDTO
            {
                Id = input.Id,
                BuildingCode = input.GetBuildingCode,
                Description = input.GetDescription,
                CurrentStatus = input.CurrentStatus,
                CreatedBy = input.GetCreatedBy,
                CreatedDate = input.GetCreatedDate,
                LastModifiedBy = input.GetLastModifiedBy,
                LastModifiedDate = input.GetLastModifiedDate
            };
        }
    }
}
