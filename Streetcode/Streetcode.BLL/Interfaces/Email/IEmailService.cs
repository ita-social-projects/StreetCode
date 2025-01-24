using Streetcode.DAL.Entities.AdditionalContent.Email.Messages.Base;

namespace Streetcode.BLL.Interfaces.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(MessageData messageData);
    }
}
