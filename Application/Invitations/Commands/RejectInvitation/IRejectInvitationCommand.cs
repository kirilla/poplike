namespace Poplike.Application.Invitations.Commands.RejectInvitation;

public interface IRejectInvitationCommand
{
    Task Execute(IUserToken userToken, RejectInvitationCommandModel model);
}
