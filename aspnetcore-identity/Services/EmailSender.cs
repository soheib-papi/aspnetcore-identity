using System.Text;
using aspnetcore_identity.Models.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using aspnetcore_identity.Services;
using Microsoft.Extensions.Options;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken)
    {
        var emailMessage = new MimeMessage
        {
            Subject = subject,
            Body = new BodyBuilder { HtmlBody = message }.ToMessageBody()
        };

        emailMessage.From.Add(new MailboxAddress("MyIdentity", _emailSettings.SenderEmail));
        emailMessage.To.Add(new MailboxAddress(email, email));

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(
                _emailSettings.SmtpServer, 
                _emailSettings.Port, 
                _emailSettings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto,
                cancellationToken);

            await client.AuthenticateAsync(
                Encoding.UTF8, 
                _emailSettings.SenderEmail, 
                _emailSettings.Password, 
                cancellationToken);

            await client.SendAsync(emailMessage, cancellationToken);
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}