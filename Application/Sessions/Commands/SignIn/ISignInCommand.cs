namespace Poplike.Application.Sessions.Commands.SignIn;

public interface ISignInCommand
{
    Task<SessionGuidResultModel> Execute(IUserToken userToken, SignInCommandModel model);
}
