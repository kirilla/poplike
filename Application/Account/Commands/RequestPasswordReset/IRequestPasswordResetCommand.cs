namespace Poplike.Application.Account.Commands.RequestPasswordReset;

public interface IRequestPasswordResetCommand
{
    Task Execute(IUserToken userToken, RequestPasswordResetCommandModel model);
}
