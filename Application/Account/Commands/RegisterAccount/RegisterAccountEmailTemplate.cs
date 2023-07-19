using Microsoft.Extensions.Options;
using Poplike.Common.Constants;
using Poplike.Common.Settings;

namespace Poplike.Application.Account.Commands.RegisterAccount;

public class RegisterAccountEmailTemplate : IRegisterAccountEmailTemplate
{
    private readonly EmailAccountConfiguration _configuration;

    public RegisterAccountEmailTemplate(
        IOptions<EmailAccountConfiguration> options)
    {
        _configuration = options.Value;
    }

    public Email Create(User user)
    {
        var email = new Email()
        {
            ToName = user.Name,
            ToAddress = user.EmailAddress,

            FromName = _configuration.Name,
            FromAddress = _configuration.Address,

            ReplyToName = null,
            ReplyToAddress = null,

            Subject = $"Välkommen till {Branding.EmailSignatureName}",

            HtmlBody = CreateHtmlBody(user),
            TextBody = CreateTextBody(user),

            Status = EmailStatus.NotSent
        };

        return email;
    }

    private string CreateHtmlBody(User user)
    {
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
                    Ett konto har skapats på
                    <a href="https://poplike.se/">Poplike.se</a>
                    med din epostadress.
                </p>
                <p>
                    Vi säger inte att det var du, men vi tror det.
                </p>
                <p>
                    Mvh, {Branding.EmailSignatureName}
                </p>
            </body>
            """;
    }

    private string CreateTextBody(User user)
    {
        return $"""
            Hej {user.Name}!

            Ett konto har skapats på
            https://poplike.se/
            med din epostadress.

            Vi säger inte att det var du, men vi tror det.

            Mvh, {Branding.EmailSignatureName}
            """;
    }
}
