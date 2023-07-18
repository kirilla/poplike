using Microsoft.Extensions.Options;
using Poplike.Common.Constants;
using Poplike.Common.Settings;

namespace Poplike.Application.Account.Commands.DoSignUp;

public class InvitationEmailTemplate : IInvitationEmailTemplate
{
    private readonly EmailAccountConfiguration _configuration;

    public InvitationEmailTemplate(
        IOptions<EmailAccountConfiguration> options)
    {
        _configuration = options.Value;
    }

    public Email Create(SignUp signup, Invitation invitation)
    {
        var email = new Email()
        {
            ToName = "Kära medmänniska",
            ToAddress = signup.EmailAddress,

            FromName = _configuration.Name,
            FromAddress = _configuration.Address,

            ReplyToName = null,
            ReplyToAddress = null,

            Subject = $"Kom till {Branding.EmailWebsiteName} {Branding.WebsiteEmoji}",

            HtmlBody = CreateHtmlBody(signup, invitation),
            TextBody = CreateTextBody(signup, invitation),

            Status = EmailStatus.NotSent
        };

        return email;
    }

    private string CreateHtmlBody(SignUp signup, Invitation invitation)
    {
        var guid = invitation.Guid.ToString()!.ToLower();

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
                <h5>Hej!</h5>
                <p>
                    Någon har bett om ett konto hos
                    <a href="https://poplike.se/">Poplike</a>
                    och vi vet att det är du.
                </p>
                <p>
                    Det är ingen idé att förneka!
                </p>
                <p>
                    <a href="https://poplike.se/invitation/accept/{guid}">Skapa ett konto nu</a> eller 
                    <a href="https://poplike.se/invitation/reject/{guid}">låt bli</a>.
                </p>
                <p>
                    Mvh, Poplike 👍🏼
                </p>
            </body>
            """;
    }

    private string CreateTextBody(SignUp signup, Invitation invitation)
    {
        var guid = invitation.Guid.ToString()!.ToLower();

        return $"""
            Hej!

            Någon har bett om ett konto hos Poplike
            https://poplike.se/
            och vi vet att det är du.
            
            Det är ingen idé att förneka!
            
            Skapa ett konto nu
            https://poplike.se/invitation/accept/{guid}
            
            Eller låt bli
            https://poplike.se/invitation/reject/{guid}

            Mvh, Poplike 👍🏼
            """;
    }
}
