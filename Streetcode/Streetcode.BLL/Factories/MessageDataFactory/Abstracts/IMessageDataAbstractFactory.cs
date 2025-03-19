using Streetcode.BLL.Models.Email.Messages.Base;

namespace Streetcode.BLL.Factories.MessageDataFactory.Abstracts;

public interface IMessageDataAbstractFactory
{
    MessageData CreateFeedbackMessageData(string from, string source, string content);
    MessageData CreateForgotPasswordMessageData(string[] to, string token, string username, string currentDomain);
    MessageData CreateDeleteConfirmationMessageData(string[] to);
}