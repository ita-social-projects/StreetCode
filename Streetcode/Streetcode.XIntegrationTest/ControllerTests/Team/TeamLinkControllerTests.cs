using System.Net;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Team;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Team;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Team;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Team;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Team
{
    [Collection("Authorization")]
    public class TeamLinkControllerTests : BaseAuthorizationControllerTests<TeamLinkClient>,
        IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Image _testImage;
        private readonly TeamMember _testTeamMember;
        private readonly TeamMemberLink _testLink;

        public TeamLinkControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "api/TeamLink", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            _testImage = ImageExtracter.Extract(uniqueId);
            _testTeamMember = TeamMemberExtracter.Extract(uniqueId, _testImage.Id);
            _testTeamMember.ImageId = _testImage.Id;
            _testLink = TeamLinkExtracter.Extract(uniqueId, _testTeamMember.Id);
        }

        [Fact]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<TeamMemberLinkDTO>>(response.Content);

            // Assert
            Assert.Multiple(
                () => Assert.True(response.IsSuccessStatusCode),
                () => Assert.NotNull(returnedValue));
        }

        [Fact]
        [ExtractCreateTeamLink]
        public async Task Create_ReturnSuccessStatusCode()
        {
            // Act
            var teamLinkCreateDto = ExtractCreateTeamLinkAttribute.TeamLinkForTest;
            teamLinkCreateDto.TeamMemberId = _testTeamMember.Id;

            // Act
            var response = await this.Client.CreateAsync(teamLinkCreateDto, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTeamLink]
        public async Task Create_TokenNotPassed_ReturnsUnauthorized()
        {
            // Act
            var teamLinkCreateDto = ExtractCreateTeamLinkAttribute.TeamLinkForTest;

            // Act
            var response = await this.Client.CreateAsync(teamLinkCreateDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTeamLink]
        public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Act
            var teamLinkCreateDto = ExtractCreateTeamLinkAttribute.TeamLinkForTest;
            teamLinkCreateDto.TeamMemberId = _testTeamMember.Id;

            // Act
            var response = await this.Client.CreateAsync(teamLinkCreateDto, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTeamLink]
        public async Task Create_CreatesNewTeamMemberLink()
        {
            // Act
            var teamLinkCreateDto = ExtractCreateTeamLinkAttribute.TeamLinkForTest;
            teamLinkCreateDto.TeamMemberId = _testTeamMember.Id;

            // Act
            var response = await this.Client.CreateAsync(teamLinkCreateDto, this.TokenStorage.AdminAccessToken);
            var fetchedLink = CaseIsensitiveJsonDeserializer.Deserialize<TeamMemberLinkDTO>(response.Content);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(teamLinkCreateDto.LogoType, fetchedLink?.LogoType),
                () => Assert.Equal(teamLinkCreateDto.TargetUrl, fetchedLink?.TargetUrl));
        }

        [Fact]
        [ExtractCreateTeamLink]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var teamLinkCreateDto = ExtractCreateTeamLinkAttribute.TeamLinkForTest;
            teamLinkCreateDto.TargetUrl = null!;

            // Act
            var response = await this.Client.CreateAsync(teamLinkCreateDto, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        
    }
}
