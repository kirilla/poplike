namespace Poplike.Application.Account.Commands.DeleteAccount;

public class DeleteAccountCommand : IDeleteAccountCommand
{
    private readonly IDatabaseService _database;

    public DeleteAccountCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(IUserToken userToken, DeleteAccountCommandModel model)
    {
        if (!userToken.CanDeleteAccount())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var user = await _database.Users
            .Where(x => x.Id == userToken.UserId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Users.Remove(user);

        await _database.SaveAsync(userToken);
    }
}
