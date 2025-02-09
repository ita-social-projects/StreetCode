using System.Net;
using FluentResults;
using Newtonsoft.Json;
using Streetcode.BLL.DTO.AdditionalContent.Filter;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.CatalogItem;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode;

[Collection("Authorization")]
public class StreetcodeQueriesControllerTests : BaseAuthorizationControllerTests<StreetcodeClient>
{
    private readonly StreetcodeContent _testStreetcodeContent;
    private readonly StreetcodeContent _testStreetcodeContent2;
    private readonly StreetcodeContent _testStreetcodeContent3;

    public StreetcodeQueriesControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/Streetcode", tokenStorage)
    {
        int uniqueId = UniqueNumberGenerator.GenerateInt();
        int uniqueId2 = UniqueNumberGenerator.GenerateInt();
        int uniqueId3 = UniqueNumberGenerator.GenerateInt();
        _testStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
        _testStreetcodeContent2 = StreetcodeContentExtracter
            .Extract(
                uniqueId2,
                uniqueId2,
                Guid.NewGuid().ToString());
        _testStreetcodeContent2.Title = "Another_Title";
        _testStreetcodeContent2.Status = StreetcodeStatus.Draft;
        _testStreetcodeContent3 = StreetcodeContentExtracter
            .Extract(
                uniqueId3,
                uniqueId3,
                Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task GetAllStreetcodes_ReturnsSuccess()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO();

        // Act
        var response = await Client.GetAllAsync(request);
        var responseDto = CaseIsensitiveJsonDeserializer.Deserialize<GetAllStreetcodesResponseDTO>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
        Assert.NotEmpty(responseDto!.Streetcodes);
    }

    [Fact]
    public async Task GetAllStreetcodes_WithPagination_ReturnsCorrectAmount()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO { Page = 1, Amount = 2 };

        // Act
        var response = await Client.GetAllAsync(request);
        var responseDto = CaseIsensitiveJsonDeserializer.Deserialize<GetAllStreetcodesResponseDTO>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
        Assert.InRange(responseDto!.Streetcodes.Count(), 1, 2);
    }

    [Fact]
    public async Task GetAllStreetcodes_WithIncorrectPagination_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO { Page = -2, Amount = 2 };

        // Act
        var response = await Client.GetAllAsync(request);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    public async Task GetAllStreetcodes_WithTitleFilter_ReturnsMatchingResults()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO { Title = "Test_Title" };

        // Act
        var response = await Client.GetAllAsync(request);
        var responseDto = CaseIsensitiveJsonDeserializer.Deserialize<GetAllStreetcodesResponseDTO>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
        Assert.All(responseDto!.Streetcodes, s => Assert.Contains("Test_Title", s.Title, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetAllStreetcodes_WithNonExistingTitleFilter_ReturnsEmptyList()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO { Title = "NonExisting_Title" };

        // Act
        var response = await Client.GetAllAsync(request);
        var responseDto = CaseIsensitiveJsonDeserializer.Deserialize<GetAllStreetcodesResponseDTO>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
        Assert.Empty(responseDto!.Streetcodes);
    }

    [Fact]
    public async Task GetAllStreetcodes_WithSorting_ReturnsSortedResults()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO { Sort = "Title" };

        // Act
        var response = await Client.GetAllAsync(request);
        var responseDto = CaseIsensitiveJsonDeserializer.Deserialize<GetAllStreetcodesResponseDTO>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
        var sorted = responseDto!.Streetcodes.OrderBy(s => s.Title).ToList();
        Assert.Equal(sorted, responseDto.Streetcodes.ToList());
    }

    [Fact]
    public async Task GetAllStreetcodes_WithIncorrectSortingProp_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO { Sort = "IncorrectTitle" };

        // Act
        var response = await Client.GetAllAsync(request);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    public async Task GetAllStreetcodes_WithFilter_ReturnsFilteredResults()
    {
        // Arrange
        string testTitle = "Test_Title";
        var request = new GetAllStreetcodesRequestDTO { Filter = $"Title:{testTitle}" };

        // Act
        var response = await Client.GetAllAsync(request);
        var responseDto = CaseIsensitiveJsonDeserializer.Deserialize<GetAllStreetcodesResponseDTO>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
        Assert.Equal(2, responseDto!.Streetcodes.Count());
        Assert.All(responseDto!.Streetcodes, s => Assert.Equal(testTitle, s.Title));
    }

