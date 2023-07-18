namespace Poplike.Application.Users.Commands.EditUser;

public class EditUserCommand : IEditUserCommand
{
    private readonly IDatabaseService _database;

    public EditUserCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(IUserToken userToken, EditUserCommandModel model)
    {
        if (!userToken.CanEditUser())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var user = await _database.Users
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (await _database.Users.AnyAsync(x =>
            x.Name == model.Name &&
            x.Id != model.Id))
            throw new NameAlreadyTakenException();

        user.Name = model.Name;

        await _database.SaveAsync(userToken);
    }
}
