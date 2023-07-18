using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poplike.Application.Legal.Filters;
using Poplike.Common.Settings;

namespace Poplike.Application.Account.Commands.RegisterAccount;

public class RegisterAccountCommand : IRegisterAccountCommand
{
    private readonly IDatabaseService _database;
    private readonly ILogger<RegisterAccountCommand> _logger;
    private readonly IWordPreventionFilter _filter;
    private readonly IRegisterAccountEmailTemplate _emailTemplate;
    private readonly ISmtpService _smtpService;
    private readonly UserAccountConfiguration _config;

    public RegisterAccountCommand(
        IDatabaseService database,
        ILogger<RegisterAccountCommand> logger,
        IWordPreventionFilter filter,
        IRegisterAccountEmailTemplate emailTemplate,
        ISmtpService smtpService,
        IOptions<UserAccountConfiguration> userAccountOptions)
    {
        _database = database;
        _logger = logger;
        _filter = filter;
        _emailTemplate = emailTemplate;
        _smtpService = smtpService;
        _config = userAccountOptions.Value;
    }

    public async Task<int> Execute(
        IUserToken userToken,
        RegisterAccountCommandModel model)
    {
        if (!userToken.CanRegisterAccount(_config))
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (string.IsNullOrWhiteSpace(model.Email))
            throw new NotPermittedException();

        if (await _database.Users.AnyAsync(x => x.Name == model.Name))
            throw new NameAlreadyTakenException();

        if (_database.Users.Any(x => x.EmailAddress == model.Email))
            throw new EmailAlreadyTakenException();

        await _filter.Filter(model.Name);

        var user = new User()
        {
            Name = model.Name,
            EmailAddress = model.Email,
            IsHidden = model.IsHidden,
            Guid = Guid.NewGuid(),
        };

        if (await _database.Users
                .AnyAsync(x => x.Guid == user.Guid))
            throw new Exception("User.Guid collision.");

        var hasher = new PasswordHasher<User>();

        var hash = hasher.HashPassword(user, model.Password);

        user.PasswordHash = hash;

        _database.Users.Add(user);
        
        await _database.SaveAsync(userToken);

        var message = _emailTemplate.Create(user);

        try
        {
            _smtpService.SendMessage(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Error sending email to {Name} {Address}, in RegisterAccountCommand. Exception: {Exception}",
                    model.Name, model.Email, ex.Message);
        }

        return user.Id;
    }
}