    [Fact]
    public async Task GetAllStreetcodes_WithIncorrectFilter_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetAllStreetcodesRequestDTO { Filter = "IncorrectStatus:Published" };

        // Act
        var response = await Client.GetAllAsync(request);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    public async Task GetAllPublished_ReturnSuccess()
    {
        // Act
        var response = await Client.GetAllPublishedAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var streetcodes = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeShortDTO>>(response.Content);
        Assert.NotNull(streetcodes);
        Assert.NotEmpty(streetcodes);
    }

    [Fact]
    public async Task GetAllShort_ReturnSuccess()
    {
        // Act
        var response = await Client.GetAllShortAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var streetcodes = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeShortDTO>>(response.Content);
        Assert.NotNull(streetcodes);
        Assert.NotEmpty(streetcodes);
    }

    [Fact]
    public async Task GetAllMainPage_ReturnSuccess()
    {
        // Act
        var response = await Client.GetAllMainPageAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var streetcodes = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeShortDTO>>(response.Content);
        Assert.NotNull(streetcodes);
        Assert.NotEmpty(streetcodes);
    }

    [Fact]
    public async Task GetPageMainPage_ReturnsPaginatedPublishedStreetcodes()
    {
        // Arrange
        ushort page = 1;
        ushort pageSize = 5;

        // Act
        var response = await Client.GetPageMainPageAsync(page, pageSize);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var streetcodes = (CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeMainPageDTO>>(response.Content) ?? Array.Empty<StreetcodeMainPageDTO>()).ToList();

        Assert.NotNull(streetcodes);
        Assert.NotEmpty(streetcodes);
        Assert.True(streetcodes.Count <= pageSize);
    }

