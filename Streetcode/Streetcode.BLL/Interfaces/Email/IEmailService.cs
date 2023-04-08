using Streetcode.DAL.Entities.AdditionalContent.Email;

namespace Streetcode.BLL.Interfaces.Email
{
  public interface IEmailService
  {
    Task<bool> SendEmailAsync(Message message);
  }
}
