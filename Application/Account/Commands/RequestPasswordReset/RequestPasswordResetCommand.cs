using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Poplike.Common.Settings;

namespace Poplike.Application.Account.Commands.RequestPasswordReset;

public class RequestPasswordResetCommand : IRequestPasswordResetCommand
{
    private readonly IDatabaseService _database;
    private readonly IDateService _dateService;
    private readonly ILogger<RequestPasswordResetCommand> _logger;
    private readonly IPasswordResetEmailTemplate _emailTemplate;
    private readonly ISmtpService _smtpService;
    private readonly UserAccountConfiguration _config;

    public RequestPasswordResetCommand(
        IDatabaseService database,
        IDateService dateService,
        ILogger<RequestPasswordResetCommand> logger,
        IPasswordResetEmailTemplate emailTemplate,
        ISmtpService smtpService,
        IOptions<UserAccountConfiguration> userAccountOptions)
    {
        _database = database;
        _dateService = dateService;
        _logger = logger;
        _emailTemplate = emailTemplate;
        _smtpService = smtpService;
        _config = userAccountOptions.Value;
    }

    public async Task Execute(IUserToken userToken, RequestPasswordResetCommandModel model)
    {
        if (!userToken.CanRequestPasswordReset(_config))
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (string.IsNullOrWhiteSpace(model.Email))
            throw new NotPermittedException();

        var user = await _database.Users
            .Where(x => x.EmailAddress == model.Email)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (await _database.PasswordResetRequests
                .AnyAsync(x => x.User.EmailAddress == model.Email))
            throw new PreexistingPasswordResetRequestException();

        var request = new PasswordResetRequest()
        {
            UserId = user.Id,
            Guid = Guid.NewGuid(),
            Created = _dateService.GetDateTimeNow(),
        };

        _database.PasswordResetRequests.Add(request);

        await _database.SaveAsync(userToken);

        var email = _emailTemplate.Create(
            request.User, request);

        try
        {
            _smtpService.SendMessage(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Error sending email to {Address}, in RequestPasswordResetCommand. Exception: {Exception}",
                    model.Email, ex.Message);
        }
    }
}
