using System.Net;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Analytics;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Analytics;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Analytics;

[Collection("Authorization")]
public class StatisticRecordControllerTests : BaseAuthorizationControllerTests<AnalyticsClient>
{
    private readonly StatisticRecord _statisticRecord;

    public StatisticRecordControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/StatisticRecord", tokenStorage)
    {
        var uniqueStreetcodeId = UniqueNumberGenerator.GenerateInt();
        var uniqueStatisticRecordId = UniqueNumberGenerator.GenerateInt();
        StreetcodeContentExtracter
            .Extract(
                uniqueStreetcodeId,
                uniqueStreetcodeId,
                Guid.NewGuid().ToString());
        _statisticRecord = StatisticRecordExtracter.Extract(uniqueStatisticRecordId, uniqueStreetcodeId);
    }

    [Fact]
    public async Task Update_ReturnSuccessStatusCode()
    {
        // Arrange
        var validId = _statisticRecord.QrId;

        // Act
        var response = await Client.Update(validId, TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task IncorrectId_Update_ReturnSuccessStatusCode()
    {
        // Arrange
        var invalidId = -9999;

        // Act
        var response = await Client.Update(invalidId, TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ReturnSuccessStatusCode()
    {
        // Act
        var response = await Client.GetAll();
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<List<StatisticRecordDTO>>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetByQrId_ReturnSuccessStatusCode()
    {
        // Arrange
        var validQrId = _statisticRecord.QrId;

        // Act
        var response = await Client.GetByQrId(validQrId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<StatisticRecordDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetByQrId_ReturnBadRequest()
    {
        // Arrange
        var validQrId = -9999;

        // Act
        var response = await Client.GetByQrId(validQrId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<StatisticRecordDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    public async Task ExistByQrId_ReturnSuccessStatusCode()
    {
        // Arrange
        var validQrId = _statisticRecord.QrId;

        // Act
        var response = await Client.ExistByQrId(validQrId);
        bool.TryParse(response.Content, out bool result);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(result);
    }

    [Fact]
    public async Task ExistByQrId_ReturnBadRequest()
    {
        // Arrange
        var validQrId = -9999;

        // Act
        var response = await Client.ExistByQrId(validQrId);
        bool.TryParse(response.Content, out bool result);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllByStreetcodeId_ReturnSuccessStatusCode()
    {
        // Arrange
        var streetcodeId = _statisticRecord.StreetcodeId;

        // Act
        var response = await Client.GetAllByStreetcodeId(streetcodeId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<List<StatisticRecordDTO>>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetAllByStreetcodeId_ReturnBadRequest()
    {
        // Arrange
        var streetcodeId = -9999;

        // Act
        var response = await Client.GetAllByStreetcodeId(streetcodeId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<StatisticRecordDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    public async Task Delete_ReturnSuccessCode()
    {
        // Arrange
        var id = _statisticRecord.QrId;

        // Act
        var response = await Client.Delete(id, TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnBadRequest()
    {
        // Arrange
        var id = -9999;

        // Act
        var response = await Client.Delete(id, TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            StatisticRecordExtracter.Remove(_statisticRecord);
        }

        base.Dispose(disposing);
    }
}