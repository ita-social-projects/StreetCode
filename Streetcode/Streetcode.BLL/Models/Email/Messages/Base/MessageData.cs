using MimeKit;

namespace Streetcode.BLL.Models.Email.Messages.Base
{
    public abstract class MessageData
    {
        public IEnumerable<string> To { get; set; } = new List<string>();
        public abstract MimeMessage ToMimeMessage();
    }
}