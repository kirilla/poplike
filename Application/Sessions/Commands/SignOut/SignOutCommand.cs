namespace Poplike.Application.Sessions.Commands.SignOut;

public class SignOutCommand : ISignOutCommand
{
    private readonly IDatabaseService _database;

    public SignOutCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(IUserToken userToken, SignOutCommandModel model)
    {
        if (!userToken.CanSignOut())
            throw new NotPermittedException();

        var session = await _database.Sessions
            .Where(x =>
                x.Guid == model.SessionGuid &&
                x.User.Guid == model.UserGuid)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Sessions.Remove(session);

        await _database.SaveAsync(userToken);
    }
}
