namespace Poplike.Application.Legal.Commands.AddWord;

public class AddWordCommand : IAddWordCommand
{
    private readonly IDatabaseService _database;

    public AddWordCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddWordCommandModel model)
    {
        if (!userToken.CanAddWord())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (await _database.Words
            .AnyAsync(x => x.Value == model.Value))
            throw new BlockedByExistingException();

        var word = new Word()
        {
            Value = model.Value,
        };

        _database.Words.Add(word);

        await _database.SaveAsync(userToken);

        return word.Id;
    }
}
