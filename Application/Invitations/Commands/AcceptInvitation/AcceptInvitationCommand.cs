using Microsoft.AspNetCore.Identity;
using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Invitations.Commands.AcceptInvitation;

public class AcceptInvitationCommand : IAcceptInvitationCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public AcceptInvitationCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(IUserToken userToken, AcceptInvitationCommandModel model)
    {
        if (!userToken.CanAcceptInvitation())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (model.Guid == null ||
            model.Guid == Guid.Empty)
            throw new InvalidDataException();

        var invitation = await _database.Invitations
            .Include(x => x.SignUp)
            .Where(x => x.Guid == model.Guid)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (await _database.Users.AnyAsync(x => x.Name == model.Name))
            throw new NameAlreadyTakenException();

        if (await _database.Users.AnyAsync(x => 
            x.EmailAddress == invitation.SignUp.EmailAddress))
            throw new EmailAlreadyTakenException();

        await _filter.Filter(model.Name);

        var user = new User()
        {
            Name = model.Name,
            EmailAddress = invitation.SignUp.EmailAddress,
            IsHidden = model.IsHidden,
            Guid = Guid.NewGuid(),
        };

        if (await _database.Users.AnyAsync(x => x.Guid == user.Guid))
            throw new Exception("User.Guid collision.");

        var hasher = new PasswordHasher<User>();

        var hash = hasher.HashPassword(user, model.Password);

        user.PasswordHash = hash;

        _database.Users.Add(user);
        
        var invitations = await _database.SignUps
            .Where(x => x.EmailAddress == invitation.SignUp.EmailAddress)
            .ToListAsync();

        _database.SignUps.RemoveRange(invitations);

        await _database.SaveAsync(userToken);
    }
}
