namespace Poplike.Application.Blurbs.Commands.RemoveCategoryBlurb;

public class RemoveCategoryBlurbCommand : IRemoveCategoryBlurbCommand
{
    private readonly IDatabaseService _database;

    public RemoveCategoryBlurbCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveCategoryBlurbCommandModel model)
    {
        if (!userToken.CanRemoveCategoryBlurb())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var blurb = await _database.CategoryBlurbs
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.CategoryBlurbs.Remove(blurb);

        await _database.SaveAsync(userToken);
    }
}
