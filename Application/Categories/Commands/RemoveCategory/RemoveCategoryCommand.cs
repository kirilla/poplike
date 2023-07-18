namespace Poplike.Application.Categories.Commands.RemoveCategory;

public class RemoveCategoryCommand : IRemoveCategoryCommand
{
    private readonly IDatabaseService _database;

    public RemoveCategoryCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveCategoryCommandModel model)
    {
        if (!userToken.CanRemoveCategory())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var category = await _database.Categories
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Categories.Remove(category);

        await _database.SaveAsync(userToken);
    }
}
