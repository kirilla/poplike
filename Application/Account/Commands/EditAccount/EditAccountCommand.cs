using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Account.Commands.EditAccount;

public class EditAccountCommand : IEditAccountCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public EditAccountCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(IUserToken userToken, EditAccountCommandModel model)
    {
        if (!userToken.CanEditAccount())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (string.IsNullOrWhiteSpace(model.Name) ||
            string.IsNullOrWhiteSpace(model.Email))
            throw new NotPermittedException();

        var user = await _database.Users
            .Where(x => x.Id == userToken.UserId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (await _database.Users.AnyAsync(x =>
            x.Name == model.Name &&
            x.Id != userToken.UserId!.Value))
            throw new NameAlreadyTakenException();

        if (await _database.Users.AnyAsync(x =>
            x.EmailAddress == model.Email &&
            x.Id != userToken.UserId!.Value))
            throw new EmailAlreadyTakenException();

        user.Name = model.Name;
        user.EmailAddress = model.Email;
        user.IsHidden = model.IsHidden;

        await _filter.Filter(model.Name);

        await _database.SaveAsync(userToken);
    }
}
