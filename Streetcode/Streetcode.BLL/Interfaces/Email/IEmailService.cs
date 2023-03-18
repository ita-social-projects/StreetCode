using Streetcode.DAL.Entities.Email;

namespace Streetcode.BLL.Interfaces.Email
{
  public interface IEmailService
  {
    Task<bool> SendEmailAsync(Message message);
  }
}
