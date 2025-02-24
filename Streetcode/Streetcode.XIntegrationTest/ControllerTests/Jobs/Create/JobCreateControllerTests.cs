using System.Net;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Job;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Job;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Job;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Jobs.Create
{
    [Collection("Authorization")]
    public class JobCreateControllerTests : BaseAuthorizationControllerTests<JobClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Job testJob;

        public JobCreateControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
           : base(factory, "/api/Job", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testJob = JobExtracter
                .Extract(uniqueId);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_ReturnsSuccessStatusCode()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJobAttribute.JobCreateForTest;

            // Act
            var response = await this.Client.CreateAsync(jobCreateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJobAttribute.JobCreateForTest;

            // Act
            var response = await this.Client.CreateAsync(jobCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJobAttribute.JobCreateForTest;

            // Act
            var response = await this.Client.CreateAsync(jobCreateDTO, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_CreatesNewJob()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJobAttribute.JobCreateForTest;

            // Act
            var response = await this.Client.CreateAsync(jobCreateDTO, this.TokenStorage.AdminAccessToken);
            var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<JobDto>(response.Content);

            // Assert
            Assert.Equal(jobCreateDTO.Title, fetchedStreetcode?.Title);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJobAttribute.JobCreateForTest;
            jobCreateDTO.Title = null!;  // Invalid data

            // Act
            var response = await this.Client.CreateAsync(jobCreateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

       
    }
}
