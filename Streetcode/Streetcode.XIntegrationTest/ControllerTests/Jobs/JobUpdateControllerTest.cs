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

namespace Streetcode.XIntegrationTest.ControllerTests.Jobs.Get
{

    [Collection("Authorization")]
    public class JobUpdateControllerTest : BaseAuthorizationControllerTests<JobClient>,
        IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Job testJob;

        public JobUpdateControllerTest(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Job", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testJob = JobExtracter
                .Extract(uniqueId);
        }

        [Fact]
        [ExtractUpdateJobAttribute] 
        public async Task Update_ReturnsSuccessStatusCode()
        {
            // Arrange
            var jobUpdateDto = ExtractUpdateJobAttribute.JobForTest; 

            // Act
            var response = await this.Client.UpdateAsync(jobUpdateDto, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); 
        }
        
        [Fact]
        [ExtractUpdateJobAttribute]
        public async Task Update_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;
            var jobUpdateDto = ExtractUpdateJobAttribute.JobForTest;
            jobUpdateDto.Id = id;

            // Act
            var response = await this.Client.UpdateAsync(jobUpdateDto, this.TokenStorage.AdminAccessToken);
            
            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }
        
        [Fact]
        [ExtractUpdateJobAttribute]
        public async Task Update_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var jobUpdateDto =  ExtractUpdateJobAttribute.JobForTest;

            // Act
            var response = await this.Client.UpdateAsync(jobUpdateDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        [ExtractUpdateJobAttribute]
        public async Task Update_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var jobUpdateDto =  ExtractUpdateJobAttribute.JobForTest;

            // Act
            var response = await this.Client.UpdateAsync(jobUpdateDto, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
        
        [Fact]
        [ExtractUpdateJobAttribute] 
        public async Task UpdateJobStatus_ReturnsSuccessStatusCode()
        {
            // Arrange
            var jobChangeStatusDto = new JobChangeStatusDto
            {
                Id = ExtractUpdateJobAttribute.JobForTest.Id,  
                Status = true 
            };
            
            // Act
            var response = await this.Client.ChangeJobStatusAsync(jobChangeStatusDto, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        [ExtractUpdateJobAttribute]
        public async Task UpdateJobStatus_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var jobChangeStatusDto = new JobChangeStatusDto
            {
                Id = ExtractUpdateJobAttribute.JobForTest.Id,  
                Status = true  
            };

            // Act
            var response = await this.Client.ChangeJobStatusAsync(jobChangeStatusDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        [ExtractUpdateJobAttribute]
        public async Task UpdateJobStatus_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var jobChangeStatusDto = new JobChangeStatusDto
            {
                Id = ExtractUpdateJobAttribute.JobForTest.Id, 
                Status = true 
            };

            // Act
            var response = await this.Client.ChangeJobStatusAsync(jobChangeStatusDto, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                JobExtracter.Remove(this.testJob);
            }

            base.Dispose(disposing);
        }
    }
}