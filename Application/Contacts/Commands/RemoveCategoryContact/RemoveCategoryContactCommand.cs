namespace Poplike.Application.Contacts.Commands.RemoveCategoryContact;

public class RemoveCategoryContactCommand : IRemoveCategoryContactCommand
{
    private readonly IDatabaseService _database;

    public RemoveCategoryContactCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveCategoryContactCommandModel model)
    {
        if (!userToken.CanRemoveCategoryContact())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var contact = await _database.CategoryContacts
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.CategoryContacts.Remove(contact);

        await _database.SaveAsync(userToken);
    }
}
