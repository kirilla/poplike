namespace Poplike.Application.Invitations.Commands.RejectInvitation;

public class RejectInvitationCommand : IRejectInvitationCommand
{
    private readonly IDatabaseService _database;
    private readonly IDateService _dateService;

    public RejectInvitationCommand(
        IDatabaseService database,
        IDateService dateService)
    {
        _database = database;
        _dateService = dateService;
    }

    public async Task Execute(IUserToken userToken, RejectInvitationCommandModel model)
    {
        if (!userToken.CanRejectInvitation())
            throw new NotPermittedException();

        if (model.Guid == null ||
            model.Guid == Guid.Empty)
            throw new InvalidDataException();

        var invitation = await _database.Invitations
            .Include(x => x.SignUp)
            .Where(x => x.Guid == model.Guid)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var signups = await _database.SignUps
            .Where(x => x.EmailAddress == invitation.SignUp.EmailAddress)
            .ToListAsync();

        _database.SignUps.RemoveRange(signups);

        await _database.SaveAsync(userToken);
    }
}
