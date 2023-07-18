using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poplike.Common.Settings;

namespace Poplike.Application.Account.Commands.DoSignUp;

public class DoSignUpCommand : IDoSignUpCommand
{
    private readonly IDatabaseService _database;
    private readonly ILogger<DoSignUpCommand> _logger;
    private readonly IInvitationEmailTemplate _emailTemplate;
    private readonly ISmtpService _smtpService;
    private readonly UserAccountConfiguration _config;

    public DoSignUpCommand(
        IDatabaseService database,
        ILogger<DoSignUpCommand> logger,
        IInvitationEmailTemplate emailTemplate,
        ISmtpService smtpService,
        IOptions<UserAccountConfiguration> userAccountOptions)
    {
        _database = database;
        _logger = logger;
        _emailTemplate = emailTemplate;
        _smtpService = smtpService;
        _config = userAccountOptions.Value;
    }

    public async Task Execute(
        IUserToken userToken,
        DoSignUpCommandModel model)
    {
        if (!userToken.CanSignUp(_config))
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        model.Email = model.Email.ToLowerInvariant();

        if (await _database.Users.AnyAsync(x => x.EmailAddress == model.Email))
            throw new EmailAlreadyTakenException();

        var signup = new SignUp()
        {
            EmailAddress = model.Email,
        };

        var invitation = new Invitation()
        {
            Guid = Guid.NewGuid(),
            SignUp = signup,
        };

        if (await _database.Invitations.AnyAsync(x =>
                x.Guid == invitation.Guid))
            throw new Exception("Guid collision.");

        _database.SignUps.Add(signup);
        _database.Invitations.Add(invitation);

        await _database.SaveAsync(userToken);

        var message = _emailTemplate.Create(
            signup, invitation);

        try
        {
            _smtpService.SendMessage(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Error sending email to {Address}, in DoSignUpCommand. Exception: {Exception}",
                    model.Email, ex.Message);
        }
    }
}
