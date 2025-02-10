using System.Net;
using Moq;
using Streetcode.BLL.DTO.Email;
using Streetcode.BLL.Models.Email.Messages.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Email;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Email;

public class EmailControllerTests : BaseControllerTests<EmailClient>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public EmailControllerTests(CustomWebApplicationFactory<Program> factory)
        : base(factory, "/api/Email")
    {
        _factory = factory;
    }

    [Fact]
    public async Task Send_ValidData_ReturnsSuccessStatusCode()
    {
        // Arrange
        var emailDto = GetEmailDTO();

        // Act
        var response = await Client.Send(emailDto);

        // Assert
        _factory.EmailServiceMock.Verify(es => es.SendEmailAsync(It.IsAny<MessageData>()), Times.Once);
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