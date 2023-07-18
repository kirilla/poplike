namespace Poplike.Application.Users.Commands.DeleteUser;

public interface IDeleteUserCommand
{
    Task Execute(IUserToken userToken, DeleteUserCommandModel model);
}
