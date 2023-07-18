namespace Poplike.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool Confirmed { get; set; }
}
