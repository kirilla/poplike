using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Contacts.Commands.EditCategoryContact;

public class EditCategoryContactCommand : IEditCategoryContactCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public EditCategoryContactCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(
        IUserToken userToken, EditCategoryContactCommandModel model)
    {
        if (!userToken.CanEditCategoryContact())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var contact = await _database.CategoryContacts
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var contacts = await _database.CategoryContacts
            .Where(x => x.CategoryId == contact.CategoryId)
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
