namespace Poplike.Application.Keywords.Commands.RemoveKeyword;

public class RemoveKeywordCommand : IRemoveKeywordCommand
{
    private readonly IDatabaseService _database;

    public RemoveKeywordCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveKeywordCommandModel model)
    {
        if (!userToken.CanRemoveKeyword())
            throw new NotPermittedException();

        var keyword = await _database.Keywords
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Keywords.Remove(keyword);

        await _database.SaveAsync(userToken);
    }
}
