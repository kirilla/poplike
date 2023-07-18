namespace Poplike.Application.Users.Commands.EditUserRoles;

public interface IEditUserRolesCommand
{
    Task Execute(IUserToken userToken, EditUserRolesCommandModel model);
}
