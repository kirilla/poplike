namespace Poplike.Application.Account.Commands.DeleteAccount;

public interface IDeleteAccountCommand
{
    Task Execute(IUserToken userToken, DeleteAccountCommandModel model);
}
