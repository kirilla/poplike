namespace Poplike.Application.Legal.Commands.RemoveWord;

public class RemoveWordCommand : IRemoveWordCommand
{
    private readonly IDatabaseService _database;

    public RemoveWordCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveWordCommandModel model)
    {
        if (!userToken.CanRemoveWord())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var word = await _database.Words
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Words.Remove(word);

        await _database.SaveAsync(userToken);
    }
}
