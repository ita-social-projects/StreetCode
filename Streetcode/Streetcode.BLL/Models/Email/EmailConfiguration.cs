namespace Streetcode.BLL.Models.Email
{
    public class EmailConfiguration
    {
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public string SmtpServer { get; set; } = null!;
        public int Port { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
