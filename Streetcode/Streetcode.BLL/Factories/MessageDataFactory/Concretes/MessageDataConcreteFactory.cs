using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Streetcode.BLL.Factories.MessageDataFactory.Abstracts;
using Streetcode.BLL.Models.Email;
using Streetcode.BLL.Models.Email.Messages;
using Streetcode.BLL.Models.Email.Messages.Base;

namespace Streetcode.BLL.Factories.MessageDataFactory.Concretes;

public class MessageDataConcreteFactory : IMessageDataAbstractFactory
{
    private readonly EmailConfiguration _configuration;

    public MessageDataConcreteFactory(IOptions<EmailConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }

    public MessageData CreateFeedbackMessageData(string from, string source, string content)
    {
        return new FeedbackMessageData
        {
            To = new string[] { _configuration.To ?? "stagestreetcodedev@gmail.com" },
            Source = source,
            Content = content,
            From = from
        };
    }

    public MessageData CreateForgotPasswordMessageData(string[] to, string token, string username, string currentDomain)
    {
        return new ForgotPasswordMessageData
        {
            From = _configuration.From,
            To = to,
            Token = token,
            Username = username,
            CurrentDomain = currentDomain
        };
    }
}