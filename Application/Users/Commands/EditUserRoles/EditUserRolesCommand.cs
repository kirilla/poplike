namespace Poplike.Application.Users.Commands.EditUserRoles;

public class EditUserRolesCommand : IEditUserRolesCommand
{
    private readonly IDatabaseService _database;

    public EditUserRolesCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(IUserToken userToken, EditUserRolesCommandModel model)
    {
        if (!userToken.CanEditUserRoles())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var user = await _database.Users
            .Where(x => x.Id == model.UserId)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (user.Id == userToken.UserId)
        {
            if (user.IsAdmin == true &&
                model.IsAdmin == false)
                throw new NotPermittedException(
                    "User can not drop their Admin role");

            if (user.IsAdmin == false && 
                model.IsAdmin == true)
                throw new NotPermittedException(
                    "User can not give themself Admin role");
        }

        user.IsAdmin = model.IsAdmin;
        user.IsModerator = model.IsModerator;
        user.IsCurator = model.IsCurator;

        await _database.SaveAsync(userToken);
    }
}
