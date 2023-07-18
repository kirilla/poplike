using NUnit.Framework.Internal.Execution;

namespace Poplike.Application.Legal.Commands.EditWord;

public class EditWordCommand : IEditWordCommand
{
    private readonly IDatabaseService _database;

    public EditWordCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, EditWordCommandModel model)
    {
        if (!userToken.CanEditWord())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (await _database.Words
            .AnyAsync(x => 
                x.Value == model.Value &&
                x.Id != model.Id))
            throw new BlockedByExistingException();

        var word = await _database.Words
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        word.Value = model.Value;

        await _database.SaveAsync(userToken);
    }
}
