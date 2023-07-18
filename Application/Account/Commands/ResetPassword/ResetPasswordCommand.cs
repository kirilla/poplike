using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Poplike.Common.Settings;

namespace Poplike.Application.Account.Commands.ResetPassword;

public class ResetPasswordCommand : IResetPasswordCommand
{
    private readonly IDatabaseService _database;
    private readonly UserAccountConfiguration _config;

    public ResetPasswordCommand(
        IDatabaseService database,
        IOptions<UserAccountConfiguration> userAccountOptions)
    {
        _database = database;
        _config = userAccountOptions.Value;
    }

    public async Task Execute(IUserToken userToken, ResetPasswordCommandModel model)
    {
        if (!userToken.CanResetPassword(_config))
            throw new NotPermittedException();

        model.TrimStringProperties();

        if (model.Guid == null ||
            model.Guid == Guid.Empty)
            throw new InvalidDataException();

        var request = await _database.PasswordResetRequests
            .Include(x => x.User)
            .Where(x =>
                x.Guid == model.Guid &&
                x.User.EmailAddress == model.Email)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (request.User == null)
            throw new NotFoundException();

        if (string.IsNullOrWhiteSpace(model.Password))
            throw new InvalidDataException();

        var hasher = new PasswordHasher<User>();

        var hash = hasher.HashPassword(request.User, model.Password);

        request.User.PasswordHash = hash;

        _database.PasswordResetRequests.Remove(request);

        await _database.SaveAsync(userToken);
    }
}
