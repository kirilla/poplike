namespace Poplike.Application.Account.Commands.EditAccount;

public interface IEditAccountCommand
{
    Task Execute(IUserToken userToken, EditAccountCommandModel model);
}
