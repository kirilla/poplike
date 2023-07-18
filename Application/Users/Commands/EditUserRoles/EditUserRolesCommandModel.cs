namespace Poplike.Application.Users.Commands.EditUserRoles;

public class EditUserRolesCommandModel
{
    public int UserId { get; set; }

    public bool IsAdmin { get; set; }
    public bool IsCurator { get; set; }
    public bool IsModerator { get; set; }
}
