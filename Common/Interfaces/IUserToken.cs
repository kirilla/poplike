namespace Poplike.Common.Interfaces;

public interface IUserToken
{
    int? UserId { get; }
    int? SessionId { get; }

    string? Name { get; }

    bool IsAuthenticated { get; }

    int? Request { get; }

    bool IsAdmin { get; }
    bool IsCurator { get; }
    bool IsModerator { get; }
}
