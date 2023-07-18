using static Poplike.Common.Validation.MaxLengths.Common;

namespace Poplike.Application.Localization.Commands.AddLanguage;

public class AddLanguageCommand : IAddLanguageCommand
{
    private readonly IDatabaseService _database;

    public AddLanguageCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddLanguageCommandModel model)
    {
        if (!userToken.CanAddLanguage())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (await _database.Languages
            .AnyAsync(x => x.Name == model.Name))
            throw new BlockedByExistingException();

        var language = new Language()
        {
            Name = model.Name,
            Culture = model.Culture,
            Emoji = model.Emoji,
        };

        _database.Languages.Add(language);

        await _database.SaveAsync(userToken);

        return language.Id;
    }
}
