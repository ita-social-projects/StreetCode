using System.Net;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Team;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Team;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Team;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Team
{
    [Collection("Authorization")]
    public class TeamControllerTests : BaseAuthorizationControllerTests<TeamClient>,
        IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly TeamMember _testTeamMember;
        private readonly Positions _testPosition;

        public TeamControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "api/Team", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();

            _testTeamMember = TeamMemberExtracter.Extract(uniqueId, uniqueId);

            _testPosition = TeamPositionsExtracter.Extract(uniqueId);

            TeamPositionsExtracter.AddTeamMemberPositions(_testTeamMember.Id, _testPosition.Id);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            // Act
            var response = await this.Client.GetAllAsync(1, 10, this.TokenStorage.AdminAccessToken);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllTeamDTO>(response.Content);

            // Assert
            Assert.Multiple(
                () => Assert.True(response.IsSuccessStatusCode),
                () => Assert.NotNull(returnedValue));
        }

        [Fact]
        public async Task GetAllMain_ReturnSuccessStatusCode()
        {
            // Act
            var response = await this.Client.GetAllMainAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<GetAllTeamDTO>>(response.Content);

            // Assert
            Assert.Multiple(
                () => Assert.True(response.IsSuccessStatusCode),
                () => Assert.NotNull(returnedValue));
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            // Arrange
            TeamMember expectedTeamMember = _testTeamMember;

            // Act
            var response = await this.Client.GetByIdAsync(expectedTeamMember.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TeamMemberDTO>(response.Content);

            // Assert
            Assert.Multiple(
                () => Assert.True(response.IsSuccessStatusCode),
                () => Assert.NotNull(returnedValue),
                () => Assert.Equal(expectedTeamMember.Id, returnedValue?.Id),
                () => Assert.Equal(expectedTeamMember.Name, returnedValue?.Name));
        }

        [Fact]
        public async Task GetById_Incorrect_ReturnBadRequest()
        {
            // Act
            const int incorrectId = -1;
            var response = await this.Client.GetByIdAsync(incorrectId);

            // Assert
            Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByRoleId_ReturnSuccessStatusCode()
        {
            // Arrange
            TeamMember expectedTeamMember = _testTeamMember;

            // Act
            var response = await this.Client.GetByRoleIdAsync(_testPosition.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<TeamMemberDTO>>(response.Content);

            // Assert
            Assert.Multiple(
                () => Assert.True(response.IsSuccessStatusCode),
                () => Assert.NotNull(returnedValue),
                () => Assert.Equal(expectedTeamMember.Id, returnedValue!.Single().Id),
                () => Assert.Equal(expectedTeamMember.Name, returnedValue!.Single().Name));
        }

        [Fact]
        [ExtractCreateTeamMember]
        public async Task Create_ReturnSuccessStatusCode()
        {
            // Arrange
            var teamMemberCreateDto = ExtractCreateTeamMemberAttribute.TeamMemberForTest;

            // Act
            var response = await this.Client.CreateAsync(teamMemberCreateDto, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTeamMember]
        public async Task Create_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var teamMemberCreateDto = ExtractCreateTeamMemberAttribute.TeamMemberForTest;

            // Act
            var response = await this.Client.CreateAsync(teamMemberCreateDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTeamMember]
        public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var teamMemberCreateDto = ExtractCreateTeamMemberAttribute.TeamMemberForTest;

            // Act
            var response = await this.Client.CreateAsync(teamMemberCreateDto, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTeamMember]
        public async Task Create_CreatesNewTeamMember()
        {
            // Arrange
            var teamMemberCreateDto = ExtractCreateTeamMemberAttribute.TeamMemberForTest;

            // Act
            var response = await this.Client.CreateAsync(teamMemberCreateDto, this.TokenStorage.AdminAccessToken);
            var fetchedTeamMember = CaseIsensitiveJsonDeserializer.Deserialize<TeamMemberDTO>(response.Content);

            // Assert
            Assert.Equal(teamMemberCreateDto.Name, fetchedTeamMember?.Name);
        }

        [Fact]
        [ExtractCreateTeamMember]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var teamMemberCreateDto = ExtractCreateTeamMemberAttribute.TeamMemberForTest;
            teamMemberCreateDto.Name = null!;

            // Act
            var response = await this.Client.CreateAsync(teamMemberCreateDto, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTeamMember]
        public async Task Create_WithExistingImage_ReturnsBadRequest()
        {
            // Arrange
            var teamMemberCreateDto = ExtractCreateTeamMemberAttribute.TeamMemberForTest;
            teamMemberCreateDto.ImageId = _testTeamMember.ImageId;

            // Act
            var response = await this.Client.CreateAsync(teamMemberCreateDto, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTeamMember]
        public async Task Update_ReturnsSuccessStatusCode()
        {
            // Arrange
            var teamMemberUpdateDto = ExtractUpdateTeamMemberAttribute.TeamMemberForTest;

            // Act
            var response = await this.Client.UpdateAsync(teamMemberUpdateDto, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTeamMember]
        public async Task Update_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            const int id = -1;
            var teamMemberUpdateDto = ExtractUpdateTeamMemberAttribute.TeamMemberForTest;
            teamMemberUpdateDto.Id = id;

            // Act
            var response = await this.Client.UpdateAsync(teamMemberUpdateDto, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        [ExtractUpdateTeamMember]
        public async Task Update_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var teamMemberUpdateDto = ExtractUpdateTeamMemberAttribute.TeamMemberForTest;

            // Act
            var response = await this.Client.UpdateAsync(teamMemberUpdateDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractUpdateTeamMember]
        public async Task Update_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var teamMemberUpdateDto = ExtractUpdateTeamMemberAttribute.TeamMemberForTest;

            // Act
            var response = await this.Client.UpdateAsync(teamMemberUpdateDto, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractDeleteTeamMember]
        public async Task Delete_ReturnsSuccessStatusCode()
        {
            // Arrange
            int id = ExtractDeleteTeamMemberAttribute.TeamMemberForTest.Id;

            // Act
            var response = await this.Client.DeleteAsync(id, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            int id = _testTeamMember.Id;

            // Act
            var response = await this.Client.DeleteAsync(id);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Delete_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            int id = _testTeamMember.Id;

            // Act
            var response = await this.Client.DeleteAsync(id, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WithInvalidData_ReturnsBadRequest()
        {
            // Act
            const int id = -1;
            var response = await this.Client.DeleteAsync(id, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
