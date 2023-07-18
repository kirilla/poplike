using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Contacts.Commands.AddCategoryContact;

public class AddCategoryContactCommand : IAddCategoryContactCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public AddCategoryContactCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddCategoryContactCommandModel model)
    {
        if (!userToken.CanAddCategoryContact())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var user = await _database.Users
            .Where(x => x.Id == userToken.UserId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var category = await _database.Categories
            .Where(x => x.Id == model.CategoryId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var contacts = await _database.CategoryContacts
            .Where(x => x.CategoryId == model.CategoryId!.Value)
            .ToListAsync();

        if (contacts.Any(x => x.Name == model.Name))
            throw new BlockedByExistingException();

        var contact = new CategoryContact()
        {
            CategoryId = category.Id,
            Name = model.Name,
            PhoneNumber = model.PhoneNumber,
            EmailAddress = model.EmailAddress,
            Url = model.Url,
        };

        _database.CategoryContacts.Add(contact);

        await _filter.Filter(model.Name);

        await _database.SaveAsync(userToken);

        return contact.Id;
    }
}
