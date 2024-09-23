using MimeKit;

namespace Streetcode.DAL.Entities.AdditionalContent.Email
{
    public class Message
    {
        public Message(IEnumerable<string> to, string from, string subject, string content)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(string.Empty, x)));
            From = from;
            Content = content;
            Subject = subject;
        }

        public List<MailboxAddress> To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