    [Fact]
    public async Task GetShortById_ReturnSuccess()
    {
        // Arrange
        int id = _testStreetcodeContent.Id;

        // Act
        var response = await Client.GetShortByIdAsync(id);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeShortDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response);
        Assert.Equal(id, returnedValue!.Id);
    }

    [Fact]
    public async Task GetShortById_Incorrect_ReturnBadRequest()
    {
        // Arrange
        int id = -100;

        // Act
        var response = await Client.GetShortByIdAsync(id);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    public async Task GetByFilter_WithExistingSearchQuery_ReturnsFilteredStreetcodes()
    {
        // Arrange
        var request = new StreetcodeFilterRequestDTO
        {
            SearchQuery = "Test_Title",
        };

        // Act
        var response = await Client.GetByFilterAsync(request);
        var responseDto = CaseIsensitiveJsonDeserializer.Deserialize<List<StreetcodeFilterResultDTO>>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(responseDto);
        Assert.True(responseDto.Count == 2);
    }

    [Fact]
    public async Task GetByFilter_WithNonExistingSearchQuery_ReturnsEmptyList()
    {
        // Arrange
        var request = new StreetcodeFilterRequestDTO
        {
            SearchQuery = "NonExisting_Title",
        };

        // Act
        var response = await Client.GetByFilterAsync(request);
        var responseDto = CaseIsensitiveJsonDeserializer.Deserialize<List<StreetcodeFilterResultDTO>>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(responseDto);
        Assert.Empty(responseDto);
    }

    [Fact]
    public async Task ExistWithIndex_WithExistingIndex_ReturnsTrue()
    {
        // Arrange
        int existingIndex = _testStreetcodeContent.Index;

        // Act
        var response = await Client.ExistWithIndexAsync(existingIndex);
        var exists = JsonConvert.DeserializeObject<bool>(response.Content!);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistWithIndex_WithNonExistingIndex_ReturnsFalse()
    {
        // Arrange
        int nonExistingIndex = -99999;

        // Act
        var response = await Client.ExistWithIndexAsync(nonExistingIndex);
        var exists = JsonConvert.DeserializeObject<bool>(response.Content!);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistWithUrl_WithExistingUrl_ReturnsTrue()
    {
        // Arrange
        string existingUrl = _testStreetcodeContent.TransliterationUrl ?? string.Empty;

        // Act
        var response = await Client.ExistWithUrlAsync(existingUrl);
        var exists = JsonConvert.DeserializeObject<bool>(response.Content!);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistWithUrl_WithNonExistingUrl_ReturnsFalse()
    {
        // Arrange
        string nonExistingUrl = "non-existing-url";

        // Act
        var response = await Client.ExistWithUrlAsync(nonExistingUrl);
        var exists = JsonConvert.DeserializeObject<bool>(response.Content!);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.False(exists);
    }

    [Fact]
    public async Task GetAllCatalog_ReturnsSuccessAndNonEmptyList()
    {
        // Arrange
        int page = 1;
        int count = 10;

        // Act
        var response = await Client.GetAllCatalogAsync(page, count);
        var catalogItems = CaseIsensitiveJsonDeserializer.Deserialize<List<CatalogItem>>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(catalogItems);
        Assert.NotEmpty(catalogItems);
        Assert.InRange(catalogItems.Count, 1, 10);
    }

    [Fact]

    // is it really good send OK(with empty list) if invalid number for count
    public async Task GetAllCatalog_WithInvalidCount_ReturnsEmptyList()
    {
        // Arrange
        int page = 1;
        int count = -10;

        // Act
        var response = await Client.GetAllCatalogAsync(page, count);
        var catalogItems = CaseIsensitiveJsonDeserializer.Deserialize<List<CatalogItem>>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(catalogItems);
        Assert.Empty(catalogItems);
    }

    [Fact]
    public async Task GetCount_ReturnsValidCount()
    {
        // Arrange
        bool? onlyPublished = false;

        // Act
        var response = await Client.GetCountAsync(onlyPublished);
        var count = JsonConvert.DeserializeObject<int>(response.Content!);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.True(count == 3);
    }

    [Fact]
    public async Task GetCount_WithOnlyPublished_ReturnsOnlyPublishedCount()
    {
        // Arrange
        bool? onlyPublished = true;

        // Act
        var response = await Client.GetCountAsync(onlyPublished);
        var count = JsonConvert.DeserializeObject<int>(response.Content!);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GetByTransliterationUrl_ReturnsStreetcode_WhenExists()
    {
        // Arrange
        string existingUrl = _testStreetcodeContent.TransliterationUrl ?? string.Empty;

        // Act
        var response = await Client.GetByTransliterationUrl(existingUrl);
        var streetcode = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeDTO>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(streetcode);
        Assert.Equal(existingUrl, streetcode.TransliterationUrl);
    }

    [Fact]
    public async Task GetByTransliterationUrl_ReturnsBadRequest_WhenNotExists()
    {
        // Arrange
        string nonExistingUrl = "non-exiting-url";

        // Act
        var response = await Client.GetByTransliterationUrl(nonExistingUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsStreetcode_WhenExists()
    {
        // Arrange
        int existingId = _testStreetcodeContent.Id;

        // Act
        var response = await this.Client.GetByIdAsync(existingId);
        var streetcode = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeDTO>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(streetcode);
        Assert.Equal(existingId, streetcode.Id);
    }

    [Fact]
    public async Task GetById_ReturnsBadRequest_WhenNotExists()
    {
        // Arrange
        int nonExistingId = -1000;

        // Act
        var response = await Client.GetByIdAsync(nonExistingId);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractStatisticRecord]
    public async Task GetByQrId_ReturnsCorrectUrl()
    {
        // Arrange
        int qrId = ExtractStatisticRecordAttribute.QrId;

        // Act
        var response = await Client.GetByQrIdAsync(qrId);
        var responseValue = CaseIsensitiveJsonDeserializer.Deserialize<string>(response.Content);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(responseValue);
        Assert.Equal(ExtractStatisticRecordAttribute.TestStreetcode.TransliterationUrl, responseValue);
    }

    [Fact]
    [ExtractStatisticRecord]
    public async Task GetByQrId_ReturnsBadRequest_WhenNotExists()
    {
        // Arrange
        int nonExistingId = -1000;

        // Act
        var response = await Client.GetByQrIdAsync(nonExistingId);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetByIndex_ReturnsStreetcode_WhenExists()
    {
        // Arrange
        int existingId = _testStreetcodeContent.Id;

        // Act
        var response = await this.Client.GetByIndexAsync(existingId);
        var streetcode = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeDTO>(response.Content);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(streetcode);
        Assert.Equal(existingId, streetcode.Id);
    }

    [Fact]
    public async Task GetByIndex_ReturnsBadRequest_WhenNotExists()
    {
        // Arrange
        int nonExistingId = -1000;

        // Act
        var response = await Client.GetByIndexAsync(nonExistingId);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            StreetcodeContentExtracter.Remove(_testStreetcodeContent);
            StreetcodeContentExtracter.Remove(_testStreetcodeContent2);
            StreetcodeContentExtracter.Remove(_testStreetcodeContent3);
        }

        base.Dispose(disposing);
    }
}