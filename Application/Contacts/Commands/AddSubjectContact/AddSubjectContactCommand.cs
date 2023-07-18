using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Contacts.Commands.AddSubjectContact;

public class AddSubjectContactCommand : IAddSubjectContactCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public AddSubjectContactCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddSubjectContactCommandModel model)
    {
        if (!userToken.CanAddSubjectContact())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var user = await _database.Users
            .Where(x => x.Id == userToken.UserId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var subject = await _database.Subjects
            .Where(x => x.Id == model.SubjectId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var contacts = await _database.SubjectContacts
            .Where(x => x.SubjectId == model.SubjectId!.Value)
            .ToListAsync();

        if (contacts.Any(x => x.Name == model.Name))
            throw new BlockedByExistingException();

        var contact = new SubjectContact()
        {
            SubjectId = subject.Id,
            Name = model.Name,
            PhoneNumber = model.PhoneNumber,
            EmailAddress = model.EmailAddress,
            Url = model.Url,
        };

        _database.SubjectContacts.Add(contact);

        await _filter.Filter(model.Name);

        await _database.SaveAsync(userToken);

        return contact.Id;
    }
}
