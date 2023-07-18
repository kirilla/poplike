namespace Poplike.Application.Localization.Commands.RemoveLanguage;

public class RemoveLanguageCommand : IRemoveLanguageCommand
{
    private readonly IDatabaseService _database;

    public RemoveLanguageCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveLanguageCommandModel model)
    {
        if (!userToken.CanRemoveLanguage())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var language = await _database.Languages
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Languages.Remove(language);

        await _database.SaveAsync(userToken);
    }
}
