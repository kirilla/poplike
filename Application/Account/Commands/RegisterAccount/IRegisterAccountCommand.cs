namespace Poplike.Application.Account.Commands.RegisterAccount;

public interface IRegisterAccountCommand
{
    Task<int> Execute(IUserToken userToken, RegisterAccountCommandModel model);
}
