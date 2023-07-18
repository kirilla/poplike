using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Poplike.Common.Settings;

namespace Poplike.Application.Sessions.Commands.SignIn;

public class SignInCommand : ISignInCommand
{
    private readonly IDatabaseService _database;
    private readonly UserAccountConfiguration _config;

    public SignInCommand(
        IDatabaseService database,
        IOptions<UserAccountConfiguration> userAccountOptions)
    {
        _database = database;
        _config = userAccountOptions.Value;
    }

    public async Task<SessionGuidResultModel> Execute(IUserToken userToken, SignInCommandModel model)
    {
        if (!userToken.CanSignIn(_config))
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (string.IsNullOrWhiteSpace(model.Email))
            throw new NotPermittedException();

        var user = await _database.Users
            .Where(x => x.EmailAddress == model.Email)
            .SingleOrDefaultAsync() ??
            throw new UserNotFoundException();

        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(
            user, user.PasswordHash, model.Password);

        switch (result)
        {
            case PasswordVerificationResult.Success:
                // continue
                break;

            case PasswordVerificationResult.SuccessRehashNeeded:
                user.PasswordHash = hasher.HashPassword(user, model.Password);
                // continue
                break;

            case PasswordVerificationResult.Failed:
                throw new PasswordVerificationFailedException();

            default:
                throw new Exception("Unknown password verification result.");
        }

        var session = new Session()
        {
            UserId = user.Id,
            Guid = Guid.NewGuid(),
        };

        _database.Sessions.Add(session);

        await _database.SaveAsync(userToken);

        return new SessionGuidResultModel()
        {
            UserGuid = user.Guid!.Value,
            SessionGuid = session.Guid!.Value,
        };
    }
}
