namespace Poplike.Application.Invitations.Commands.AcceptInvitation;

public interface IAcceptInvitationCommand
{
    Task Execute(IUserToken userToken, AcceptInvitationCommandModel model);
}
