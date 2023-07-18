namespace Poplike.Application.Contacts.Commands.RemoveSubjectContact;

public class RemoveSubjectContactCommand : IRemoveSubjectContactCommand
{
    private readonly IDatabaseService _database;

    public RemoveSubjectContactCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveSubjectContactCommandModel model)
    {
        if (!userToken.CanRemoveSubjectContact())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var expression = await _database.SubjectContacts
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.SubjectContacts.Remove(expression);

        await _database.SaveAsync(userToken);
    }
}
