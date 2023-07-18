using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages;

[AllowAnonymous]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : UserTokenPageModel
{
    public ErrorModel(
        IUserToken userToken)
        :
        base(PageKind.Error, userToken)
    {
    }

    public void OnGet()
    {
    }
}
