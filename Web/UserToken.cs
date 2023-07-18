namespace Poplike.Web;

public class UserToken : IUserToken
{
    private readonly IHttpContextAccessor? _httpContext;

    public int? UserId { get; }
    public int? SessionId { get; }

    public string? Name { get; }

    public bool IsAuthenticated { get; }

    public int? Request { get; }

    public bool IsAdmin { get; }
    public bool IsCurator { get; }
    public bool IsModerator { get; }

    public UserToken(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;

        UserId = _httpContext?.HttpContext?.Items?["UserId"] as int?;
        SessionId = _httpContext?.HttpContext?.Items?["SessionId"] as int?;

        Name = _httpContext?.HttpContext?.Items?["UserName"] as string;

        IsAdmin = _httpContext?.HttpContext?.Items?["IsAdmin"] as bool? ?? false;
        IsCurator = _httpContext?.HttpContext?.Items?["IsCurator"] as bool? ?? false;
        IsModerator = _httpContext?.HttpContext?.Items?["IsModerator"] as bool? ?? false;

        IsAuthenticated = UserId.HasValue && SessionId.HasValue;

        if (IsAuthenticated)
        {
            Request = new Random((int)DateTime.Now.Ticks).Next();
        }
    }
}
