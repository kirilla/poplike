using Microsoft.AspNetCore.Identity;
using Poplike.Application.Sessions.Commands.SignIn;

namespace Poplike.Application.Account.Commands.ChangePassword;

public class ChangePasswordCommand : IChangePasswordCommand
{
    private readonly IDatabaseService _database;

    public ChangePasswordCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(IUserToken userToken, ChangePasswordCommandModel model)
    {
        if (!userToken.CanChangePassword())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (model.NewPassword == model.OldPassword)
            throw new InvalidDataException();

        if (string.IsNullOrWhiteSpace(model.OldPassword) ||
            string.IsNullOrWhiteSpace(model.NewPassword))
            throw new InvalidDataException();

        var user = await _database.Users
            .Where(x => x.Id == userToken.UserId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(
            user, user.PasswordHash, model.OldPassword);

        if (result != PasswordVerificationResult.Success &&
            result != PasswordVerificationResult.SuccessRehashNeeded)
            throw new PasswordVerificationFailedException();

        var hash = hasher.HashPassword(user, model.NewPassword);

        user.PasswordHash = hash;

        await _database.SaveAsync(userToken);
    }
}
