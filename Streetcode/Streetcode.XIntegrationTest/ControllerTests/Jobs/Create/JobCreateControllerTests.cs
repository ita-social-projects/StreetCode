using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Xunit;
using Streetcode.DAL.Entities.Jobs;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Job;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Job;
using System.Net;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Job;
using Streetcode.BLL.DTO.Jobs;

namespace Streetcode.XIntegrationTest.ControllerTests.Jobs.Create
{
    [Collection("Authorization")]
    public class JobCreateControllerTests : BaseAuthorizationControllerTests<JobClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private Job _testJob;

        public JobCreateControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
           : base(factory, "/api/Job", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this._testJob = JobExtracter
                .Extract(uniqueId);
        }

        public override void Dispose()
        {
            JobExtracter.Remove(this._testJob);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_ReturnsSuccessStatusCode()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJob.JobCreateForTest;

            // Act
            var response = await this.client.CreateAsync(jobCreateDTO, this._tokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJob.JobCreateForTest;

            // Act
            var response = await this.client.CreateAsync(jobCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJob.JobCreateForTest;

            // Act
            var response = await this.client.CreateAsync(jobCreateDTO, this._tokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_CreatesNewJob()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJob.JobCreateForTest;

            // Act
            var response = await this.client.CreateAsync(jobCreateDTO, this._tokenStorage.AdminAccessToken);
            var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<JobDto>(response.Content);

            // Assert
            Assert.Equal(jobCreateDTO.Title, fetchedStreetcode.Title);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJob.JobCreateForTest;
            jobCreateDTO.Title = null;  // Invalid data

            // Act
            var response = await this.client.CreateAsync(jobCreateDTO, this._tokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestJob]
        public async Task Create_WithExistingJob_ReturnsConflict()
        {
            // Arrange
            var jobCreateDTO = ExtractCreateTestJob.JobForTest;
            jobCreateDTO.Id = this._testJob.Id;

            // Act
            var response = await this.client.CreateAsync(jobCreateDTO, this._tokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
