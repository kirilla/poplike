namespace Poplike.Application.Account.Commands.ResetPassword;

public interface IResetPasswordCommand
{
    Task Execute(IUserToken userToken, ResetPasswordCommandModel model);
}
