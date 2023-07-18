using Poplike.Application.Sessions.Queues;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Poplike.Web;

public class CookieValidator : CookieAuthenticationEvents
{
    private readonly IDatabaseService _database;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISessionActivityList _sessionActivityList;

    public CookieValidator(
        IDatabaseService database,
        IHttpContextAccessor httpContextAccessor,
        ISessionActivityList sessionActivityList)
    {
        _database = database;
        _httpContextAccessor = httpContextAccessor;
        _sessionActivityList = sessionActivityList;
    }

    public override async Task ValidatePrincipal(
        CookieValidatePrincipalContext context)
    {
        var userPrincipal = context.Principal;

        var userGuid = userPrincipal?.Claims
            .Where(x => x.Type == "UserGuid")
            .Select(x => x.Value)
            .FirstOrDefault();

        var sessionGuid = userPrincipal?.Claims
            .Where(x => x.Type == "SessionGuid")
            .Select(x => x.Value)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(userGuid) ||
            string.IsNullOrEmpty(sessionGuid))
        {
            context.RejectPrincipal();

            await context.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return;
        }

        var session = _database.Sessions
            .Where(x =>
                x.Guid.ToString() == sessionGuid &&
                x.User.Guid.ToString() == userGuid)
            .Select(x => new
            {
                SessionId = x.Id,
                x.UserId,
                x.User.IsAdmin,
                x.User.IsCurator,
                x.User.IsModerator,
                x.User.Name,
            })
            .SingleOrDefault();

        if (session == null)
        {
            context.RejectPrincipal();

            await context.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return;
        }

        _sessionActivityList.AddSessionId(session.SessionId);

        var items = _httpContextAccessor.HttpContext?.Items;

        items!["UserGuid"] = userGuid;
        items!["SessionGuid"] = sessionGuid;

        items!["UserId"] = session.UserId;
        items!["SessionId"] = session.SessionId;

        items!["UserName"] = session.Name;

        items!["IsAdmin"] = session.IsAdmin;
        items!["IsCurator"] = session.IsCurator;
        items!["IsModerator"] = session.IsModerator;
    }
}
