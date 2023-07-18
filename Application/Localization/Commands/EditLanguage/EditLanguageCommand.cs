using NUnit.Framework.Internal.Execution;

namespace Poplike.Application.Localization.Commands.EditLanguage;

public class EditLanguageCommand : IEditLanguageCommand
{
    private readonly IDatabaseService _database;

    public EditLanguageCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, EditLanguageCommandModel model)
    {
        if (!userToken.CanEditLanguage())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (await _database.Languages
            .AnyAsync(x =>
                x.Name == model.Name &&
                x.Id != model.Id))
            throw new BlockedByExistingException();

        var language = await _database.Languages
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        language.Name = model.Name;
        language.Culture = model.Culture;
        language.Emoji = model.Emoji;

        await _database.SaveAsync(userToken);
    }
}
