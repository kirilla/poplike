namespace Poplike.Application.Users.Commands.EditUser;

public interface IEditUserCommand
{
    Task Execute(IUserToken userToken, EditUserCommandModel model);
}
