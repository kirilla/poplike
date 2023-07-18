namespace Poplike.Application.Account.Commands.ChangePassword;

public interface IChangePasswordCommand
{
    Task Execute(IUserToken userToken, ChangePasswordCommandModel model);
}
