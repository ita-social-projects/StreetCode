using System.Net;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode;

[Collection("Authorization")]
public class StreetcodeDeleteControllerTests : BaseAuthorizationControllerTests<StreetcodeClient>
{
    private readonly StreetcodeContent _testSoftDeleteStreetcodeContent;
    private readonly StreetcodeContent _testDeleteStreetcodeContent;

    public StreetcodeDeleteControllerTests(
        CustomWebApplicationFactory<Program> factory,
        TokenStorage tokenStorage)
        : base(factory, "/api/Streetcode", tokenStorage)
    {
        var uniqueId = UniqueNumberGenerator.GenerateInt();
        var uniqueId2 = UniqueNumberGenerator.GenerateInt();
        _testSoftDeleteStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
        _testDeleteStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                uniqueId2,
                uniqueId2,
                Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task SoftDelete_WithValidId_ShouldReturnSuccess()
    {
        // Arrange
        DeleteSoftStreetcodeCommand command = new (_testSoftDeleteStreetcodeContent.Id);

        // Act
        var deleteResponse = await Client.SoftDeleteAsync(command, TokenStorage.AdminAccessToken);

        // Assert
        Assert.NotNull(deleteResponse);
        Assert.True(deleteResponse.IsSuccessStatusCode);

        var response = await Client.GetByIdAsync(command.Id);
        var streetcodeDto = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeDTO>(response.Content);
        Assert.NotNull(streetcodeDto);
        Assert.Equal(StreetcodeStatus.Deleted, streetcodeDto.Status);
    }

    [Fact]
    public async Task SoftDelete_WithNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        DeleteSoftStreetcodeCommand command = new (5555555);

        // Act
        var response = await Client.SoftDeleteAsync(command, TokenStorage.AdminAccessToken);

        // Assert
        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldReturnSuccess()
    {
        // Arrange
        DeleteStreetcodeCommand command = new (_testDeleteStreetcodeContent.Id);

        // Act
        var deleteResponse = await Client.DeleteAsync(command, TokenStorage.AdminAccessToken);

        // Assert
        Assert.NotNull(deleteResponse);
        Assert.True(deleteResponse.IsSuccessStatusCode);

        var response = await Client.GetByIdAsync(command.Id);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        DeleteStreetcodeCommand command = new (5555555);

        // Act
        var response = await Client.DeleteAsync(command, TokenStorage.AdminAccessToken);

        // Assert
        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

   
}