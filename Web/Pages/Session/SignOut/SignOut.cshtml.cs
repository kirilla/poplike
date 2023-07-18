using Poplike.Application.Sessions.Commands.SignOut;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Poplike.Web.Pages.Session.SignOut;

public class SignOutModel : UserTokenPageModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISignOutCommand _signOutCommand;

    public SignOutModel(
        IHttpContextAccessor httpContextAccessor,
        ISignOutCommand signOutCommand,
        IUserToken userToken)
        :
        base(PageKind.SignOut, userToken)
    {
        _httpContextAccessor = httpContextAccessor;
        _signOutCommand = signOutCommand;
    }

    public IActionResult OnGet()
    {
        try
        {
            if (!User?.Identity?.IsAuthenticated ?? false)
                return LocalRedirect("/");

            if (!UserToken.CanSignOut())
                throw new NotPermittedException();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            //if (!UserToken.CanSignOut())
            //  throw new NotPermittedException();

            var items = _httpContextAccessor.HttpContext?.Items;

            var userGuid = items?["UserGuid"]?.ToString();
            var sessionGuid = items?["SessionGuid"]?.ToString();

            var model = new SignOutCommandModel()
            {
                UserGuid = new Guid(userGuid!),
                SessionGuid = new Guid(sessionGuid!),
            };

            await _signOutCommand.Execute(UserToken, model);

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return LocalRedirect("/");
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
