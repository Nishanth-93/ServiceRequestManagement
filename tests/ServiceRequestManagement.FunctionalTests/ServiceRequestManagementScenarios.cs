using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace ServiceRequestManagement.FunctionalTests
{
    public class ServiceRequestManagementScenarios : ServiceRequestManagementScenariosBase
    {
        [Fact]
        public async Task Given_ServiceRequestsInDb_When_PerformingGetRequestOnAllServiceRequests_Then_ReturnsSuccess()
        {
            using (var serviceRequestMgmtServer = new ServiceRequestManagementScenariosBase().CreateServer())
            {
                var response = await serviceRequestMgmtServer
                    .CreateClient()
                    .GetAsync(Get.ServiceRequests);
                
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
