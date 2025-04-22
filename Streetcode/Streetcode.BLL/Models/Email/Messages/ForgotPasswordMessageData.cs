using MimeKit;
using Streetcode.BLL.Constants.MessageData;
using Streetcode.BLL.Models.Email.Messages.Base;

namespace Streetcode.BLL.Models.Email.Messages;

public class ForgotPasswordMessageData : MessageData
{
    public string From { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string CurrentDomain { get; set; } = string.Empty;

    public override MimeMessage ToMimeMessage()
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("", From));
        emailMessage.To.AddRange(To.Select(x => new MailboxAddress(string.Empty, x)));
        emailMessage.Subject = MessageDataConstants.ForgotPasswordTitle;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"""
                      <p>Привіт,</p>
                      <p>Ми отримали запит на відновлення паролю для вашого облікового запису.</p>
                      <p>Щоб продовжити, будь ласка, натисніть посилання нижче:</p>
                      <p>[<a href="{CurrentDomain}/forgot-password-reset?token={Token}&username={Username}" class="btn">Відновити пароль</a>]</p>
                      <p>Якщо ви не робили цей запит, просто проігноруйте цей лист. Ваш пароль залишиться незмінним.</p>
                      <p>З найкращими побажаннями,<br>Команда Historycode</p>
                      """,
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }
}