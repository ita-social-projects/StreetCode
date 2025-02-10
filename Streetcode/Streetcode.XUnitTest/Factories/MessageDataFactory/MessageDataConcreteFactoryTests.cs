using Microsoft.Extensions.Options;
using Moq;
using Streetcode.BLL.Factories.MessageDataFactory.Concretes;
using Streetcode.BLL.Models.Email;
using Streetcode.BLL.Models.Email.Messages;
using Xunit;

namespace Streetcode.XUnitTest.Factories.MessageDataFactory;

public class MessageDataConcreteFactoryTests
{
    private readonly MessageDataConcreteFactory _factory;
    private readonly EmailConfiguration _emailConfig;

    public MessageDataConcreteFactoryTests()
    {
        _emailConfig = new EmailConfiguration
        {
            To = "test@example.com",
            From = "noreply@example.com",
        };

        var optionsMock = new Mock<IOptions<EmailConfiguration>>();
        optionsMock.Setup(o => o.Value).Returns(_emailConfig);

        _factory = new MessageDataConcreteFactory(optionsMock.Object);
    }

    [Fact]
    public void CreateFeedbackMessageData_ShouldReturnValidMessage()
    {
        // Arrange
        string from = "user@example.com";
        string source = "test_source";
        string content = "test_content";

        // Act
        var message = _factory.CreateFeedbackMessageData(from, source, content);

        // Assert
        Assert.NotNull(message);
        Assert.IsType<FeedbackMessageData>(message);
    }

    [Fact]
    public void CreateForgotPasswordMessageData_ShouldReturnValidMessage()
    {
        // Arrange
        string[] to = { "user@example.com" };
        string token = "reset-token";
        string username = "TestUser";
        string currentDomain = "https://example.com";

        // Act
        var message = _factory.CreateForgotPasswordMessageData(to, token, username, currentDomain);

        // Assert
        Assert.NotNull(message);
        Assert.IsType<ForgotPasswordMessageData>(message);
    }
}