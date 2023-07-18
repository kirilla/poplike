using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Poplike.Application.Emails.BackgroundServices;
using Poplike.Application.Interfaces;
using Poplike.Common.Settings;
using Poplike.Domain;

namespace Poplike.Infrastructure;

public class SmtpService : ISmtpService
{
    private readonly ILogger<EmailSender> _logger;
    private readonly EmailAccountConfiguration _config;

    public SmtpService(
        ILogger<EmailSender> logger,
        IOptions<EmailAccountConfiguration> options)
    {
        _logger = logger;
        _config = options.Value;
    }

    public void SendMessage(Email email)
    {
        var mail = new MailMessage();

        mail.From = new MailAddress(_config.Address, _config.Name);

        if (!string.IsNullOrWhiteSpace(email.ReplyToAddress))
        {
            mail.ReplyToList.Add(new MailAddress(email.ReplyToAddress, email.ReplyToName));
        }

        mail.To.Add(new MailAddress(email.ToAddress, email.ToName));

        mail.Subject = email.Subject;
        //mail.Body = email.HtmlBody;
        //mail.IsBodyHtml = true;

        var textView = AlternateView.CreateAlternateViewFromString(
            email.TextBody, Encoding.UTF8, MediaTypeNames.Text.Plain);

        var htmlView = AlternateView.CreateAlternateViewFromString(
            email.HtmlBody, Encoding.UTF8, MediaTypeNames.Text.Html);

        mail.AlternateViews.Add(textView);
        mail.AlternateViews.Add(htmlView);

        //foreach (var attachment in attachments)
        //{
        //    var stream = new MemoryStream(attachment.Data, 0, attachment.Data.Length);

        //    //stream.Flush();
        //    //stream.Seek(0, 0);

        //    mail.Attachments.Add(new Attachment(
        //        stream, attachment.Name, attachment.ContentType));
        //}

        SmtpClient smtp = new SmtpClient(_config.SmtpHost);
        smtp.Credentials = new NetworkCredential(_config.Address, _config.Password);
        smtp.EnableSsl = true;
        smtp.Port = _config.SmtpPort;

        smtp.Send(mail);
    }
}
