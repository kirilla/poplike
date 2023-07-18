namespace Poplike.Application.Sessions.Commands.SignOut;

public class SignOutCommandModel
{
    public Guid UserGuid { get; set; }
    public Guid SessionGuid { get; set; }
}
