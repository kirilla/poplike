namespace Poplike.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IDeleteUserCommand
{
    private readonly IDatabaseService _database;

    public DeleteUserCommand(
        IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(IUserToken userToken, DeleteUserCommandModel model)
    {
        if (!userToken.CanDeleteUser())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        if (model.Id == userToken.UserId)
            throw new Exception("Self-deletion not permitted.");

        var user = await _database.Users
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Users.Remove(user);

        await _database.SaveAsync(userToken);
    }
}
