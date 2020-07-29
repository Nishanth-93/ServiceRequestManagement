using Newtonsoft.Json;
using ServiceRequestManagement.API.Application.DTOs;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceRequestManagement.FunctionalTests
{
    public class ServiceRequestManagementScenarios : ServiceRequestManagementScenariosBase
    {
        [Fact]
        public async Task Given_ServiceRequestEndpoint_When_PerformingGetRequestOnAllServiceRequests_Then_ReturnsSuccess()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var response = await serviceRequestMgmtServer
                    .CreateClient()
                    .GetAsync(Get.ServiceRequests);
                
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Given_PostCreateServiceRequestDTO_When_PerformingPostRequestToCreateServiceRequest_Then_ReturnsSuccess()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var requestBody = BuildPostCreateServiceRequestDTO();
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await serviceRequestMgmtServer
                    .CreateClient()
                    .PostAsync(Post.ServiceRequest, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                var actual = JsonConvert.DeserializeObject<ServiceRequestDTO>(responseContent);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.NotEqual(default, actual.Id);
                Assert.Equal(requestBody.BuildingCode, actual.BuildingCode);
                Assert.Equal(requestBody.CreatedBy, actual.CreatedBy);
                Assert.Equal(requestBody.Description, actual.Description);
                Assert.InRange(actual.CreatedDate, DateTime.UtcNow.AddSeconds(-30), DateTime.UtcNow);
            }
        }

        [Fact]
        public async Task Given_PostCreateServiceRequestDTO_With_NullRequiredFields_When_PerformingPostRequestToCreateServiceRequest_Then_ReturnsBadRequest()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var requestBody = BuildPostCreateServiceRequestDTO(null, null, null);
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await serviceRequestMgmtServer
                    .CreateClient()
                    .PostAsync(Post.ServiceRequest, content);;

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task Given_PostCreateServiceRequestDTO_With_RequiredFieldsExceedingMaxLength_When_PerformingPostRequestToCreateServiceRequest_Then_ReturnsBadRequest()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var requestBody = BuildPostCreateServiceRequestDTO("Taumatawhakatangi­hangakoauauotamatea­turipukakapikimaunga­horonukupokaiwhen­uakitanatahu, New Zealand",
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    "RHOSHANDIATELLYNESHIAUNNEVESHENK KOYAANISQUATSIUTH WILLIAMS");
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await serviceRequestMgmtServer
                    .CreateClient()
                    .PostAsync(Post.ServiceRequest, content); ;

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task Given_IdOfServiceRequest_And_ServiceRequestExists_When_PerformingGetRequestOnServiceRequestById_Then_ReturnsOk()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var requestBody = BuildPostCreateServiceRequestDTO();
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var createResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PostAsync(Post.ServiceRequest, content);
                var responseContent = await createResponse.Content.ReadAsStringAsync();
                var createdServiceRequest = JsonConvert.DeserializeObject<ServiceRequestDTO>(responseContent);

                var getResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .GetAsync(Get.ServiceRequestById(createdServiceRequest.Id));


                Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Given_IdOfServiceRequest_And_ServiceRequestDoesNotExist_When_PerformingGetRequestOnServiceRequestById_Then_ReturnsNotFound()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var getResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .GetAsync(Get.ServiceRequestById(new Guid("11111111-1111-1111-1111-111111111111")));

                Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Given_IdOfServiceRequest_WithValidBody_And_ServiceRequestExists_When_PerformingPutRequestToUpdateServiceRequestById_Then_ReturnsOk()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var createRequestBody = BuildPostCreateServiceRequestDTO();
                var createContent = new StringContent(JsonConvert.SerializeObject(createRequestBody), Encoding.UTF8, "application/json");
                var createResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PostAsync(Post.ServiceRequest, createContent);
                var createResponseContent = await createResponse.Content.ReadAsStringAsync();
                var createdServiceRequest = JsonConvert.DeserializeObject<ServiceRequestDTO>(createResponseContent);

                var updateRequestBody = BuildPutUpdateServiceRequestDTO();
                var updateContent = new StringContent(JsonConvert.SerializeObject(updateRequestBody), Encoding.UTF8, "application/json");
                var updateResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PutAsync(Put.ServiceRequestById(createdServiceRequest.Id), updateContent);
                var updatedResponseContent = await updateResponse.Content.ReadAsStringAsync();
                var actual = JsonConvert.DeserializeObject<ServiceRequestDTO>(updatedResponseContent);

                Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
                Assert.Equal(updateRequestBody.BuildingCode, actual.BuildingCode);
                Assert.Equal(updateRequestBody.CurrentStatus, actual.CurrentStatus);
                Assert.Equal(updateRequestBody.Description, actual.Description);
                Assert.Equal(updateRequestBody.ModifiedBy, actual.LastModifiedBy);
                Assert.InRange((DateTime)actual.LastModifiedDate, DateTime.UtcNow.AddSeconds(-30), DateTime.UtcNow);
                Assert.Equal(createRequestBody.CreatedBy, actual.CreatedBy);
            }
        }

        [Fact]
        public async Task Given_IdOfServiceRequest_With_NullModifiedByInBody_When_PerformingPutRequestToUpdateServiceRequestById_Then_ReturnsBadRequest()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var updateRequestBody = BuildPutUpdateServiceRequestDTO(modifiedBy: null);
                var updateContent = new StringContent(JsonConvert.SerializeObject(updateRequestBody), Encoding.UTF8, "application/json");
                var updateResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PutAsync(Put.ServiceRequestById(new Guid("11111111-1111-1111-1111-111111111111")), updateContent);

                Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Given_IdOfServiceRequest_With_EmptyModifiedBy_And_ServiceRequestExists_When_PerformingPutRequestToUpdateServiceRequestById_Then_ReturnsBadRequest()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var createRequestBody = BuildPostCreateServiceRequestDTO();
                var createContent = new StringContent(JsonConvert.SerializeObject(createRequestBody), Encoding.UTF8, "application/json");
                var createResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PostAsync(Post.ServiceRequest, createContent);
                var createResponseContent = await createResponse.Content.ReadAsStringAsync();
                var createdServiceRequest = JsonConvert.DeserializeObject<ServiceRequestDTO>(createResponseContent);

                var updateRequestBody = BuildPutUpdateServiceRequestDTO(modifiedBy: "     ");
                var updateContent = new StringContent(JsonConvert.SerializeObject(updateRequestBody), Encoding.UTF8, "application/json");
                var updateResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PutAsync(Put.ServiceRequestById(createdServiceRequest.Id), updateContent);

                Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Given_IdOfServiceRequest_With_OutOfRangeCurrentStatusEnum_And_ServiceRequestExists_When_PerformingPutRequestToUpdateServiceRequestById_Then_ReturnsBadRequest()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var createRequestBody = BuildPostCreateServiceRequestDTO();
                var createContent = new StringContent(JsonConvert.SerializeObject(createRequestBody), Encoding.UTF8, "application/json");
                var createResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PostAsync(Post.ServiceRequest, createContent);
                var createResponseContent = await createResponse.Content.ReadAsStringAsync();
                var createdServiceRequest = JsonConvert.DeserializeObject<ServiceRequestDTO>(createResponseContent);

                var updateRequestBody = BuildPutUpdateServiceRequestDTO(currentStatus: (CurrentStatus)45);
                var updateContent = new StringContent(JsonConvert.SerializeObject(updateRequestBody), Encoding.UTF8, "application/json");
                var updateResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PutAsync(Put.ServiceRequestById(createdServiceRequest.Id), updateContent);

                Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Given_IdOfServiceRequest_And_ServiceRequestDoesNotExist_When_PerformingPutRequestToUpdateServiceRequestById_Then_ReturnsNotFound()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var updateRequestBody = BuildPutUpdateServiceRequestDTO();
                var updateContent = new StringContent(JsonConvert.SerializeObject(updateRequestBody), Encoding.UTF8, "application/json");
                var updateResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PutAsync(Put.ServiceRequestById(new Guid("11111111-1111-1111-1111-111111111111")), updateContent);

                Assert.Equal(HttpStatusCode.NotFound, updateResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Given_IdOfServiceRequest_And_ServiceRequestExists_When_PerformingDeleteRequestToDeleteServiceRequestById_Then_ReturnsNoContent()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var createRequestBody = BuildPostCreateServiceRequestDTO();
                var createContent = new StringContent(JsonConvert.SerializeObject(createRequestBody), Encoding.UTF8, "application/json");
                var createResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .PostAsync(Post.ServiceRequest, createContent);
                var createResponseContent = await createResponse.Content.ReadAsStringAsync();
                var createdServiceRequest = JsonConvert.DeserializeObject<ServiceRequestDTO>(createResponseContent);

                var deleteResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .DeleteAsync(Delete.ServiceRequestById(createdServiceRequest.Id));

                var getResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .GetAsync(Get.ServiceRequestById(createdServiceRequest.Id));

                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
                Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Given_IdOfServiceRequest_And_ServiceRequestDoesNotExists_When_PerformingDeleteRequestToDeleteServiceRequestById_Then_ReturnsNotFound()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var deleteResponse = await serviceRequestMgmtServer
                    .CreateClient()
                    .DeleteAsync(Delete.ServiceRequestById(new Guid("11111111-1111-1111-1111-111111111111")));

                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }

        /// <summary>
        /// Builds a PostCreateServiceRequestDTO
        /// </summary>
        /// <param name="buildingCode"></param>
        /// <param name="description"></param>
        /// <param name="createdBy"></param>
        /// <returns>Built PostCreateServiceRequestDTO</returns>
        private PostCreateServiceRequestDTO BuildPostCreateServiceRequestDTO(string buildingCode = "123", string description = "Turn up the heat", string createdBy = "Aaron Jaeger")
        {
            return new PostCreateServiceRequestDTO
            {
                BuildingCode = buildingCode,
                Description = description,
                CreatedBy = createdBy
            };
        }

        /// <summary>
        /// Builds a PutUpdateServiceRequestDTO
        /// </summary>
        /// <param name="buildingCode"></param>
        /// <param name="description"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="currentStatus"></param>
        /// <returns>Built PutUpdateServiceRequestDTO</returns>
        private PutUpdateServiceRequestDTO BuildPutUpdateServiceRequestDTO(string buildingCode = "456", string description = "Now it's too hot!", string modifiedBy = "Aaron M. Jaeger", CurrentStatus currentStatus = CurrentStatus.InProgress)
        {
            return new PutUpdateServiceRequestDTO
            {
                BuildingCode = buildingCode,
                ModifiedBy = modifiedBy,
                CurrentStatus = currentStatus,
                Description = description,
            };
        }
    }
}
