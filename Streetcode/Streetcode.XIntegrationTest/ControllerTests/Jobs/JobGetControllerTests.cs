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
    [Collection("Job")]
    public class JobGetControllerTests : BaseControllerTests<JobClient>
    {
        private readonly Job testJob;

        public JobGetControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Job")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testJob = JobExtracter.Extract(uniqueId);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            // Arrange
            var response = await this.Client.GetAllAsync(1, 10);

            // Act
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllJobsDTO>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        public async Task GetAll_PageNumberTooBig_ReturnFailureStatusCode()
        {
            // Arrange
            var response = await this.Client.GetAllAsync((ushort?)ushort.MaxValue, 10);

            // Act
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllJobsDTO>(response.Content);

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Null(returnedValue);
        }

        [Fact]
        public async Task GetAll_PageSizeIsZero_ReturnsEmptyCollection()
        {
            var response = await this.Client.GetAllAsync(1, 0);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllJobsDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Empty(returnedValue.Jobs);
        }
        [Fact]
        public async Task GetAllShort_ReturnSuccessStatusCode()
        {
            // Arrange
            var response = await this.Client.GetAllShortAsync();

            // Act
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<JobShortDto>>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetActiveJobs_ReturnSuccessStatusCode()
        {
            // Arrange
            var response = await this.Client.GetActiveJobsAsync();

            // Act
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<JobDto>>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.NotEmpty(returnedValue);
        }

        [Fact]
      public async Task GetById_ReturnSuccessStatusCode()
      {
          // Arrange
          Job expected = this.testJob;
      
          // Act
          var response = await this.Client.GetByIdAsync(expected.Id);
      
          // Assert
          Assert.True(response.IsSuccessStatusCode);
          
          string content = response.Content;  
          Assert.False(string.IsNullOrWhiteSpace(content));
      
          var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<JobDto>(content);
      
          Assert.NotNull(returnedValue);
          Assert.Equal(expected.Id, returnedValue.Id);
      }
       
      [Fact]
              public async Task GetById_Incorrect_ReturnBadRequest()
              {
                  int id = -100;
                  var response = await this.Client.GetByIdAsync(id);
      
                  Assert.Multiple(
                      () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                      () => Assert.False(response.IsSuccessStatusCode));
              }
       

    }

}