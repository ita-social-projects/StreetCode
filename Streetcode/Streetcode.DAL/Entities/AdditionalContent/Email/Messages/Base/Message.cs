using MimeKit;

namespace Streetcode.DAL.Entities.AdditionalContent.Email.Messages.Base
{
    public abstract class Message
    {
        public Message(IEnumerable<string> to, string from)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(string.Empty, x)));
        }

        public List<MailboxAddress> To { get; set; }

        public abstract MimeMessage ToMimeMessage(EmailConfiguration emailConfig);
    }
}