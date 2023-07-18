namespace Poplike.Application.Auth;

public class NoUserToken : IUserToken
{
    public int? UserId { get; }
    public int? SessionId { get; }

    public string? Name { get; }

    public bool IsAuthenticated { get; }

    public int? Request { get; }

    public bool IsAdmin { get; }
    public bool IsCurator { get; }
    public bool IsModerator { get; }

    public NoUserToken()
    {
        UserId = null;
        SessionId = null;

        Name = null;

        IsAdmin = false;
        IsCurator = false;
        IsModerator = false;

        IsAuthenticated = false;

        Request = null;
    }
}
