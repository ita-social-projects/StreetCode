using System.Net;
using Streetcode.BLL.DTO.Email;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Email;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Email;

public class EmailControllerTests : BaseControllerTests<EmailClient>
{
    public EmailControllerTests(CustomWebApplicationFactory<Program> factory)
        : base(factory, "/api/Email")
    {
    }

    [Fact]
    public async Task Send_ValidData_ReturnsSuccessStatusCode()
    {
        // Arrange
        var emailDto = GetEmailDTO();

        // Act
        var response = await Client.Send(emailDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private EmailDTO GetEmailDTO()
    {
        return new EmailDTO
        {
            From = "test@test.com",
            Source = "test_source",
            Content = "test_content",
            Token = "test_token",
        };
    }
}