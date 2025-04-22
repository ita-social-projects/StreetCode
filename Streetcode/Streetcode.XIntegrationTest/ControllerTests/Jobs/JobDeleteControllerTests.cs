using System.Net;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Job;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Job;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Job;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Jobs.Delete
{
    [Collection("Authorization")]
    public class JobDeleteControllerTests : BaseAuthorizationControllerTests<JobClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Job testJob;

        public JobDeleteControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Job", tokenStorage)  
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testJob = JobExtracter
                .Extract(uniqueId);
        }

        [Fact]
        [ExtractDeleteTestJob]
        public async Task Delete_WithValidId_ReturnsSuccessStatusCode()
        {
            // Arrange
            var jobId = ExtractDeleteTestJobAttribute.JobForTest.Id;

            // Act
            var response = await this.Client.DeleteAsync(jobId, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);  
        }

		[Fact]
		[ExtractDeleteTestJob]
		public async Task Delete_WithNonExistingId_ShouldReturnNotFound()
		{
		// Arrange
            var jobId = -1000;

            // Act
            var response = await this.Client.DeleteAsync(jobId, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); 
		}
    }
}