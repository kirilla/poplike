using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Contacts.Commands.EditSubjectContact;

public class EditSubjectContactCommand : IEditSubjectContactCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public EditSubjectContactCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(
        IUserToken userToken, EditSubjectContactCommandModel model)
    {
        if (!userToken.CanEditSubjectContact())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var contact = await _database.SubjectContacts
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var contacts = await _database.SubjectContacts
            .Where(x => x.SubjectId == contact.SubjectId)
            .ToListAsync();

        if (contacts.Any(x =>
            x.Name == model.Name &&
            x.Id != model.Id))
            throw new BlockedByExistingException();

        contact.Name = model.Name;
        contact.PhoneNumber = model.PhoneNumber;
        contact.EmailAddress = model.EmailAddress;
        contact.Url = model.Url;

        await _filter.Filter(model.Name);

        await _database.SaveAsync(userToken);
    }
}
