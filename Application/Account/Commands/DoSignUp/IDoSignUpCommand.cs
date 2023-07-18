namespace Poplike.Application.Account.Commands.DoSignUp;

public interface IDoSignUpCommand
{
    Task Execute(IUserToken userToken, DoSignUpCommandModel model);
}
