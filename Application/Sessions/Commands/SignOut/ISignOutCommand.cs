namespace Poplike.Application.Sessions.Commands.SignOut;

public interface ISignOutCommand
{
    Task Execute(IUserToken userToken, SignOutCommandModel model);
}
