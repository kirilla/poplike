using Microsoft.Extensions.Options;
using Poplike.Common.Constants;
using Poplike.Common.Settings;

namespace Poplike.Application.Account.Commands.RequestPasswordReset;

public class PasswordResetEmailTemplate : IPasswordResetEmailTemplate
{
    private readonly EmailAccountConfiguration _configuration;

    public PasswordResetEmailTemplate(
        IOptions<EmailAccountConfiguration> options)
    {
        _configuration = options.Value;
    }

    public Email Create(User user, PasswordResetRequest resetRequest)
    {
        var email = new Email()
        {
            ToName = user.Name,
            ToAddress = user.EmailAddress,

            FromName = _configuration.Name,
            FromAddress = _configuration.Address,

            ReplyToName = null,
            ReplyToAddress = null,

            Subject = "Byta lösenord?",

            HtmlBody = CreateHtmlBody(user, resetRequest),
            TextBody = CreateTextBody(user, resetRequest),

            Status = EmailStatus.NotSent
        };

        return email;
    }

    private string CreateHtmlBody(User user, PasswordResetRequest resetRequest)
    {
        var guid = resetRequest.Guid.ToString()!.ToLower();

        return $"""
            <!DOCTYPE html>
            <html lang="sv">
            <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width">
            <style>
            </style>
            </head>
            <body style="background-color:white">
                <h4>Hej {user.Name}!</h4>
                <p>
                    Någon har bett om att byta lösenord för ett "konto" 
                    hos Poplike.se med din adress.
                </p>
                <p>
                    Vi säger inte att det är ditt konto, men ...
                </p>
                <p>
                    Om du vill byta lösenord så använd 
                    <a href="https://poplike.se/account/resetpassword/{guid}">den här länken</a>.
                </p>
                <p>
                    Mvh, {Branding.EmailSignatureName}
                </p>
            </body>
            """;
    }

    private string CreateTextBody(User user, PasswordResetRequest resetRequest)
    {
        var guid = resetRequest.Guid.ToString()!.ToLower();

        return $"""
            Hej {user.Name}!

            Någon har bett om att byta lösenord för ett "konto" 
            hos Poplike.se med din adress.

            Vi säger inte att det är ditt konto, men ...

            Om du vill byta lösenord så använd den här länken:
            https://poplike.se/account/resetpassword/{guid}

            Mvh, {Branding.EmailSignatureName}
            """;
    }
}
